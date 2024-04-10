using UnityBase.EventBus;
using UnityBase.Manager;
using UnityBase.Manager.Data;
using UnityBase.ManagerSO;
using UnityBase.Service;
using UnityBase.StateMachineCore;
using UnityEngine;
using VContainer.Unity;

public class GameplayStateMachine : IStateMachine, ITickable, IGameplayConstructorService
{
    private readonly GameplayStateMachineSO _gameplayStateMachineSo;
    private readonly IGameplayDataService _gameplayDataService;
    private readonly int _opponentCount;
    
    
    private readonly IState _idleState, _playerMoveState, _cardDistributionState, _cardCollectingState, _resultCalculationState;
    private readonly IState[] _opponentMoveStates;
    
    private readonly IUserDeck _playerDeck;
    private readonly IUserDeck[] _opponentDecks;

    private readonly EventBinding<GameStateData> _gameStateBinding;
    private GameState _currentGameState;
    
    private IState _currentGameplayState;
    public IState CurrentGameplayState => _currentGameplayState;
    private bool IsLastOpponentCardsFinished => ((OpponentMoveState)_opponentMoveStates[^1]).IsCardsFinished;
    

    public GameplayStateMachine(GameplayDataHolderSO gameplayDataHolderSo, IGameplayDataService gameplayDataService, IDiscardDeck discardDeck, ICardContainer cardContainer)
    {
        _gameplayStateMachineSo = gameplayDataHolderSo.gameplayStateMachineSo;
        _gameplayDataService = gameplayDataService;
        
        _opponentCount = _gameplayStateMachineSo.GetOpponentCount();

        var playerSlots = _gameplayStateMachineSo.GetDeckView<PlayerDeckView>().Slots;
        _playerDeck = new UserDeckController(0, playerSlots);
        _playerMoveState = new PlayerMoveState(_playerDeck);

        _opponentDecks = new IUserDeck[_opponentCount];
        _opponentMoveStates = new IState[_opponentCount];

        for (int i = 1; i <= _opponentCount; i++)
        {
            var opponentSlot = _gameplayStateMachineSo.GetOpponentDeckViewBy(i).Slots;
            _opponentDecks[i] = new UserDeckController(i, opponentSlot);
            _opponentMoveStates[i] = new OpponentMoveState(_opponentDecks[i]);
        }

        _idleState = new IdleState();
        _cardDistributionState = new CardDistributionState(cardContainer, _playerDeck, _opponentDecks, discardDeck);
        _cardCollectingState = new CardCollectingState();
        _resultCalculationState = new ResultCalculationState();
        
        _gameStateBinding = new EventBinding<GameStateData>();

        ChangeState(_idleState);
    }

    public void Initialize()
    {
        _playerMoveState.OnStateComplete += OnPlayerMoveStateComplete;
        
        for (int i = 1; i <= _opponentCount; i++) 
            _opponentMoveStates[i].OnStateComplete += OnOpponentMoveStateComplete;

        _cardDistributionState.OnStateComplete += OnDistributionStateComplete;
        _cardCollectingState.OnStateComplete += OnCollectingStateComplete;
        _resultCalculationState.OnStateComplete += OnResultCalculated;
        
        _gameStateBinding.Add(OnGameplayStateChanged);
        EventBus<GameStateData>.AddListener(_gameStateBinding, GameStateData.GetChannel(TransitionState.End));
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

    public void Start() { }

    private void OnPlayerMoveStateComplete()
    {
        ChangeState(IsLastOpponentCardsFinished ? _cardDistributionState : _opponentMoveStates[0]);
    }
    
    private void OnOpponentMoveStateComplete()
    {
        if (_currentGameplayState is not OpponentMoveState currentOpponent)
        {
            Debug.LogError("Current state can not cast to Opponent State");
            return;
        }
        
        var nextOpponentId = currentOpponent.UserID + 1;
        
        if (nextOpponentId >= _opponentCount)
        {
            ChangeState(_playerMoveState);
        }
        else
        {
            ChangeState(_opponentMoveStates[nextOpponentId]);
        }
    }
    
    private void OnDistributionStateComplete()
    {
        var isCardsFinished = ((CardDistributionState)_cardDistributionState).IsAllCardsFinished;

        if (isCardsFinished)
        {
            ChangeState(_resultCalculationState);
        }
        else
        {
            ChangeState(_playerMoveState);
        }
    }
    
    private void OnCollectingStateComplete()
    {
        if (_currentGameplayState is PlayerMoveState)
        {
            OnPlayerMoveStateComplete();
        }
        else 
        {
            OnOpponentMoveStateComplete();
        }
    }
    
    private void OnResultCalculated()
    {
        var isPlayerWin = ((ResultCalculationState)_resultCalculationState).IsPlayerWin;
        
        _gameplayDataService.ChangeGameState(isPlayerWin ? GameState.GameSuccessState : GameState.GameFailState, 1f);
    }

    public void Tick()
    {
        _currentGameplayState?.OnUpdate(Time.deltaTime);
    }

    public void ChangeState(IState state)
    {
        _currentGameplayState?.OnExit();
        _currentGameplayState = state;
        _currentGameplayState.OnEnter();
    }
    
    public void Dispose()
    {
        _playerMoveState.OnStateComplete -= OnPlayerMoveStateComplete;
        
        for (int i = 1; i <= _opponentCount; i++) 
            _opponentMoveStates[i].OnStateComplete -= OnOpponentMoveStateComplete;
        
        _cardDistributionState.OnStateComplete -= OnDistributionStateComplete;
        _cardCollectingState.OnStateComplete -= OnCollectingStateComplete;
        
        _gameStateBinding.Remove(OnGameplayStateChanged);
        EventBus<GameStateData>.RemoveListener(_gameStateBinding, GameStateData.GetChannel(TransitionState.End));
    }
}