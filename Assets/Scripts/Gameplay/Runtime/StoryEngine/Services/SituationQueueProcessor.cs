using System;
using System.Collections.Generic;
using System.Threading;
using Core.Input;
using Core.Utils;
using Cysharp.Threading.Tasks;
using Gameplay.StoryEngine.Constructor;
using Gameplay.StoryEngine.Core;
using UI.SituationDisplay;

namespace Gameplay.StoryEngine
{
    public class SituationQueueProcessor
    {
        private SituationsQueueFactory _situationsQueueFactory;
        private IUISituationModel _uiSituationModel;
        
        private Queue<SituationModel> _situationsQueue;
        private IUserInputListener _inputListener;
        private ChoiceModel.Factory _choiceModelFactory;

        public SituationQueueProcessor(
            IUISituationModel uiSituationModel,
            IUserInputListener inputListener,
            ChoiceModel.Factory choiceModelFactory,
            SituationsQueueFactory situationsQueueFactory)
        {
            _choiceModelFactory = choiceModelFactory;
            _inputListener = inputListener;
            _uiSituationModel = uiSituationModel;
            _situationsQueueFactory = situationsQueueFactory;
        }
        
        public async UniTask CreateQueue(CancellationToken cancellationToken)
        {
            _situationsQueue = await _situationsQueueFactory.Create(cancellationToken);
        }

        public async UniTask Run(CancellationToken cancellationToken)
        {
            try
            {
                _inputListener.SetListening(true);
                while (_situationsQueue.Count > 0)
                {
                    var situation = _situationsQueue.Dequeue();
                    
                    _uiSituationModel.Update(situation.Context.Value.Description,
                        GetChoiceDescription(situation.Context.Value.Choices[0]),
                        GetChoiceDescription(situation.Context.Value.Choices[1]));
                    
                    var direction = await _inputListener.OnSwipeRegistered.AwaitNextValueAsync(cancellationToken);
                    var choiceModel = GetChoiceModel(situation, direction);
                    choiceModel.Execute();
                }
            }
            catch (OperationCanceledException)
            {
                _situationsQueue.Clear();
            }
        }
        private ChoiceModel GetChoiceModel(SituationModel situation, Direction direction)
        {
            var choiceObject = direction == Direction.Left
                ? situation.Context.Value.Choices[0]
                : situation.Context.Value.Choices[1];
            return _choiceModelFactory.Create(choiceObject);
        }

        //TODO factory
        private string GetChoiceDescription(ChoiceObject choiceObject)
        {
            if(choiceObject is ChoiceWithItemObject choiceWithItemObject)
            {
                var firstItem = choiceWithItemObject.Output[0];
                return string.Format(choiceWithItemObject.Description, firstItem.ID,firstItem.Amount);
            }
            return string.Empty;
        }
    }
    
}