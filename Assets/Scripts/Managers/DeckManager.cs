using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardData;
using CardPackData;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System.Linq;



public class DeckManager : MonoBehaviour
    {
        public static DeckManager Instance { get; private set; }
        public int maxCardsOnTable = 10;
        public Card coinCardData;

     // Listes des cartes actifs sur la table
        public List<CardDisplay> activeCards = new List<CardDisplay>();
        public List<Card> questCardIdea = new List<Card>();

     // Evenement pour mettre a jour le HUD
        public event System.Action OnCardsUpdate;
        private System.Object _lock = new System.Object(); 
        

    void Awake()
    {
        //Debug.Log($"{Time.time} Awake: {gameObject.name}");
        if (Instance == null) Instance = this;
        LoadCards();
    }


    private void LoadCards()
    {
            coinCardData = Resources.Load<Card>("Coin/Coin");
            questCardIdea.AddRange(Resources.LoadAll<Card>("Idee/OceanQuest"));
    }
    
    public void RegisterCard(CardDisplay card)
    {
        lock (_lock) { // Permet de gerer l'acces a la liste activesCards.
            if (!activeCards.Contains(card))
            {
                activeCards.Add(card);
                OnCardsUpdate?.Invoke(); // Déclenche la mise à jour du HUD
            }
        }

    }


    public void UnregisterCard(CardDisplay card)
    {
        if (activeCards.Contains(card))
        {
            activeCards.Remove(card);
            OnCardsUpdate?.Invoke();
        }
    }

    // Compte les cartes d'un type spécifique (ex: Human)
    public int CountCardsOfType(CardData.CardType type)
    {
       
        int count = 0;

        foreach (CardDisplay card in activeCards)
        {
            if (card.cardData.cardType == type)
            {
                count++;
            }
            else
            {
                //Debug.Log("Probeleme");
            }
        }
       
        return count;
    }

    // Récupère toutes les cartes d'un type spécifique dans activeCards et les retourne sous forme de tableau
    public CardDisplay[] GetCardsOfType(CardData.CardType type)
    {
        return activeCards.Where(card => card.cardData.cardType == type).ToArray();
    }
}





//public List<Card> humanCardsData = new List<Card>();
//public List<Card> foodCardsData = new List<Card>();
//public List<Card> rawMatCardsData = new List<Card>();
//public List<Card> primaryMatCardsData = new List<Card>();
//public List<CardPack> cardPackData = new List<CardPack>();
//public List<Card> ennemyCardsData = new List<Card>();
//public List<Card> ideeCardsData = new List<Card>();


//humanCardsData.AddRange(Resources.LoadAll<Card>("Human"));
//foodCardsData.AddRange(Resources.LoadAll<Card>("Food"));
//rawMatCardsData.AddRange(Resources.LoadAll<Card>("Material/Primary"));
//primaryMatCardsData.AddRange(Resources.LoadAll<Card>("Material/Raw"));
//ennemyCardsData.AddRange(Resources.LoadAll<Card>("Ennemy"));
//ideeCardsData.AddRange(Resources.LoadAll<Card>("Idee"));