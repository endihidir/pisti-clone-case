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

    private readonly IState _idleState, _playerMoveState, _cardDealingState, _resultCalculationState;
    private readonly IState[] _opponentMoveStates;
    
    private readonly IUserBoard _playerBoard;
    private readonly IUserBoard[] _opponentBoards;

    private readonly GameRoundViewHandler _gameRoundViewHandler;
    
    private readonly EventBinding<GameStateData> _gameStateBinding;
    private GameState _currentGameState;
    
    private IState _currentGameplayState;

    public IState CurrentGameplayState => _currentGameplayState;
    private bool IsLastOpponentCardsFinished => ((OpponentMoveState)_opponentMoveStates[^1]).IsCardsFinished;
    
    public GameplayStateMachine(GameplayDataHolderSO gameplayDataHolderSo, IDiscardPile discardPile, ICardContainer cardContainer)
    {
        _gameplayStateMachineSo = gameplayDataHolderSo.gameplayStateMachineSo;
        _opponentCount = _gameplayStateMachineSo.GetOpponentCount();
        _gameStateBinding = new EventBinding<GameStateData>();

        _idleState = new IdleState();
        
        var playerBoard = _gameplayStateMachineSo.GetBoardView<PlayerBoardView>();
        _playerBoard = new UserBoardController(0, new UserDeckController(playerBoard), new CollectedCardsContainer(playerBoard));
        _playerMoveState = new PlayerMoveState(_playerBoard, discardPile);

        _opponentBoards = new IUserBoard[_opponentCount];
        _opponentMoveStates = new IState[_opponentCount];

        for (int i = 0; i < _opponentCount; i++)
        {
            var userID = i + 1;
            var opponentBoard = _gameplayStateMachineSo.GetOpponentBoardViewBy(userID);
            _opponentBoards[i] = new UserBoardController(userID, new UserDeckController(opponentBoard), new CollectedCardsContainer(opponentBoard));
            _opponentMoveStates[i] = new OpponentMoveState(_opponentBoards[i], discardPile);
        }
        
        _cardDealingState = new CardDealingState(cardContainer, _playerBoard, _opponentBoards, discardPile);
        _resultCalculationState = new ResultCalculationState(_playerBoard, _opponentBoards);

        ChangeState(_idleState);
        
        _gameRoundViewHandler = new GameRoundViewHandler(_gameplayStateMachineSo.gameRoundView, cardContainer, _opponentCount);
    }

    public void Initialize()
    {
        _playerMoveState.OnStateComplete += OnPlayerMoveStateComplete;
        
        for (int i = 0; i < _opponentCount; i++) 
            _opponentMoveStates[i].OnStateComplete += OnOpponentMoveStateComplete;

        _cardDealingState.OnStateComplete += OnDealingStateComplete;
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
            ChangeState(_cardDealingState);
        }
    }

    private void OnPlayerMoveStateComplete() => ChangeState(_opponentMoveStates[0]);

    private void OnOpponentMoveStateComplete()
    {
        if (_currentGameplayState is OpponentMoveState currentOpponent)
        {
            var nextOpponentId = currentOpponent.UserID + 1;

            if (IsLastOpponentCardsFinished)
            {
                _gameRoundViewHandler.UpdateView();
                
                ChangeState(_cardDealingState);
            }
            else
            {
                var nextState = nextOpponentId > _opponentCount ? _playerMoveState : _opponentMoveStates[nextOpponentId - 1];
            
                ChangeState(nextState);
            }
        }
        else
        {
            Debug.LogError("Current state can not cast to Opponent State");
        }
    }
    
    private void OnDealingStateComplete() => ChangeState(IsLastOpponentCardsFinished ? _resultCalculationState : _playerMoveState);

    private void OnResultCalculated()
    {
        var isPlayerWin = ((ResultCalculationState)_resultCalculationState).IsPlayerWin;
        _gameplayDataService.ChangeGameState(isPlayerWin ? GameState.GameSuccessState : GameState.GameFailState, 0f);
    }

    public void Tick() => _currentGameplayState?.OnUpdate(Time.deltaTime);

    public void ChangeState(IState state)
    {
        if (_currentGameplayState != state)
        {
            _currentGameplayState?.OnExit();
            _currentGameplayState = state;
            _currentGameplayState.OnEnter();
        }
        else
        {
            Debug.LogError("You can not set same state!");
        }
    }
    
    public void Dispose()
    {
        _playerBoard.Reset();
        _opponentBoards.ForEach(x => x.Reset());
        
        _playerMoveState.Reset();
        _opponentMoveStates.ForEach(x => x.Reset());
        _cardDealingState.Reset();
        _resultCalculationState.Reset();
        _gameRoundViewHandler.Reset();
        
        _playerMoveState.OnStateComplete -= OnPlayerMoveStateComplete;
        
        for (int i = 0; i < _opponentCount; i++) 
            _opponentMoveStates[i].OnStateComplete -= OnOpponentMoveStateComplete;
        
        _cardDealingState.OnStateComplete -= OnDealingStateComplete;
        
        _gameStateBinding.Remove(OnGameplayStateChanged);
        EventBus<GameStateData>.RemoveListener(_gameStateBinding, GameStateData.GetChannel(TransitionState.Middle));
    }
}