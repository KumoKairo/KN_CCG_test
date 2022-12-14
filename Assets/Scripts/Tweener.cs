using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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

        var cardWidth = cards[0].rectTransform.rect.width;
        var offset = CalculateOffset(numOfCards, cardWidth, _animSettings.placingMargin);

        for (int i = 0; i < numOfCards; i++)
        {
            var card = cards[i];
            card.gameObject.SetActive(true);

            var transform = card.rectTransform;
            transform.rotation = Quaternion.Euler(0f, -90f, 0);
            transform.localPosition = _positions.handStartPosition.localPosition;

            var insertAtPos = i * _animSettings.appearDelay;
            var moveTo = _positions.handAppearPosition.localPosition;
            moveTo.x = ShiftedPosition(i, cardWidth, offset, _animSettings.placingMargin);

            anim.Insert(insertAtPos,
                card.rectTransform.DOLocalMove(moveTo,
                        _animSettings.appearSpeed, true)
                    .SetEase(_animSettings.defaultEase));

            anim.Insert(insertAtPos,
                card.rectTransform.DORotate(Vector3.zero, _animSettings.appearSpeed)
                    .SetEase(_animSettings.defaultEase));
            
            insertAtPos = _animSettings.appearDelay * numOfCards + _animSettings.appearIdle +
                          _animSettings.handMoveDelay * i;
            
            PlaceCardOnArc(anim, insertAtPos, card, moveTo, numOfCards, i);
        }

        anim.OnComplete(OnPlacingInHandCompleted);
    }

    public static float ShiftedPosition(int index, float cardWidth, float offset, float margin)
    {
        return index * cardWidth + index * margin - offset;
    }

    public static float CalculateOffset(int numOfCards, float cardWidth, float margin)
    {
        var floorHalfNumOfCards = numOfCards / 2;
        var isOdd = 1 - numOfCards % 2;
        var offset = floorHalfNumOfCards * cardWidth - cardWidth * isOdd * 0.5f +
                     margin * floorHalfNumOfCards;
        return offset;
    }

    private void PlaceCardOnArc(Sequence anim, float insertAtPos, Card card, Vector3 moveTo, int numOfCards, int index)
    {
        var handleIdlePosition = _positions.handIdlePosition.localPosition;

        const float middle = 0.5f;
        var normalizedIndexPosition = numOfCards == 1 ? middle : (float)index / (numOfCards - 1); // NaN safeguard
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

    private void OnPlacingInHandCompleted()
    {
        var button = _manager.randomChangeButton;
        button.alpha = 0f;
        button.gameObject.SetActive(true);
        button.DOFade(1f, _animSettings.appearSpeed);
        button.blocksRaycasts = true;

        _manager.MakeCardsInteractable(true);
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

        if (remainingCards.Count <= 0)
        {
            return;
        }

        RepositionCards(remainingCards);
    }

    public void RepositionCards(List<Card> cards)
    {
        var numOfCards = cards.Count;
        var cardWidth = cards[0].rectTransform.rect.width;
        var offset = CalculateOffset(numOfCards, cardWidth, _animSettings.placingMargin);

        var anim = DOTween.Sequence();
        for (int i = 0; i < numOfCards; i++)
        {
            var card = cards[i];
            var moveTo = _manager.positions.handIdlePosition.localPosition;
            moveTo.x = ShiftedPosition(i, cardWidth, offset, _animSettings.placingMargin);
            PlaceCardOnArc(anim, 0f, card, moveTo, numOfCards, i);
        }

        anim.OnComplete(() =>
        {
            _manager.MakeCardsInteractable(true);
        });
    }

    public void FadeImage(Image image, float targetAlpha)
    {
        image.DOFade(targetAlpha, _animSettings.appearSpeed);
    }
}