using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    public Text manaText;
    public Text attackText;
    public Text healthText;
    public Text cardName;
    public Text description;
    public RawImage backdrop;

    private int _mana;
    private int _attack;
    private int _health;
    private Manager _manager;

    public string CardName
    {
        set => cardName.text = value;
    }

    public int Mana
    {
        set
        {
            var oldValue = _mana;
            _mana = value;
            CounterChangeValue(manaText, oldValue, value);
        }
        get => _mana;
    }
    
    public int Attack
    {
        set
        {
            var oldValue = _attack;
            _attack = value;
            CounterChangeValue(attackText, oldValue, value);
        }
        get => _attack;
    }
    
    public int Health
    {
        set
        {
            var oldValue = _health;
            _health = value;
            CounterChangeValue(healthText, oldValue, value);
        }
        get => _health;
    }

    public string Description
    {
        set => description.text = value;
    }

    public void Init(Manager manager)
    {
        const int minAttributeValue = 3;
        const int maxAttributeValue = 10;
        
        _manager = manager;

        _mana = Random.Range(minAttributeValue, maxAttributeValue);
        _attack = Random.Range(minAttributeValue, maxAttributeValue);
        _health = Random.Range(minAttributeValue, maxAttributeValue);
        
        manaText.text = _mana.ToString();
        attackText.text = _attack.ToString();
        healthText.text = _health.ToString();
    }

    private void CounterChangeValue(Text textToChange, int oldValue, int newValue)
    {
        if (oldValue == newValue)
        {
            return;
        }
        
        textToChange.DOCounter(oldValue, newValue, _manager.animSettings.valueChangeCardsDelay);
        textToChange.rectTransform.DOShakePosition(_manager.animSettings.punchDuration, _manager.animSettings.punchStrength);
    }
}
