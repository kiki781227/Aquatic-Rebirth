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
    public GameObject button;
    public GameObject tabDescription;
    public GameObject timer;
    public GameObject sellPlace;
    public GameObject buyCardsPack;
    public GameObject gameOverPopup;
    public GameObject craftZone;
    public TMP_Text gameOverComments;
    public GameObject craftPrefab;

    

    [Header("Scripts")]
    public CardDisplay starterPackCard;
    public HeadUpDisplay headUpDisplay;
    public CarteDescriptionUIManager tabDescri;
    public HideTabOceanQuest tabOceanQuest;

    [Header("Param�tres des phases et qu�tes")]
    public int totalPhases = 3;           // Nombre total de phases
    public int questsPerPhase = 4;        // Nombre de qu�tes par phase
    public int oceanLifeMax = 100;  // Vie de l'oc�an (point de d�part)
    public int oceanActualLife;     
    public int oceanLifeGainPerPhase = 10; // Gain de vie par phase accomplie
    public bool isPhaseDone = false;

    [Header("Suivi de la progression")]
    public int currentPhase = 0;          // Phase en cours (0 = premi�re phase)
    public int questsCompletedThisPhase = 0; // Nombre de qu�tes accomplies dans la phase en cours

    [Header("R�f�rences (exemple)")]
    // Si tu as des GameObjects pour tes packs, tu peux les glisser ici
    public List<GameObject> cardPackObjects;

    public delegate void OceanLifeChanged(int newLife);
    public event OceanLifeChanged OnOceanLifeChanged;
    public CanvasGroup interactionCanvasGroup;  

    private void Awake()
    {
        // Gestion du Singleton (si n�cessaire)
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
        tabDescri.HideDescri();
        tabOceanQuest.Hidetab();
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
           tabDescri.ShowDescri();
           tabOceanQuest.Showtab();
           starterPackdata.isOpened = false;
        }

    }


    //-------------------- METHODES POUR RESET FOOD CARD VALUES,  FEED HUMANS ----------------------\\  
    private void ResetFoodCardValues()
    {
        CardDisplay[] foodCards = DeckManager.Instance.GetCardsOfType(CardType.Food);
        //Debug.Log($"Nombre de cartes Food trouv�es : {foodCards.Length}");
        foreach (CardDisplay foodCard in foodCards)
        {
            //Debug.Log($"R�initialisation de {foodCard.cardData.cardName} de {foodCard.cardData.value} � {foodCard.cardData.originalValue}");
            foodCard.cardData.value = foodCard.cardData.originalValue;
            foodCard.UpdateCardDisplay();
        }
    }


// Nourrit les cartes Human en fonction des exigences en nourriture
    public void FeedHumans()
    {
        // R�cup�re toutes les cartes Human sur la table
        List<CardDisplay> humanCards = new List<CardDisplay>(DeckManager.Instance.GetCardsOfType(CardType.Human));
        // R�cup�re toutes les cartes Food sur la table
        List<CardDisplay> foodCards = new List<CardDisplay>(DeckManager.Instance.GetCardsOfType(CardType.Food));

        // Trie les cartes Human par valeur d�croissante (le biologiste mange en premier)
        humanCards.Sort((a, b) => b.cardData.value.CompareTo(a.cardData.value));

        // Nourrit les cartes Human
        foreach (CardDisplay humanCard in humanCards)
        {
            int foodRequired = humanCard.cardData.value;
            int foodConsumed = 0;

            // Consomme la nourriture jusqu'� ce que les besoins soient satisfaits ou qu'il n'y ait plus de nourriture
            while (foodRequired > 0 && foodCards.Count > 0)
            {
                CardDisplay foodCard = foodCards[0];
                int foodValue = foodCard.cardData.value;

                if (foodValue <= foodRequired)
                {
                    // Consomme toute la nourriture de cette carte
                    foodConsumed += foodValue;
                    foodRequired -= foodValue;
                    foodCards.RemoveAt(0);
                    Destroy(foodCard.gameObject);
                }
                else
                {
                    // Consomme partiellement la nourriture de cette carte
                    foodConsumed += foodRequired;
                    foodCard.cardData.value -= foodRequired;
                    foodCard.UpdateCardDisplay();
                    foodRequired = 0;
                }
            }

            if (foodRequired > 0)
            {
                // Si les besoins ne sont pas satisfaits, d�truire la carte Human
                DeckManager.Instance.UnregisterCard(humanCard);
                Destroy(humanCard.gameObject);
                Debug.Log("Carte Human d�truite : " + humanCard.cardData.cardName);

                // V�rifie si le biologiste est d�truit
                if (humanCard.cardData.cardName == "Biologiste")
                {
                    ShowGameOverPopup("Pas assez de nourriture. Le biologiste est mort");
                    StopGame();
                    return;
                }
            }
        }

        //// R�initialise les valeurs des cartes Food restantes
        foreach (CardDisplay foodCard in foodCards)
        {
            foodCard.cardData.value = foodCard.cardData.originalValue;
            
        }
    }


    //-------------------- METHODE POUR START, STOP, GAMEOVER POP-UP----------------------\\
    private void StartGame(bool activer)
    {
        if (button != null) button.SetActive(activer);
        if (sellPlace != null) sellPlace.SetActive(activer);
        if (buyCardsPack != null) buyCardsPack.SetActive(activer);
        if (timer != null) timer.SetActive(activer);
        if(craftZone != null) craftZone.SetActive(activer);
        ResetFoodCardValues();
        
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

    // Arr�te le jeu
    public void StopGame()
    {
        Time.timeScale = 0f; // Met le jeu en pause
        // D�sactive les interactions utilisateur
        if (interactionCanvasGroup != null)
        {
            interactionCanvasGroup.gameObject.SetActive(false);
        }
    }

    //-------------------- UPDATE QUEST PROGRESS ----------------------\\
    // Met � jour l'�tat des qu�tes et des phases
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
                ShowGameOverPopup("VICTOIRE ! Toutes les quetes sont termines");
                StopGame();
                currentPhase = 0;
            }
            else
            {
                cardPackObjects[currentPhase].SetActive(true);
                QuestManager.Instance.NextPhase();
                cardPackObjects[currentPhase].SetActive(true);
                questsCompletedThisPhase = 0;
                UpdateOceanLife(oceanLifeGainPerPhase);
            }
        }
    }
    //----------------- CHECK CARDS ON TABLE ----------------------\\

    // V�rifie si le nombre de cartes sur la table d�passe la limite
    public bool CheckCardsOnTableLimit()
    {
        int maxCardsOnTable = DeckManager.Instance.maxCardsOnTable;
        int cardsOnTable = DeckManager.Instance.CountCardsExcludingTypes(CardType.Ennemy, CardType.Human);
        return cardsOnTable > maxCardsOnTable;
    }

    //----------------- UPDATE OCEAN LIFE ----------------------\\

    // Met � jour la vie de l'oc�an
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

        // Notifie les changements de la vie de l'oc�an
        OnOceanLifeChanged?.Invoke(oceanActualLife);
    }

    // V�rifie et met � jour la vie de l'oc�an en fonction des cartes ennemies sur la table
    public void CheckAndApplyEnemyCardEffects()
    {
        CardDisplay[] enemyCards = DeckManager.Instance.GetCardsOfType(CardType.Ennemy).ToArray();
        int totalValue = 0;
        foreach(CardDisplay enemyCard in enemyCards)
        {
            totalValue += enemyCard.cardData.value;
        }
        UpdateOceanLife(-(totalValue));
    }

    





}

