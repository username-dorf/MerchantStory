using System.Threading;
using Core.SceneManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.StateMachine
{
    public class LobbyState : IState
    {
        private ISceneManager _sceneManager;

        public LobbyState(ISceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }
        public async UniTask EnterAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Start entering LobbyState");
            await _sceneManager.LoadSceneAsync(ScenesProvider.LOBBY_SCENE);
            Debug.Log("End entering LobbyState");
        }

        public async UniTask ExitAsync(CancellationToken cancellationToken)
        {
        }
    }
}