using System.Threading;
using Core.AssetProvider;
using Core.SceneManager;
using Cysharp.Threading.Tasks;
using Gameplay.StoryEngine.Core;
using UnityEngine;

namespace Core.StateMachine
{
    public class LobbyState : IState
    {
        private ISceneManager _sceneManager;
        private IAssetProvider _assetProvider;
        private SituationsQueueFactory _situationsQueueFactory;

        public LobbyState(
            ISceneManager sceneManager,
            IAssetProvider assetProvider,
            SituationsQueueFactory situationsQueueFactory)
        {
            _situationsQueueFactory = situationsQueueFactory;
            _assetProvider = assetProvider;
            _sceneManager = sceneManager;
        }
        public async UniTask EnterAsync(CancellationToken cancellationToken)
        {
            var situationsQueue = await _situationsQueueFactory.Create(cancellationToken);
        }

        public async UniTask ExitAsync(CancellationToken cancellationToken)
        {
        }
    }
}