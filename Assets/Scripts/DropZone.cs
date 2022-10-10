using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Manager manager;
    public RectTransform rectTransform;

    private List<Card> _placedCards;

    private void Start()
    {
        _placedCards = new List<Card>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var animSettings = manager.animSettings;
        
        var card = eventData.pointerDrag.GetComponent<Card>();
        card.BecomePlaced();
        _placedCards.Add(card);
        manager.RemoveCardFromHand(card);

        var cardWidth = card.rectTransform.rect.width;
        var numCards = _placedCards.Count;
        var offset = Tweener.CalculateOffset(numCards, cardWidth, animSettings.placingMargin);
        for (int i = 0; i < numCards; i++)
        {
            var positionAt = rectTransform.localPosition;
            positionAt.x = Tweener.ShiftedPosition(i, cardWidth, offset, animSettings.placingMargin);
            _placedCards[i].rectTransform.DOLocalMove(positionAt, animSettings.appearSpeed)
                .SetEase(animSettings.defaultEase);
        }
    }
}
