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
    public float arcHeight = 20f;
    public float arcRotation = 15f;
    public float valueChangeCardsDelay = 1f;
    public float punchStrength = 1f;
    public float punchDuration = 0.1f;
    
    [Range(0f, 1f)]
    public float sineArcLift = 0.2f;
}
