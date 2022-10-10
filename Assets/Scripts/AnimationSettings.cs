using DG.Tweening;
using UnityEngine;

[CreateAssetMenu]
public class AnimationSettings : ScriptableObject
{
    public float placingMargin = 8f;
    public Ease defaultEase = Ease.OutCubic;
    public float appearSpeed = 2f;
    public float appearDelay = 1f;
    public float appearIdle = 1f;
    public float handMoveDelay = 0.1f;
}
