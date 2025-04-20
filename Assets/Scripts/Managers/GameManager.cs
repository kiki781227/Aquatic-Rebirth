using CardData;
using CardPackData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    // Singleton (optionnel)
    public static GameManager Instance { get; private set; }

    public CardPack starterPackdata;

    [Header("Les objEts de la scene")]
    public GameObject buttonHide_Show;
    public GameObject timer;
    public GameObject sellPlace;
    public GameObject buyCardsPack;
    public GameObject gameOverPopup;
    public TMP_Text gameOverComments;
    public GameObject craftPrefab;
    public GameObject logo;
    public GameObject tabOperations;

    

    [Header("Scripts")]
    public CardDisplay starterPackCard;
    public HeadUpDisplay headUpDisplay;
   
    

    [Header("Paramètres des phases et quêtes")]
    public int totalPhases = 3;           // Nombre total de phases
    public int questsPerPhase = 4;        // Nombre de quêtes par phase
    public int oceanLifeMax = 100;  // Vie de l'océan (point de départ)
    public int oceanActualLife;     
    public int oceanLifeGainPerPhase = 10; // Gain de vie par phase accomplie
    //public bool isPhaseDone = false;

    [Header("Suivi de la progression")]
    public int currentPhase = 0;          // Phase en cours (0 = première phase)
    public int questsCompletedThisPhase = 0; // Nombre de quêtes accomplies dans la phase en cours

    [Header("Références (exemple)")]
    // GameObject pakcs
    public List<GameObject> cardPackObjects;

    
    public delegate void OceanLifeChanged(int newLife);
    public event OceanLifeChanged OnOceanLifeChanged;

    // Canvas groupe pour toutes les interactions utilisateur ( le GameObject Background)
    public CanvasGroup interactionCanvasGroup;
    public CanvasGroup tabDescriCanvasGroup;

    private void Awake()
    {
        // Gestion du Singleton (si nécessaire)
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        StartGame(false);
        headUpDisplay.HideHUD();
        
       
        oceanActualLife = Random.Range(50, 81);
        Debug.Log($"PHASE NUMERO {currentPhase + 1}");

        //Debug.Log($"{Time.time} Start: {gameObject.name}");
        if (starterPackCard != null)
        {
            starterPackdata = starterPackCard.cardData as CardPack;
            if (starterPackdata == null) Debug.LogError("Le CardData n'est pas un CardPack !");
        }
    }

    private void Update()
    {
        if (starterPackdata != null && starterPackdata.isOpened)
        {
           StartGame(true);
           headUpDisplay.ShowHUD();
           starterPackdata.isOpened = false;
        }

    }


    //-------------------- METHODES POUR RESET FOOD CARD VALUES,  FEED HUMANS ----------------------\\  


