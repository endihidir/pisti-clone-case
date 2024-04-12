using Sirenix.Utilities;
using UnityBase.EventBus;
using UnityBase.Manager;
using UnityBase.Manager.Data;
using UnityBase.ManagerSO;
using UnityBase.Service;
using UnityBase.StateMachineCore;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayStateMachine : IStateMachine, ITickable, IGameplayConstructorService
{
    [Inject]
    private readonly IGameplayDataService _gameplayDataService;
    
    private readonly GameplayStateMachineSO _gameplayStateMachineSo;
    private readonly int _opponentCount;

    private readonly IDiscardBoard _discardBoard;
    private readonly IState _idleState, _playerMoveState, _cardDistributionState, _resultCalculationState;
    private readonly IState[] _opponentMoveStates;
    
    private readonly IUserBoard _playerBoard;
    private readonly IUserBoard[] _opponentBoards;

    private readonly EventBinding<GameStateData> _gameStateBinding;
    private GameState _currentGameState;
    
    private IState _currentGameplayState;
    public IState CurrentGameplayState => _currentGameplayState;
    private bool IsLastOpponentCardsFinished => ((OpponentMoveState)_opponentMoveStates[^1]).IsCardsFinished;
    
    public GameplayStateMachine(GameplayDataHolderSO gameplayDataHolderSo, IDiscardBoard discardBoard, ICardContainer cardContainer)
    {
        _gameplayStateMachineSo = gameplayDataHolderSo.gameplayStateMachineSo;
        _opponentCount = _gameplayStateMachineSo.GetOpponentCount();
        _discardBoard = discardBoard;

        var playerBoard = _gameplayStateMachineSo.GetDeckView<PlayerBoardView>();
        _playerBoard = new UserBoardController(0, new UserDeckController(playerBoard.Slots), new CollectedCardsContainer(playerBoard.CardCollectingArea));
        _playerMoveState = new PlayerMoveState(_playerBoard, _discardBoard);

        _opponentBoards = new IUserBoard[_opponentCount];
        _opponentMoveStates = new IState[_opponentCount];

        for (int i = 0; i < _opponentCount; i++)
        {
            var userID = i + 1;
            var opponentBoard = _gameplayStateMachineSo.GetOpponentDeckViewBy(userID);
            _opponentBoards[i] = new UserBoardController(userID, new UserDeckController(opponentBoard.Slots), new CollectedCardsContainer(opponentBoard.CardCollectingArea));
            _opponentMoveStates[i] = new OpponentMoveState(_opponentBoards[i], _discardBoard);
        }

        _idleState = new IdleState();
        _cardDistributionState = new CardDistributionState(cardContainer, _playerBoard, _opponentBoards, _discardBoard);
        _resultCalculationState = new ResultCalculationState(_playerBoard, _opponentBoards);
        
        _gameStateBinding = new EventBinding<GameStateData>();

        ChangeState(_idleState);
    }

    public void Initialize()
    {
        _playerMoveState.OnStateComplete += OnPlayerMoveStateComplete;
        
        for (int i = 0; i < _opponentCount; i++) 
            _opponentMoveStates[i].OnStateComplete += OnOpponentMoveStateComplete;

        _cardDistributionState.OnStateComplete += OnDistributionStateComplete;
        _resultCalculationState.OnStateComplete += OnResultCalculated;
        
        _gameStateBinding.Add(OnGameplayStateChanged);
        EventBus<GameStateData>.AddListener(_gameStateBinding, GameStateData.GetChannel(TransitionState.Middle));
    }

    private void OnGameplayStateChanged(GameStateData gameStateData)
    {
        var startState = gameStateData.StartState;
        _currentGameState = gameStateData.EndState;

        if (startState == GameState.GameLoadingState && _currentGameState == GameState.GamePlayState)
        {
            ChangeState(_cardDistributionState);
        }
    }

    private void OnPlayerMoveStateComplete() => ChangeState(_opponentMoveStates[0]);

    private void OnOpponentMoveStateComplete()
    {
        if (_currentGameplayState is not OpponentMoveState currentOpponent)
        {
            Debug.LogError("Current state can not cast to Opponent State");
            return;
        }
        
        var nextOpponentId = currentOpponent.UserID + 1;

        if (IsLastOpponentCardsFinished)
        {
            ChangeState(_cardDistributionState);
        }
        else
        {
            var nextState = nextOpponentId > _opponentCount ? _playerMoveState : _opponentMoveStates[nextOpponentId - 1];
            
            ChangeState(nextState);
        }
    }
    
    private void OnDistributionStateComplete() => ChangeState(IsLastOpponentCardsFinished ? _resultCalculationState : _playerMoveState);

    private void OnResultCalculated()
    {
        var isPlayerWin = ((ResultCalculationState)_resultCalculationState).IsPlayerWin;
        _gameplayDataService.ChangeGameState(isPlayerWin ? GameState.GameSuccessState : GameState.GameFailState, 1f);
    }

    public void Tick() => _currentGameplayState?.OnUpdate(Time.deltaTime);

    public void ChangeState(IState state)
    {
        if (_currentGameplayState == state)
        {
            Debug.LogError("You can not set same state!");
            return;
        }
        
        _currentGameplayState?.OnExit();
        _currentGameplayState = state;
        _currentGameplayState.OnEnter();
    }
    
    public void Dispose()
    {
        _playerMoveState.OnStateComplete -= OnPlayerMoveStateComplete;
        
        for (int i = 0; i < _opponentCount; i++) 
            _opponentMoveStates[i].OnStateComplete -= OnOpponentMoveStateComplete;
        
        _cardDistributionState.OnStateComplete -= OnDistributionStateComplete;
        
        _gameStateBinding.Remove(OnGameplayStateChanged);
        EventBus<GameStateData>.RemoveListener(_gameStateBinding, GameStateData.GetChannel(TransitionState.Middle));
        
        _playerBoard.Reset();
        _opponentBoards.ForEach(x => x.Reset());
    }
}