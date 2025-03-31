using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameTimer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Dur�e d'une journ�e (en secondes)")]
    private float dayDuration = 120f;
    private float currentTimer;
    private int currentDay = 1;
    private bool isPaused = false; // G�re l'�tat de pause

    // R�f�rences UI
    public TMP_Text dayText;
    public Image timerBar;
    public GameObject stopIcon;
    public GameObject resumeIcon;
    public GameObject feedHumansButton; // Bouton pour nourrir les humains
    public CarteDescriptionUIManager uiManager;
    private bool isPointerOver = false;

    //
    //public CanvasGroup interactionCanvasGroup;
    public CanvasGroup cardManagerCanvasGroup;
    //public CanvasGroup buyCanvasGroup;
    public CanvasGroup btnTimerCanvasGroup;
    public GameObject questObject;
    public GameObject sellObject;
    public GameObject btnHide_ShowObject;

    void Start()
    {
        currentTimer = dayDuration;
        UpdateDayUI();
        UpdateTimerBar();
    }

    void Update()
    {
        // Si le jeu est en pause, on ne d�cr�mente/incr�mente plus
        if (isPaused) return;

        // Sinon, on incr�mente le timer
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
        // D�sactive les interactions utilisateur
        SetAllCanvasGroupState(false);


        // Affiche le bouton pour nourrir les humains
        feedHumansButton.SetActive(true);

    
        Pause();

    }

    public void FeedHumans()
    {
        GameManager.Instance.FeedHumans();

        // Masque le bouton apr�s avoir nourri les humains
        feedHumansButton.SetActive(false);


        // V�rifie si le nombre de cartes sur la table d�passe la limite
        if (GameManager.Instance.CheckCardsOnTableLimit())
        {
            SetCanvasGroupState(cardManagerCanvasGroup, true);
            sellObject.SetActive(true);

            StartCoroutine(CheckCardsOnTable());
        }
        else
        {
            // R�active les interactions utilisateur et passe � la nouvelle journ�e
            SetAllCanvasGroupState(true);
            GameManager.Instance.UpdateOceanLife(-4);
            GameManager.Instance.CheckAndApplyEnemyCardEffects();
            NewDay();
        }
    }

    private IEnumerator CheckCardsOnTable()
    {
        int maxCardOnTable = DeckManager.Instance.maxCardsOnTable;
        while (true)
        {
            int nbCardsOnTable = DeckManager.Instance.activeCards.Count;

            if (nbCardsOnTable <= maxCardOnTable)
            {
                // R�active les interactions utilisateur et passe � la nouvelle journ�e
                SetAllCanvasGroupState(true);
                NewDay();
                yield break;
            }

            // Attendre un court instant avant de v�rifier � nouveau
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

    // Appel� quand on clique sur le bouton Pause
    public void Pause()
    {
        isPaused = true;
        stopIcon.SetActive(false);
        resumeIcon.SetActive(true);
        questObject.SetActive(false);
        btnHide_ShowObject.SetActive(false);
        sellObject.SetActive(false);

    }

    // Appel� quand on clique sur le bouton Play
    public void Play()
    {
        isPaused = false;
        stopIcon.SetActive(true);
        resumeIcon.SetActive(false);
        questObject.SetActive(true);
        btnHide_ShowObject.SetActive(true);
        sellObject.SetActive(true); 
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
