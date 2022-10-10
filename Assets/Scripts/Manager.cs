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
    public Image dropArea;
    public Text loadingText;
    public Canvas rootCanvas;
    public RectTransform rootCanvasTransform;
    public Card cardPrefab;

    [Space]
    public PositionStorage positions;
    public AnimationSettings animSettings;

    private List<Card> _cards;
    private List<Action<Card, int>> _valueChangingDelegates;
    private Tweener _tweener;

    private List<Card> _cardsToDiscard;
    private List<Card> _cardsToReposition;

    private IEnumerator Start()
    {
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
        _cardsToDiscard = new List<Card>(1);
        _cardsToReposition = new List<Card>(numOfCards);

        _cards = new List<Card>(numOfCards);
        
        for (int i = 0; i < numOfCards; i++)
        {
            var request = UnityWebRequestTexture.GetTexture(PictureUrl);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                var card = Instantiate(cardPrefab, rootCanvasTransform);
                card.Init(this);
                card.gameObject.SetActive(false);
                var texture = DownloadHandlerTexture.GetContent(request);
                texture.filterMode = FilterMode.Point;
                card.backdrop.texture = texture;
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
    
    public void RepositionCardsExcept(Card card)
    {
        MakeCardsInteractable(false);
        
        _cardsToReposition.Clear();
        for (int i = 0; i < _cards.Count; i++)
        {
            if (card != _cards[i])
            {
                _cardsToReposition.Add(_cards[i]);
            }
        }

        if (_cardsToReposition.Count > 0)
        {
            _tweener.RepositionCards(_cardsToReposition);
        }
    }

    public void OnStartEndDrag(bool isStart)
    {
        var targetDropAreaAlpha = isStart ? 1f : 0f;
        _tweener.FadeImage(dropArea, targetDropAreaAlpha);
    }

    public void RemoveCardFromHand(Card card)
    {
        _cards.Remove(card);
    }
    
    public void MakeCardsInteractable(bool isInteractable)
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].MakeInteractable(isInteractable);
        }
    }

    private IEnumerator ChangeCardValuesCoroutine()
    {
        MakeCardsInteractable(false);
        
        var cardsCount = _cards.Count;
        var counterDelay = new WaitForSeconds(animSettings.valueChangeCardsDelay);
        for (int i = 0; i < cardsCount; i++)
        {
            var card = _cards[i];
            var randomChanger = _valueChangingDelegates[Random.Range(0, _valueChangingDelegates.Count)];
            var randomValue = Random.Range(-2, 9); // Relative change, not absolute
            randomChanger(card, randomValue);
            
            if (card.Health < 1)
            {
                _cardsToDiscard.Add(card);
            }
            
            yield return counterDelay;
        }

        _cards.RemoveAll(card => _cardsToDiscard.Contains(card));
        _tweener.DiscardCards(_cardsToDiscard, _cards);
        _cardsToDiscard.Clear();

        randomChangeButton.blocksRaycasts = true;
        MakeCardsInteractable(true);
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
