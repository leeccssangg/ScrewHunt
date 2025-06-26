using UnityEngine;
using DG.Tweening;

public class ScaleDownTweenTransition : TweenTransition
{
    [field: SerializeField] public Transform Owner { get; private set; }
    [field: SerializeField] public Vector3 ScaleDown { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
    [field: SerializeField] public float Delay { get; private set; }
    [field: SerializeField] public Ease Ease { get; private set; }
    
    public override Tween Play()
    {
        MainTween = Owner.DOScale(ScaleDown, Duration).SetEase(Ease).SetDelay(Delay);
        return MainTween;
    }
    public override Tween Kill()
    {
        MainTween?.Kill();
        return MainTween;
    }
    public override float GetDuration()
    {
        return Duration;
    }
    
    public override float GetDelay()
    {
        return Delay;
    }
}
