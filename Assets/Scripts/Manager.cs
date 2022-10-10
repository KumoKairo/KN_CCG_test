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

    public CanvasGroup randomChangeButton;
    public Text loadingText;
    public RectTransform rootCanvasTransform;
    public Card cardPrefab;

    [Space]
    public PositionStorage positions;
    public AnimationSettings animSettings;

    private List<Card> _cards;
    private List<Action<Card, int>> _valueChangingDelegates;
    private Tweener _tweener;

    private List<Card> _cardsToDiscard;

    private IEnumerator Start()
    {
        _cardsToDiscard = new List<Card>(1);
        
        randomChangeButton.gameObject.SetActive(false);
        randomChangeButton.blocksRaycasts = false;
        loadingText.gameObject.SetActive(true);

        _tweener = new Tweener(animSettings, positions, this);
        
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
                card.Init(this);
                card.gameObject.SetActive(false);
                // var texture = DownloadHandlerTexture.GetContent(request);
                // texture.filterMode = FilterMode.Point;
                // card.backdrop.texture = texture;
                card.CardName = RandomTextGenerator.GetRandomName();
                card.Description = RandomTextGenerator.GetRandomDescription();
                
                _cards.Add(card);
            }
            else
            {
                loadingText.text = DownloadingError;
                yield break;
            }
        }
        
        loadingText.gameObject.SetActive(false);

        _tweener.TweenCardsIn(_cards);
    }

    public void OnButtonClicked()
    {
        randomChangeButton.blocksRaycasts = false;
        StartCoroutine(ChangeCardValuesCoroutine());
    }

    private IEnumerator ChangeCardValuesCoroutine()
    {
        var cardsCount = _cards.Count;
        var counterDelay = new WaitForSeconds(animSettings.valueChangeCardsDelay);
        for (int i = 0; i < cardsCount; i++)
        {
            var card = _cards[i];
            var randomChanger = _valueChangingDelegates[Random.Range(0, _valueChangingDelegates.Count)];
            var randomValue = Random.Range(-2, 9);
            randomChanger(card, randomValue);
            yield return counterDelay;
        }

        for (int i = 0; i < _cardsToDiscard.Count; i++)
        {
            var card = _cardsToDiscard[i];
            _cards.Remove(card);
            _tweener.DiscardCard(card, _cards);
        }

        randomChangeButton.blocksRaycasts = true;
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
