using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Core.SceneManager
{
    public interface ISceneManager
    {
        UniTask LoadSceneAsync(string sceneName);
    }
    public class SceneManager : ISceneManager
    {
        public async UniTask LoadSceneAsync(string sceneName)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneName);
            while (!handle.IsDone)
            {
                Debug.Log("Loading progress: " + (handle.GetDownloadStatus().Percent * 100) + "%");
                await UniTask.Yield();
            }
            Debug.Log("Scene loaded successfully!");
        }
    }
}