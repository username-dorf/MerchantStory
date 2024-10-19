using System;
using System.Collections.Generic;
using Core.AssetProvider;
using Core.Input;
using MVVM;
using TMPro;
using UI.Runtime.SituationDisplay;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.Runtime.Situation
{
    public class UISituationView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        public TMP_Text description;
        public TMP_Text choiceA;
        public TMP_Text choiceB;
        
        private UISituationViewAnimation _animation;
        
        public void Initialize(UISituationViewModel viewModel)
        {
            _animation = new UISituationViewAnimation(animator);

            viewModel.ChoiceADesc
                .Subscribe(OnChoiceAChanged)
                .AddTo(this);

            viewModel.ChoiceBDesc
                .Subscribe(OnChoiceBChanged)
                .AddTo(this);

            viewModel.Description
                .Subscribe(OnDescriptionChanged)
                .AddTo(this);

            viewModel.FlipProgress
                .Subscribe(OnFlipValueChanged)
                .AddTo(this);

            viewModel.SwipeProgress
                .Subscribe(value=>OnSelectionValueChanged(value.Progress, value.Direction))
                .AddTo(this);
        }

        private void OnChoiceAChanged(string value)
        {
            choiceA.text = value;
        }
        
        private void OnChoiceBChanged(string value)
        {
            choiceB.text = value;
        }
        
        private void OnDescriptionChanged(string value)
        {
            description.text = value;
        }

        private void OnFlipValueChanged(float progress)
        {
            _animation.DoFlip(progress);
        }
        private void OnSelectionValueChanged(float progress, Direction direction)
        {
            _animation.DoSelectionDrag(progress, direction);
        }
        
    }

    public class UISituationViewFactoryAssetRequest : IRequestAssets, IDisposable
    {
        public static string ASSET_TAG = "UISituationView";
        private Dictionary<Type, string> _dictionary;

        public Dictionary<Type, string> RequestAssets()
        {
            _dictionary = UnityEngine.Pool.DictionaryPool<Type,string>.Get();
            _dictionary.Add(typeof(UISituationView), ASSET_TAG);
            return _dictionary;
        }

        public void Dispose()
        {
            UnityEngine.Pool.DictionaryPool<Type, string>.Release(_dictionary);
        }
    }
    public class UISituationViewFactory 
    {
        private IAssetProvider _assetProvider;
        private DiContainer _diContainer;

        public UISituationViewFactory(DiContainer diContainer,IAssetProvider assetProvider)
        {
            _diContainer = diContainer;
            _assetProvider = assetProvider;
        }
        
        public UISituationView Create(Transform parent)
        {
            var asset = _assetProvider.GetAsset<UISituationView>(UISituationViewFactoryAssetRequest.ASSET_TAG);
            var instance = _diContainer.InstantiatePrefabForComponent<UISituationView>(asset, parent);
            var viewModel = _diContainer.Instantiate<UISituationViewModel>();
            viewModel.Initialize();
            instance.Initialize(viewModel);
            return instance;
        }

        
    }
}
