using System;
using Core.Input;
using UnityEngine;

namespace UI.SituationDisplay
{
    public class UISituationViewAnimation 
    {
        private Animator _animator;
        public UISituationViewAnimation(Animator animator)
        {
            _animator = animator;
        }
    
        public void DoSelectionDrag(float progress, Direction direction)
        {
            ChangeAnimationDirection(progress, direction);
        }

        public void DoFlip(float progress, Action callback = null)
        {
            PlayIdleAnimation(progress);
            _animator.Play("UISituationView_Flip",1, progress);
            if(Mathf.Approximately(progress, 1))
                callback?.Invoke();
        }
        public void DoCancel(float progress)
        {
            PlayIdleAnimation(progress);
            _animator.Play("UISituationView_Idle",1, progress);
        }
        
        private void ChangeAnimationDirection(float progress, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    _animator.Play("UISituationView_SelectLeft",0,progress);
                    break;
                case Direction.Right:
                    _animator.Play("UISituationView_SelectRight",0,progress);
                    break;
            }
        }

        private void PlayIdleAnimation(float progress)
        {
            _animator.Play("UISituationView_Idle",0, progress);
        }
        
    }
}
