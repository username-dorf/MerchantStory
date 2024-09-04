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
            await _stateMachine.ChangeStateAsync<LobbyState>(false);
        }

        public async UniTask ExitAsync(CancellationToken cancellationToken)
        {
        }
    }
}