using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tweener
{
    private AnimationSettings _animSettings;
    private PositionStorage _positions;
    private Manager _manager;

    public Tweener(AnimationSettings animSettings, PositionStorage positions, Manager manager)
    {
        _animSettings = animSettings;
        _positions = positions;
        _manager = manager;
    }

    public void TweenCardsIn(List<Card> cards)
    {
        var numOfCards = cards.Count;

        var anim = DOTween.Sequence();

        var floorHalfNumOfCards = numOfCards / 2;
        var isOdd = 1 - numOfCards % 2;
        var cardWidth = cards[0].rectTransform.rect.width;
        var offset = floorHalfNumOfCards * cardWidth - cardWidth * isOdd * 0.5f +
                     _animSettings.placingMargin * floorHalfNumOfCards;

        for (int i = 0; i < numOfCards; i++)
        {
            var card = cards[i];
            var transform = card.rectTransform;
            transform.rotation = Quaternion.Euler(0f, -90f, 0);
            transform.localPosition = _positions.handStartPosition.localPosition;

            var insertAtPos = i * _animSettings.appearDelay;
            var moveTo = _positions.handAppearPosition.localPosition;
            moveTo.x = i * cardWidth + i * _animSettings.placingMargin - offset;

            anim.Insert(insertAtPos,
                card.rectTransform.DOLocalMove(moveTo,
                        _animSettings.appearSpeed, true)
                    .SetEase(_animSettings.defaultEase));

            anim.Insert(insertAtPos,
                card.rectTransform.DORotate(Vector3.zero, _animSettings.appearSpeed)
                    .SetEase(_animSettings.defaultEase));

            card.gameObject.SetActive(true);

            insertAtPos = _animSettings.appearDelay * numOfCards + _animSettings.appearIdle +
                          _animSettings.handMoveDelay * i;

            var handleIdlePosition = _positions.handIdlePosition.localPosition;

            var normalizedIndexPosition = (float)i / (numOfCards - 1);
            var normalizedSineValue = Mathf.Sin(Mathf.PI * normalizedIndexPosition);
            handleIdlePosition.y += normalizedSineValue * _animSettings.arcHeight;
            
            var handPosition = new Vector3(moveTo.x, handleIdlePosition.y, handleIdlePosition.z);
            anim.Insert(insertAtPos, card.rectTransform.DOLocalMove(handPosition, _animSettings.appearSpeed)
                .SetEase(_animSettings.defaultEase));

            var targetRotation = Vector3.zero;
            var lift = _animSettings.sineArcLift;
            var liftedNormalizedPosition = Mathf.Lerp(-1f + lift, 1f - lift, normalizedIndexPosition);
            targetRotation.z = liftedNormalizedPosition * _animSettings.arcRotation;
            
            anim.Insert(insertAtPos, card.rectTransform.DORotate(targetRotation, _animSettings.appearSpeed)
                .SetEase(_animSettings.defaultEase));
        }

        anim.OnComplete(OnPlacingInHandCompleted);
    }
    
    private void OnPlacingInHandCompleted()
    {
        var button = _manager.randomChangeButton;
        button.alpha = 0f;
        button.gameObject.SetActive(true);
        button.DOFade(1f, _animSettings.appearSpeed);
        button.blocksRaycasts = true;
    }

    public void DiscardCards(List<Card> cardsToDiscard, List<Card> remainingCards)
    {
        for (int i = 0; i < cardsToDiscard.Count; i++)
        {
            var delay = _manager.animSettings.handMoveDelay * i;
            var card = cardsToDiscard[i];
            card.rectTransform.DOLocalMove(_manager.positions.cardDiscardPosition.localPosition,
                _manager.animSettings.appearSpeed)
                .SetDelay(delay);
            card.canvasGroup.DOFade(0f, _manager.animSettings.appearSpeed)
                .SetDelay(delay)
                .OnComplete(() => Object.Destroy(card.gameObject));
        }
    }
}