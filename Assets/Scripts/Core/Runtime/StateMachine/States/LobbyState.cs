using System.Threading;
using Core.AssetProvider;
using Core.SceneManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.StateMachine
{
    public class LobbyState : IState
    {
        private ISceneManager _sceneManager;
        private IAssetProvider _assetProvider;

        public LobbyState(ISceneManager sceneManager, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _sceneManager = sceneManager;
        }
        public async UniTask EnterAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Start entering LobbyState");
            await _assetProvider.LoadAssets<Sprite>("UI", default);
            await _sceneManager.LoadSceneAsync(ScenesProvider.LOBBY_SCENE);
            Debug.Log("End entering LobbyState");
        }

        public async UniTask ExitAsync(CancellationToken cancellationToken)
        {
        }
    }
}