using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.AssetProvider;
using Cysharp.Threading.Tasks;
using Gameplay.StoryEngine.Constructor;
using UnityEngine;

namespace Gameplay.StoryEngine.Core
{
    public interface ISituationsResourcesProvider
    {
        IEnumerable<SituationObject> GetSituationsResources(string assetsTag);
    }

    public interface ISituationsResourceController : IDisposable
    {
        UniTask LoadResources(string assetsTag, CancellationToken externalToken = default);
        void ReleaseResources();
    }
    public class SituationsResourcesController : ISituationsResourceController,ISituationsResourcesProvider
    {
        
        private IAssetProvider _assetProvider;
        private CancellationTokenSource _cancellationTokenSource;
        private string _assetsTag;

        public SituationsResourcesController(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

        }

        public async UniTask LoadResources(string assetsTag, CancellationToken externalToken = default)
        {
            _assetsTag = assetsTag;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            try
            {
                _assetsTag = assetsTag;
                await _assetProvider.LoadAssets<ScriptableObject>(assetsTag, 
                    _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
        }

        public void ReleaseResources()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            if(!string.IsNullOrEmpty(_assetsTag))
                _assetProvider.ReleaseAll(_assetsTag);
        }
        
        public IEnumerable<SituationObject> GetSituationsResources(string assetsTag)
        {
            var objects = _assetProvider.GetAssets<ScriptableObject>(assetsTag);
            return objects.OfType<SituationObject>();
        }
    }
}