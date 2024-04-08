using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityBase.Extensions;
using UnityEngine;

namespace UnityBase.Command
{
    public class ObjectMoveCommand : MoveCommand
    {
        private Vector3 _newPosition;
        
        private Vector3 _oldPosition;
        
        private bool _isInProgress;
        public override bool IsInProgress => _isInProgress;
        public override bool CanPassNextCommandInstantly => _moveEntity.CanPassNextMovementInstantly;

        private CancellationTokenSource _cancellationTokenSource;
        public ObjectMoveCommand(IMoveEntity moveEntity) : base(moveEntity) { }

        public override void Record() => _newPosition = _moveEntity.TargetPosition;

        public override async void Execute(Action onComplete)
        {
            _oldPosition = _moveEntity.ObjectTransform.position;
            
            _newPosition = _moveEntity.TargetPosition;

            await MoveObjectAsync(_newPosition, onComplete);
        }
        
        public override async void Undo(bool directly, Action onComplete)
        {
            if (directly)
            {
                _moveEntity.ObjectTransform.position = _oldPosition;
                onComplete?.Invoke();
                return;    
            }
            
            await MoveObjectAsync(_oldPosition, onComplete);
        }

        public override async void Redo(bool directly, Action onComplete)
        {
            if (directly)
            {
                _moveEntity.ObjectTransform.transform.position = _newPosition;
                onComplete?.Invoke();
                return;
            }
            
            await MoveObjectAsync(_newPosition, onComplete);
        }

        public override void Cancel() => _cancellationTokenSource?.Cancel();
        public override void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTask MoveObjectAsync(Vector3 targetPosition, Action onComplete)
        {
            _isInProgress = true;
            
            CancellationTokenExtentions.Refresh(ref _cancellationTokenSource);

            try
            {
                var transform = _moveEntity.HandlerTransform;

                while (transform.position.Distance(targetPosition) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveEntity.Speed * Time.deltaTime);
                    
                    await UniTask.WaitForSeconds(0f,false, PlayerLoopTiming.Update, _cancellationTokenSource.Token);
                }

                transform.position = targetPosition;
                
                onComplete?.Invoke();

                _isInProgress = false;
            }
            catch (Exception e)
            {
                //Debug.Log(e);
                
            }
        }
    }
}