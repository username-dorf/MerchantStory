using System;
using System.Threading;
using Core.AssetProvider;
using Core.SceneManager;
using Cysharp.Threading.Tasks;
using UI.Runtime.Situation;
using UniRx;

namespace Core.StateMachine
{
    public class AdventureState : IState
    {
        private CompositeDisposable _compositeDisposable;

        private ISceneManager _sceneManager;
        private IAssetProvider _assetProvider;
     


        public AdventureState(
            ISceneManager sceneManager,
            IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _sceneManager = sceneManager;
            _compositeDisposable = new CompositeDisposable();
        }
        public async UniTask EnterAsync(CancellationToken cancellationToken)
        {
            
            await LoadAssets(cancellationToken);
            await _sceneManager.LoadSceneAsync("AdventureModeScene");
        }

        public async UniTask ExitAsync(CancellationToken cancellationToken)
        {
            _compositeDisposable.Clear();
        }

        private async UniTask LoadAssets(CancellationToken cancellationToken)
        {
            try
            {
                var uiSituationViewFactoryAssetRequest = new UISituationViewFactoryAssetRequest();
                _compositeDisposable.Add(uiSituationViewFactoryAssetRequest);
                await _assetProvider.ResolveRequest(uiSituationViewFactoryAssetRequest, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                
            }
        }
    }
}