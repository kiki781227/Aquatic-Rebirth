using CardData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTager : MonoBehaviour
{
    private Card cardData;

    public void SetCardData(Card data)
    {
        cardData = data;
        ApplyTag();
    }

    private void ApplyTag()
    {
        if (cardData != null)
        {
            string tagToAssign = GetTagFromCardType(cardData.cardType);
            gameObject.tag = tagToAssign;
            ///Debug.Log($"Carte instanciée avec tag : {tagToAssign}");
        }
        else
        {
            Debug.LogWarning("Aucune donnée de carte fournie !");
        }
    }

    private string GetTagFromCardType(CardType type)
    {
        switch (type)
        {
            case CardType.Human:
                return "Human";
            case CardType.Food:
                return "Food";
            case CardType.Coin:
                return "Coin";
            case CardType.RawMaterial:
                return "RawMaterial";
            case CardType.PrimaryMaterial:
                return "PrimaryMaterial";
            case CardType.Ennemy:
                return "Ennemy";
            case CardType.CardPack:
                return "CardPack";
            case CardType.Idea:
                return "Idea";
            case CardType.ToolCrafted:
                return "CraftedCard";
            default:
                return "Untagged";
        }
    }
}
