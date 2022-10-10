using System.Collections.Generic;
using UnityEngine;

public class Tweener
{
    public void TweenCardsIn(List<Card> cards, RectTransform startingPosition)
    {
        var numOfCards = cards.Count;
        for (int i = 0; i < numOfCards; i++)
        {
            var card = cards[i];
            var transform = card.rectTransform;
            transform.rotation = Quaternion.Euler(0f, -90f, 0);
            transform.localPosition = startingPosition.localPosition;
        }
        
        // var floorHalfNumOfCards = numOfCards / 2;
        // var isOdd = 1 - numOfCards % 2;
        // var cardWidth = cards[0].rectTransform.rect.width;
        // var offset = floorHalfNumOfCards * cardWidth - cardWidth * isOdd * 0.5f;
        // for (int i = 0; i < numOfCards; i++)
        // {
        //     var card = cards[i];
        //     var newX = i * cardWidth - offset;
        //     card.rectTransform.localPosition = new Vector3(newX, 0f, 0f);
        //     card.gameObject.SetActive(true);
        // }
    }
}