// Nourrit les cartes Human en fonction des exigences en nourriture
    public bool FeedHumans()
    {
        bool isValidate = true;
        // Récupère toutes les cartes Human sur la table
        List<CardDisplay> humanCards = new List<CardDisplay>(DeckManager.Instance.GetCardsOfType(CardType.Human));
        // Récupère toutes les cartes Food sur la table
        List<CardDisplay> foodCards = new List<CardDisplay>(DeckManager.Instance.GetCardsOfType(CardType.Food));

        // Trie les cartes Human par valeur décroissante (le biologiste mange en premier)
        humanCards.Sort((a, b) => b.cardData.value.CompareTo(a.cardData.value));

        // Nourrit les cartes Human
        foreach (CardDisplay humanCard in humanCards)
        {
            int foodRequired = humanCard.cardData.value;
            int foodConsumed = 0;

            // Consomme la nourriture jusqu'à ce que les besoins soient satisfaits ou qu'il n'y ait plus de nourriture
            while (foodRequired > 0 && foodCards.Count > 0)
            {
                CardDisplay foodCard = foodCards[0];
                int foodValue = foodCard.cardData.value;

                if (foodValue <= foodRequired)
                {
                    // Consomme toute la nourriture de cette carte
                    foodConsumed += foodValue;
                    foodRequired -= foodValue;
                    //foodCard.cardData.value = foodCard.cardData.originalValue;
                    //Debug.Log("Consomer toute la carte"+ foodCard.cardData.value);
                    foodCards.RemoveAt(0);
                    Destroy(foodCard.gameObject);
                    
                }
                else
                {
                    //Debug.Log("Consommer partiellement la carte: ");
                    // Consomme partiellement la nourriture de cette carte
                    foodConsumed += foodRequired;
                    //Debug.Log($"Nourriture consommer par {humanCard.cardData.cardName} : {foodConsumed}");
                    foodCard.cardData.value -= foodRequired;
                    //Debug.Log($"Reste de la nourriture");
                    foodCard.UpdateCardDisplay();
                    foodRequired = 0;
                    
                }
                DeckManager.Instance.NotifyFoodCardsUpdated();
            }

            if (foodRequired > 0)
            {
                // Si les besoins ne sont pas satisfaits, détruire la carte Human
                DeckManager.Instance.UnregisterCard(humanCard);
                Destroy(humanCard.gameObject);
                Debug.Log("Carte Human détruite : " + humanCard.cardData.cardName);

                // Vérifie si le biologiste est détruit
                if (humanCard.cardData.cardName == "Biologiste")
                {
                    ShowGameOverPopup("Pas assez de nourriture. Le biologiste est mort");
                    StopGame();
                    return !isValidate;
                }
            }

            
        }
        return isValidate;

    }


    //-------------------- METHODE POUR START, STOP, GAMEOVER POP-UP----------------------\\
    private void StartGame(bool activer)
    {
        Time.timeScale = 1f; // Met le jeu en pause
        if (buttonHide_Show != null) buttonHide_Show.SetActive(activer);
        if (sellPlace != null) sellPlace.SetActive(activer);
        if (buyCardsPack != null) buyCardsPack.SetActive(activer);
        if (timer != null) timer.SetActive(activer);
        if(tabOperations != null) tabOperations.SetActive(activer);
        if (logo != null) logo.SetActive(!activer);
        if(tabDescriCanvasGroup != null) tabDescriCanvasGroup.alpha = activer ? 1 : 0;
    }

    // Affiche le pop-up "Game Over"
    public void ShowGameOverPopup(string comments)
    {
        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true);
            gameOverComments.text = comments;   
        }
    }

    // Arrête le jeu
    public void StopGame()
    {
        Time.timeScale = 0f; // Met le jeu en pause
        // Désactive les interactions utilisateur
        if (interactionCanvasGroup != null)
        {
            interactionCanvasGroup.gameObject.SetActive(false);
        }
    }

    //-------------------- UPDATE QUEST PROGRESS ----------------------\\
    // Met à jour l'état des quêtes et des phases
    public void UpdateQuestProgress()
    {
        questsCompletedThisPhase ++;
        if (questsCompletedThisPhase >= questsPerPhase)
        {
            currentPhase++;
            Debug.Log(currentPhase);
            Debug.Log($"PHASE NUMERO {currentPhase + 1}");
            questsCompletedThisPhase = 0;
            if (currentPhase > totalPhases)
            {
                ShowGameOverPopup("VICTOIRE ! Toutes les quetes sont termines. La vie de l'ocean est de : " + oceanActualLife.ToString());
                StopGame();
                currentPhase = 0;
            }
            else
            {
                cardPackObjects[currentPhase].SetActive(true);
                QuestManager.Instance.NextPhase();
                //cardPackObjects[currentPhase].SetActive(true);
                questsCompletedThisPhase = 0;
                UpdateOceanLife(oceanLifeGainPerPhase);
            }
        }
    }
    //----------------- CHECK CARDS ON TABLE ----------------------\\

    // Vérifie si le nombre de cartes sur la table dépasse la limite
    public bool CheckCardsOnTableLimit()
    {
        int maxCardsOnTable = DeckManager.Instance.maxCardsOnTable;
        int cardsOnTable = DeckManager.Instance.CountCardsExcludingTypes(CardType.Ennemy, CardType.Human);
        return cardsOnTable > maxCardsOnTable;
    }

    //----------------- UPDATE OCEAN LIFE ----------------------\\

    // Met à jour la vie de l'océan
    public void UpdateOceanLife(int value)
    {
        oceanActualLife += value;
        if (oceanActualLife > oceanLifeMax)
        {
            oceanActualLife = oceanLifeMax;
        }
        else if (oceanActualLife <= 0)
        {
            oceanActualLife = 0;
            StopGame();
            ShowGameOverPopup("La vie de l'ocean est a 0");
        }

        // Notifie les changements de la vie de l'océan
        OnOceanLifeChanged?.Invoke(oceanActualLife);
    }

    // Vérifie et met à jour la vie de l'océan en fonction des cartes ennemies sur la table
    public void CheckAndApplyEnemyCardEffects()
    {
        CardDisplay[] enemyCards = DeckManager.Instance.GetCardsOfType(CardType.Ennemy).ToArray();
        int totalValue = 0;
        if(enemyCards.Length == 0)
        {
            Debug.Log("Aucune carte ennemie sur la table");
            return;
        }
        else
        {
            foreach (CardDisplay enemyCard in enemyCards)
            {
                totalValue += enemyCard.cardData.value;
            }
            UpdateOceanLife(-(totalValue));
        }

    }


 





}

