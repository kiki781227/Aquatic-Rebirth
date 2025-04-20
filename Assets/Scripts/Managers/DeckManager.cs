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

    // Load les data des cartes quest a remplir 
        public List<Card> questCardIdea = new List<Card>();

     // Evenement pour mettre a jour le HUD
        public event System.Action OnCardsUpdate;

    void Awake()
    {
        //Debug.Log($"{Time.time} Awake: {gameObject.name}");
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        LoadCards();
    }


    private void LoadCards()
    {
            coinCardData = Resources.Load<Card>("Coin/Coin");
            questCardIdea.AddRange(Resources.LoadAll<Card>("Idee/OceanQuest"));
    }
    
    public void RegisterCard(CardDisplay card)
    {
        if (card == null)
        {
            Debug.LogError("CardDisplay is null!");
            return;
        }

        if (card.cardData == null)
        {
            Debug.LogError("Card data is null for CardDisplay: " + card.name);
            return;
        }

        if (!activeCards.Contains(card))
        {
           activeCards.Add(card);
           OnCardsUpdate?.Invoke(); // Déclenche la mise à jour du HUD
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

    // Compte les cartes en excluant certains types
    public int CountCardsExcludingTypes(params CardType[] excludedTypes)
    {
        int count = activeCards.Count(card => !excludedTypes.Contains(card.cardData.cardType));
        //Debug.Log(count);
        return count;
    }

    // Récupère toutes les cartes d'un type spécifique dans activeCards et les retourne sous forme de tableau
    public CardDisplay[] GetCardsOfType(CardData.CardType type)
    {
        return activeCards.Where(card => card.cardData.cardType == type).ToArray();
    }

    public void NotifyFoodCardsUpdated()
    {
        OnCardsUpdate?.Invoke();
    }

}


