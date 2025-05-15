using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameTimer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    [Header("Donnee typee")]
    private float dayDuration = 170f;
    private float currentTimer;
    private int currentDay = 1;
    private bool isPaused = false; // Gère l'état de pause
    private bool isPointerOver = false;
    private bool isTutorialTriggered = false; // Gère l'état du tutoriel


    [Header("References UI")]
    public TMP_Text dayText;
    public Image timerBar;
    private AudioSource audioSource;

    [Header("Script")]
    public CarteDescriptionUIManager uiManager;
    public TabHide tabHide;

    [Header("CanvasGroup")]
    public CanvasGroup cardManagerCanvasGroup; 
    public CanvasGroup btnTimerCanvasGroup;
    public CanvasGroup btnHide_ShowCanvasGroup;
    public CanvasGroup bckgrndOverlayCanvasGroup;


    [Header("Objets")]
    public GameObject stopIcon;
    public GameObject resumeIcon;
    public GameObject feedHumansButton; // Bouton pour nourrir les humains
    public GameObject questObject;
    public GameObject sellObject;
    public GameObject craftZone;
    //public GameObject pauseTxt;
    public GameObject buyplace;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Start est appele");
        currentTimer = dayDuration;
        UpdateDayUI();
        UpdateTimerBar();
    }

    void Update()
    {
        // Si le jeu est en pause, on ne décrémente/incrémente plus
        if (isPaused) return;

        // Sinon, on incrémente le timer
        currentTimer -= Time.deltaTime;
        //Debug.Log(currentTimer);
        UpdateTimerBar();

        if (isPointerOver)
        {
            UpdateTimerDisplay();
        }

        if (currentTimer <= 0)
        {
            EndOfDay();
        }
    }

    void EndOfDay()
    {

        if(!isTutorialTriggered)
        {
            TutorialManager.Instance.TriggerTutorial("Timer");
            isTutorialTriggered = true; // Marque le tutoriel comme déclenché
        }

        // Désactive les interactions utilisateur
        SetAllCanvasGroupState(false);


        // Affiche le bouton pour nourrir les humains
        feedHumansButton.SetActive(true);

    
        Pause();

    }

    public void FeedHumans()
    {
        audioSource.Play();
        if (GameManager.Instance.FeedHumans() == false) return;
        //GameManager.Instance.FeedHumans();

        // Masque le bouton après avoir nourri les humains
        feedHumansButton.SetActive(false);


        // Vérifie si le nombre de cartes sur la table dépasse la limite
        if (GameManager.Instance.CheckCardsOnTableLimit())
        {
            SetCanvasGroupState(cardManagerCanvasGroup, true);
            sellObject.SetActive(true);

            StartCoroutine(CheckCardsOnTable());
        }
        else
        {
            // Réactive les interactions utilisateur et passe à la nouvelle journée
            SetAllCanvasGroupState(true);
            GameManager.Instance.UpdateOceanLife(-4);
            GameManager.Instance.CheckAndApplyEnemyCardEffects();
            NewDay();
        }
    }

    private IEnumerator CheckCardsOnTable()
    {
        int maxCardOnTable = DeckManager.Instance.maxCardsOnTable;
        Debug.Log("Limite de carte sur table: " + maxCardOnTable);
        while (true)
        {
            int nbCardsOnTable = DeckManager.Instance.CountCardsExcludingTypes(CardData.CardType.Ennemy,CardData.CardType.Human);
            Debug.Log(" carte sur table: " + nbCardsOnTable);
            if (nbCardsOnTable <= maxCardOnTable)
            {
                Debug.Log("carte sur table est inferieure a la limite");
                // Réactive les interactions utilisateur et passe à la nouvelle journée
                SetAllCanvasGroupState(true);
                NewDay();
                yield break;
            }

            // Attendre un court instant avant de vérifier à nouveau
            yield return new WaitForSeconds(0.5f);
        }
    }
    void NewDay()
    {
        currentDay++;
        currentTimer = dayDuration;
        UpdateDayUI();
        UpdateTimerBar();
        Play(); 
    }

    void UpdateDayUI()
    {
        if (dayText != null)
        {
            dayText.text = "Jour " + currentDay;
        }
    }

    void UpdateTimerBar()
    {
        if (timerBar != null)
        {
            timerBar.fillAmount = 1- (currentTimer / dayDuration);
        }
    }

    void UpdateTimerDisplay()
    {
        if (uiManager != null)
        {
            uiManager.UpdateTimerDisplay(currentTimer);
        }
    }

    // Appelé quand on clique sur le bouton Pause
    public void Pause()
    {
        bckgrndOverlayCanvasGroup.alpha = 1f;
        audioSource.Play();
        isPaused = true;

        
        stopIcon.SetActive(false);
        resumeIcon.SetActive(true);

        buyplace.SetActive(false);
        sellObject.SetActive(false);
        //pauseTxt.SetActive(true);

        SetCanvasGroupState(cardManagerCanvasGroup, false);

        tabHide.Hide();
        btnHide_ShowCanvasGroup.interactable = false;
        btnHide_ShowCanvasGroup.blocksRaycasts = false;
    }

    // Appelé quand on clique sur le bouton Play
    public void Play()
    {
        bckgrndOverlayCanvasGroup.alpha = 0f;
        audioSource.Play();
        isPaused = false;

        SetCanvasGroupState(cardManagerCanvasGroup, true);

        stopIcon.SetActive(true);
        resumeIcon.SetActive(false);

        buyplace.SetActive(true);
        sellObject.SetActive(true);
        //pauseTxt.SetActive(false);
       
        
        
        btnHide_ShowCanvasGroup.interactable = true;
        btnHide_ShowCanvasGroup.blocksRaycasts = true;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        //Debug.Log("PointerEnter GameTimer activer");
        UpdateTimerDisplay();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        //Debug.Log("PointerExit GameTimer activer");
        uiManager.ClearDescription();
    }

    private void SetCanvasGroupState(CanvasGroup canvasGroup, bool state)
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;

        }
    }

    private void SetAllCanvasGroupState(bool state)
    {
        SetCanvasGroupState(cardManagerCanvasGroup, state);
        SetCanvasGroupState(btnTimerCanvasGroup, state);

    }



}
