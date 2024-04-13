using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityBase.StateMachineCore;

public class ResultCalculationState : IState
{
    public event Action OnStateComplete;
    private readonly List<IUserBoard> _allUserBoards;
    private bool _isPlayerWin;

    public bool IsPlayerWin => _isPlayerWin;

    public ResultCalculationState(IUserBoard playerBoard, IUserBoard[] opponentBoards)
    {
        _allUserBoards = new List<IUserBoard> { playerBoard };
        
        foreach (var opponentBoard in opponentBoards)
        {
            _allUserBoards.Add(opponentBoard);
        }
    }
    
    public async void OnEnter()
    {
        AddExtraPointToUser();
        
        var maxPoint = _allUserBoards.Max(x => x.CollectedCards.CollectedCardPoint);
        
        var userWithMaxPoint = _allUserBoards.FirstOrDefault(x => x.CollectedCards.CollectedCardPoint == maxPoint);
        
        _isPlayerWin = userWithMaxPoint?.UserID == 0;
        
        await UniTask.WaitForSeconds(0.5f);
        
        OnStateComplete?.Invoke();
    }

    public void OnUpdate(float deltaTime) { }
    public void OnExit() { }
    public void Reset() { }
    
    private void AddExtraPointToUser()
    {
        var maxCardCount = _allUserBoards.Max(x => x.CollectedCards.CollectedCardCount);

        var selectedUsers = _allUserBoards.Where(x => x.CollectedCards.CollectedCardCount == maxCardCount).ToArray();

        if (selectedUsers.Length == 1)
        {
            selectedUsers[0].CollectedCards.AddExtraPoint(3);
        }
    }
}