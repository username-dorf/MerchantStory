using Core.Input;
using UniRx;
using UnityEngine;

public class UISituationViewAnimation 
{
    private Animator _animator;
    public UISituationViewAnimation(Animator animator)
    {
        _animator = animator;
    }
    
    public void DoSelectionDrag(float progress, Direction direction)
    {
        UpdateAnimationProgress(progress);
        ChangeAnimationDirection(progress, direction);
    }

    public void DoFlip(float progress)
    {
        _animator.Play("UISituationView_Idle",0, progress);
        _animator.Play("UISituationView_Flip",1, progress);
    }
    
    private void UpdateAnimationProgress(float progress)
    {
        _animator.speed = 0f;
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
        _animator.Play(state.fullPathHash, 0, progress);
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
}
