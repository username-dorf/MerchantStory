using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.AssetProvider
{
    public interface IAssetProvider
    {
        /// <summary>
        /// Load a single asset by path
        /// </summary>
        /// <param name="addressablePath"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        UniTask<T> LoadAsset<T>(string addressablePath, CancellationToken cancellationToken) where T : class;

        /// <summary>
        /// Load multiple assets by tag
        /// </summary>
        /// <param name="addressablesTag"></param>
        /// <param name="cancellationToken"></param>
        UniTask LoadAssets<T>(string addressablesTag,CancellationToken cancellationToken);

        /// <summary>
        /// Get a loaded asset by path
        /// </summary>
        /// <param name="addressablePath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetAsset<T>(string addressablePath);

        /// <summary>
        /// Get all loaded assets by tag
        /// </summary>
        /// <param name="addressablesTag"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAssets<T>(string addressablesTag);

        /// <summary>
        /// Release a single asset by path
        /// </summary>
        /// <param name="addressablePath"></param>
        void Release(string addressablePath);

        /// <summary>
        /// Release all assets associated with a tag
        /// </summary>
        /// <param name="addressablesTag"></param>
        void ReleaseAll(string addressablesTag);

        /// <summary>
        /// Load a single asset by path with progress
        /// </summary>
        /// <param name="addressablePath"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        UniTask<T> LoadAssetWithProgress<T>(string addressablePath, IProgress<float> progress, CancellationToken cancellationToken);

        /// <summary>
        /// Load multiple assets by tag with progress
        /// </summary>
        /// <param name="addressablesTag"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        UniTask LoadAssetsWithProgress(string addressablesTag, IProgress<float> progress, CancellationToken cancellationToken);
    }
}