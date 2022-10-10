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

    public string CardName
    {
        set => cardName.text = value;
    }

    public int Mana
    {
        set
        {
            _mana = value;
            manaText.text = _mana.ToString();
        }
        get => _mana;
    }
    
    public int Attack
    {
        set
        {
            _attack = value;
            attackText.text = _attack.ToString();
        }
        get => _attack;
    }
    
    public int Health
    {
        set
        {
            _health = value;
            healthText.text = _health.ToString();
        }
        get => _health;
    }

    public string Description
    {
        set => description.text = value;
    }
}
