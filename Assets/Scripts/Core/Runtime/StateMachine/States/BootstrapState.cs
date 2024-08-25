using System.Threading;
using Core.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.StateMachine
{
    public class BootstrapState : IState
    {
        private IStateMachine _stateMachine;
        private IAssetProvider _assetProvider;

        public BootstrapState(IStateMachine stateMachine, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _stateMachine = stateMachine;
        }
        public async UniTask EnterAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Start entering BootstrapState");
            await _assetProvider.LoadAsset<Sprite>("Cart",default);
            await _stateMachine.ChangeStateAsync<LobbyState>(false);
            Debug.Log("End entering BootstrapState");
        }

        public async UniTask ExitAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Start exiting BootstrapState");
            await UniTask.Delay(1000);
            _assetProvider.Release("!");
            Debug.Log("End exiting BootstrapState");
        }
    }
}