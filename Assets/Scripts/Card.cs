using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
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
            StartCoroutine(CounterChangeValue(manaText, _mana, value));
            _mana = value;
        }
        get => _mana;
    }
    
    public int Attack
    {
        set
        {
            StartCoroutine(CounterChangeValue(attackText, _attack, value));
            _attack = value;
        }
        get => _attack;
    }
    
    public int Health
    {
        set
        {
            StartCoroutine(CounterChangeValue(healthText, _health, value));
            _health = value;
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

    private IEnumerator CounterChangeValue(Text textToChange, int oldValue, int newValue)
    {
        var difference = newValue - oldValue;
        var counterChangingSpeed = new WaitForSeconds(_manager.animSettings.valueChangeCardsDelay / difference);
        var sign = Mathf.Sign(newValue);
        var abs = Mathf.Abs(difference);

        for (int i = 0; i < abs; i++)
        {
            textToChange.text = $"{oldValue + i * sign}";
            yield return counterChangingSpeed;
        }
    }
}
