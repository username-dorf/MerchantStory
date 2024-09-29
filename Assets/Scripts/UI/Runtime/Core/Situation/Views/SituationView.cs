using UnityEngine;

namespace UI.Runtime.Situation
{
    public class SituationView : MonoBehaviour
    {
        [field:SerializeField] public ActionButton[] ActionButtons { get; private set; }


        public void Initialize(SituationViewModel viewModel)
        {
        
        }

        public void OnActionsChange(ActionButtonModel[] choiceActions)
        {
        
        }
        private void Start()
        {
            var model = new ActionButtonModel("Action", () => Debug.Log("Action"));
        }
    }
}
