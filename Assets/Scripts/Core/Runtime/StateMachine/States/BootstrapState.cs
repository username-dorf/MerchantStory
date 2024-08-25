using System.Threading;
using Core.SceneManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.StateMachine
{
    public class BootstrapState : IState
    {
        private IStateMachine _stateMachine;

        public BootstrapState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public async UniTask EnterAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Start entering BootstrapState");
            await _stateMachine.ChangeStateAsync<LobbyState>(false);
            Debug.Log("End entering BootstrapState");
        }

        public async UniTask ExitAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Start exiting BootstrapState");
            await UniTask.Delay(1000);
            Debug.Log("End exiting BootstrapState");
        }
    }
}