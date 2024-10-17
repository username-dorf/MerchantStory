using System;
using System.Collections.Generic;
using Core.AssetProvider;
using MVVM;
using TMPro;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.Runtime.Situation
{
    public class UISituationView : MonoBehaviour
    {
        [Data("Description")] public TMP_Text description;
        [Data("ChoiceA")] public TMP_Text choiceA;
        [Data("ChoiceB")] public TMP_Text choiceB;
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
            return instance;
        }

        
    }
}
