using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardData;

namespace CardPackData
{
    [CreateAssetMenu(fileName = "NewPackCard", menuName = "PackCard")]
    public class CardPack : Card
    {
        public List<Card> containedCard;

        public int toReveal = 5;
        public bool isOpened = false;
        public bool isStarterPack = false;
    }

}

