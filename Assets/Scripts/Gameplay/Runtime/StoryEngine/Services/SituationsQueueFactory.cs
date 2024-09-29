using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.StoryEngine.Constructor;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.StoryEngine.Core
{
    public class SituationsQueueFactory
    {
        private ISituationsResourcesProvider _resourcesProvider;
        private ISituationsResourceController _resourceController;
        private IFactory<SituationObject, SituationModel> _situationModelFactory;
        private const string ASSET_TAG = "SE-Situation";


        public SituationsQueueFactory(
            SituationModel.Factory situationModelFactory,
            ISituationsResourceController resourceController,
            ISituationsResourcesProvider resourcesProvider)
        {
            _situationModelFactory = situationModelFactory;
            _resourceController = resourceController;
            _resourcesProvider = resourcesProvider;
        }
        
        public async UniTask<Queue<SituationModel>> Create(CancellationToken cancellationToken)
        {
            try
            {
                await _resourceController.LoadResources(ASSET_TAG, cancellationToken);
                var situations = GetSituationsResources(ASSET_TAG).ToList();
                return CreateQueue(situations,5,false);
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
        private Queue<SituationModel> CreateQueue(List<SituationObject> availableSituations, int length, bool unique)
        {
            var deck = new Queue<SituationModel>();
            using (var pooledObject = UnityEngine.Pool.ListPool<SituationObject>.Get(out List<SituationObject> situationsCopy))
            {
                situationsCopy.Clear();
                situationsCopy.AddRange(availableSituations);
                for (int i = 0; i < length; i++)
                {
                    var situation = situationsCopy[Random.Range(0, availableSituations.Count)];
                    var model = _situationModelFactory.Create(situation);
                    deck.Enqueue(model);
                    if(unique)
                        situationsCopy.Remove(situation);
                }
            }           
            return deck;
        }
    }
}