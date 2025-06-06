using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardData
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {

        public string cardName;
        public CardType cardType;

        [TextArea(10, 10)]
        public string cardDescription;

        public Sprite cardSprite;
        public int health;
        public int value = 0;
        //public int originalValue = 0;
        public int sellingPrice = 0;
        public bool isSellable = false;



    }


    // Sert a donner des noms a des constantes
    public enum CardType // Les indices d'enumerations commencent par zero
    {
        Human, // indice 0
        Food, // indice 1
        Coin, // indice 2
        RawMaterial, // indice 3
        PrimaryMaterial, // indice 4
        Ennemy, // indice 5
        CardPack, // indice 6
        Idea, // indice 7
        ToolCrafted, // indice 8
        QuestCard, // indice 9
        QuestSlot // indice 10
    }


}
