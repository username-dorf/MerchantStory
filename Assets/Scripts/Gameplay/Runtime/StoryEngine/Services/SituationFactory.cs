using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Utils;
using Cysharp.Threading.Tasks;
using Gameplay.StoryEngine.Constructor;
using Gameplay.StoryEngine.Core;

namespace Gameplay.StoryEngine
{
    public class SituationFactory
    {
        private SituationModel.Factory _situationModelFactory;
        private ISituationsResourceController _resourceController;
        private ISituationsResourcesProvider _resourcesProvider;
        private List<SituationObject> _situations;
        private const string ASSET_TAG = "SE-Situation";


        public SituationFactory(
            SituationModel.Factory situationModelFactory,
            ISituationsResourceController resourceController,
            ISituationsResourcesProvider resourcesProvider)
        {
            _resourcesProvider = resourcesProvider;
            _resourceController = resourceController;
            _situationModelFactory = situationModelFactory;
        }
        
        public async UniTask<SituationModel> Create(CancellationToken cancellationToken)
        {
            try
            {
                if (_situations is null)
                {
                    await _resourceController.LoadResources(ASSET_TAG, cancellationToken);
                    _situations = GetSituationsResources(ASSET_TAG).ToList();
                }
                return CreateWithWeight(_situations);
            }
            catch (OperationCanceledException)
            {
                _resourceController.ReleaseResources();
                return null;
            }
        }
        
        private IEnumerable<SituationObject> GetSituationsResources(string assetsTag)
        {
            return _resourcesProvider.GetSituationsResources(assetsTag).ToList();
        }
        private SituationModel CreateWithWeight(List<SituationObject> availableSituations, IList<SituationObject> ignore = null)
        {
            using (var pooledObject = UnityEngine.Pool.ListPool<SituationObject>.Get(out List<SituationObject> situationsCopy))
            {
                situationsCopy.Clear();
                if (ignore is null)
                    ignore = Array.Empty<SituationObject>();
                situationsCopy.AddRange(availableSituations.Except(ignore));
                var situation = situationsCopy.ChooseRandom();
                return _situationModelFactory.Create(situation);
            }           
        }
    }
}