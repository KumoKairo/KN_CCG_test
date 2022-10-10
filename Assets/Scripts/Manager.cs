using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Manager : MonoBehaviour
{
    private const string PictureUrl = "https://picsum.photos/190";
    private const string DownloadingError = "ERROR DOWNLOADING PICTURES";

    public Button randomChangeButton;
    public Text loadingText;
    public RectTransform rootCanvasTransform;
    public Card cardPrefab;

    [Space]
    public PositionStorage positions;
    public AnimationSettings animSettings;

    private List<Card> _cards;
    private List<Action<Card, int>> _valueChangingDelegates;
    private Tweener _tweener;

    private IEnumerator Start()
    {
        const int minAttributeValue = 3;
        const int maxAttributeValue = 10;
        
        randomChangeButton.gameObject.SetActive(false);
        loadingText.gameObject.SetActive(true);

        _tweener = new Tweener(animSettings, positions);
        
        _valueChangingDelegates = new List<Action<Card, int>>
        {
            ChangeMana,
            ChangeAttack,
            ChangeHealth
        };

        var numOfCards = Random.Range(4, 7);
        _cards = new List<Card>(numOfCards);
        
        for (int i = 0; i < numOfCards; i++)
        {
            // var request = UnityWebRequestTexture.GetTexture(PictureUrl);
            // yield return request.SendWebRequest();
            if (true /*request.result == UnityWebRequest.Result.Success */)
            {
                var card = Instantiate(cardPrefab, rootCanvasTransform);
                card.gameObject.SetActive(false);
                // var texture = DownloadHandlerTexture.GetContent(request);
                // texture.filterMode = FilterMode.Point;
                // card.backdrop.texture = texture;
                card.CardName = RandomTextGenerator.GetRandomName();
                card.Description = RandomTextGenerator.GetRandomDescription();

                card.Mana = Random.Range(minAttributeValue, maxAttributeValue);
                card.Attack = Random.Range(minAttributeValue, maxAttributeValue);
                card.Health = Random.Range(minAttributeValue, maxAttributeValue);
                
                _cards.Add(card);
            }
            else
            {
                loadingText.text = DownloadingError;
                yield break;
            }
        }
        
        loadingText.gameObject.SetActive(false);
        randomChangeButton.gameObject.SetActive(true);

        _tweener.TweenCardsIn(_cards);
    }

    public void OnButtonClicked()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            var card = _cards[i];
            var randomChanger = _valueChangingDelegates[Random.Range(0, _valueChangingDelegates.Count)];
            var randomValue = Random.Range(-2, 9); 
            randomChanger(card, randomValue);
        }
    }

    private void ChangeMana(Card card, int newValue)
    {
        card.Mana += newValue;
    }

    private void ChangeAttack(Card card, int newValue)
    {
        card.Attack += newValue;
    }

    private void ChangeHealth(Card card, int newValue)
    {
        card.Health += newValue;
    }
}
