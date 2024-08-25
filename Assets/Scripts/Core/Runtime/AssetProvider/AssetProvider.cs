using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<(string tag, string path), object> _loadedAssets = new Dictionary<(string tag, string path), object>();

        /// <summary>
        /// Load a single asset by path
        /// </summary>
        /// <param name="addressablePath"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> LoadAsset<T>(string addressablePath, CancellationToken cancellationToken) where T : class
        {
            (string tag, string path) key = (null, addressablePath);
            if (_loadedAssets.TryGetValue(key, out var asset))
                return (T)asset;

            var handle = Addressables.LoadAssetAsync<T>(addressablePath);

            try
            {
                await handle.ToUniTask(cancellationToken: cancellationToken);
                _loadedAssets[key] = handle.Result;
                return _loadedAssets[key] as T;
            }
            catch (OperationCanceledException)
            {
                Addressables.Release(handle);
                throw;
            }
        }

        /// <summary>
        /// Load multiple assets by tag
        /// </summary>
        /// <param name="addressablesTag"></param>
        /// <param name="cancellationToken"></param>
        public async UniTask LoadAssets<T>(string addressablesTag,CancellationToken cancellationToken)
        {
            var handle = Addressables.LoadAssetsAsync<T>(addressablesTag, null);
            try
            {
                await handle.ToUniTask(cancellationToken: cancellationToken);
                var assets = new List<T>();
                assets.AddRange(handle.Result);

                foreach (var asset in assets)
                {
                    var key = (addressablesTag,
                        asset.ToString());
                    _loadedAssets[key] = asset;
                }
            }
            catch (OperationCanceledException)
            {
                Addressables.Release(handle);
                throw;
            }
        }

        /// <summary>
        /// Get a loaded asset by path
        /// </summary>
        /// <param name="addressablePath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAsset<T>(string addressablePath)
        {
            (string tag, string path) key = (null, addressablePath);
            if (_loadedAssets.TryGetValue(key, out var asset))
            {
                return (T)asset;
            }

            Debug.LogWarning($"Asset at path {addressablePath} not found.");
            return default;
        }

        /// <summary>
        /// Get all loaded assets by tag
        /// </summary>
        /// <param name="addressablesTag"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAssets<T>(string addressablesTag)
        {
            foreach (var kvp in _loadedAssets)
            {
                if (kvp.Key.tag == addressablesTag)
                {
                    yield return (T)kvp.Value;
                }
            }
        }

        /// <summary>
        /// Release a single asset by path
        /// </summary>
        /// <param name="addressablePath"></param>
        public void Release(string addressablePath)
        {
            (string tag, string path) key = (null, addressablePath);
            if (_loadedAssets.TryGetValue(key, out var asset))
            {
                Addressables.Release(asset);
                _loadedAssets.Remove(key);
            }
            else
            {
                Debug.LogWarning($"Asset at path {addressablePath} not found.");
            }
        }

        /// <summary>
        /// Release all assets associated with a tag
        /// </summary>
        /// <param name="addressablesTag"></param>
        public void ReleaseAll(string addressablesTag)
        {
            var keysToRemove = new List<(string tag, string path)>();

            foreach (var kvp in _loadedAssets)
            {
                if (kvp.Key.tag == addressablesTag)
                {
                    Addressables.Release(kvp.Value);
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _loadedAssets.Remove(key);
            }
        }

        /// <summary>
        /// Load a single asset by path with progress
        /// </summary>
        /// <param name="addressablePath"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> LoadAssetWithProgress<T>(string addressablePath, IProgress<float> progress, CancellationToken cancellationToken)
        {
            (string tag, string path) key = (null, addressablePath);
            if (_loadedAssets.TryGetValue(key, out var loadedAsset))
                return (T)loadedAsset;

            var handle = Addressables.LoadAssetAsync<T>(addressablePath);

            handle.Completed += op => progress.Report(1.0f);

            try
            {
                while (!handle.IsDone)
                {
                    progress.Report(handle.GetDownloadStatus().Percent);
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }

                cancellationToken.ThrowIfCancellationRequested();

                T asset = handle.Result;
                _loadedAssets[key] = asset;

                return asset;
            }
            catch (OperationCanceledException)
            {
                Addressables.Release(handle);
                throw;
            }
        }
        
        /// <summary>
        /// Load multiple assets by tag with progress
        /// </summary>
        /// <param name="addressablesTag"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        public async UniTask LoadAssetsWithProgress(string addressablesTag, IProgress<float> progress, CancellationToken cancellationToken)
        {
            var handle = Addressables.LoadAssetsAsync<object>(addressablesTag, null);

            handle.Completed += op => progress.Report(1.0f);

            var assets = new List<object>();

            try
            {
                while (!handle.IsDone)
                {
                    progress.Report(handle.GetDownloadStatus().Percent);
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }

                cancellationToken.ThrowIfCancellationRequested();

                assets.AddRange(handle.Result);

                foreach (var asset in assets)
                {
                    var key = (addressablesTag, asset.ToString());
                    _loadedAssets[key] = asset;
                }
            }
            catch (OperationCanceledException)
            {
                Addressables.Release(handle);
                throw;
            }
        }

    }
}
