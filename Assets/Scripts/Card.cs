using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card/Card Data")]
public abstract class Card : ScriptableObject
{
    [SerializeField] private string cardName;
    public string CardName => cardName;
    [SerializeField] private string cardType;
    public string CardType => cardType;
    [SerializeField] private Sprite cardIcon;
    public Sprite CardIcon => cardIcon;
    [SerializeField] private int moveCost = 0;
    public int MoveCost => moveCost;
    [SerializeField] private int shopPrice = 0;
    public int ShopPrice => shopPrice;
}
