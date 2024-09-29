using System.Threading;
using Core.AssetProvider;
using Core.Database;
using Core.Runtime.Storage;
using Core.SceneManager;
using Core.Storage;
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
        private IStorageReceiver _storageReceiver;

        public LobbyState(
            ISceneManager sceneManager,
            IAssetProvider assetProvider,
            SituationsQueueFactory situationsQueueFactory,
            IStorageReceiver storageReceiver)
        {
            _storageReceiver = storageReceiver;
            _situationsQueueFactory = situationsQueueFactory;
            _assetProvider = assetProvider;
            _sceneManager = sceneManager;
        }
        public async UniTask EnterAsync(CancellationToken cancellationToken)
        {
            //var situationsQueue = await _situationsQueueFactory.Create(cancellationToken);
            //_storageReceiver.Receive(new AmountableItem("coin",-190));
            await _sceneManager.LoadSceneAsync("LobbyScene");
        }

        public async UniTask ExitAsync(CancellationToken cancellationToken)
        {
        }
    }
}