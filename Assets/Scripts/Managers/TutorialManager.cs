using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }


    [Header("Tutoriels disponibles")]
    public List<TutorialData> tutorials; // Liste des tutoriels disponibles

    [Header("Références")]
    public TutorialDisplay tutorialDisplay; // Référence au gestionnaire d'affichage

    private Queue<TutorialData> tutorialQueue = new Queue<TutorialData>(); // File d'attente des tutoriels automatiques
    private int currentPageIndex = 0;
    private TutorialData currentTutorial;
    private bool isTutorialRunning = false; // Indique si un tutoriel est en cours

    public GameObject timer;
    public GameObject laboratoryPanel;
    public GameObject tabDescription;
    public GameObject laboratoryBtn;
    public CanvasGroup bckgrndOverlay;
   

    private void Awake()
    {
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
        // Ajouter les tutoriels automatiques à la file d'attente
        foreach (var tutorial in tutorials)
        {
            if (tutorial.triggerAutomatically)
            {
                tutorialQueue.Enqueue(tutorial);
            }
        }

        // Démarrer le premier tutoriel automatique
        StartNextTutorial();
    }

    private void StartNextTutorial()
    {
        if (tutorialQueue.Count > 0)
        {
            var nextTutorial = tutorialQueue.Dequeue();
            StartTutorial(nextTutorial);
        }
        else
        {
            Debug.Log("Tous les tutoriels automatiques sont terminés.");
            isTutorialRunning = false;
        }
    }

    public void StartTutorial(TutorialData tutorial)
    {

        if (tutorial.tutorialName != "Welcome")
        {
            timer.SetActive(false);
            laboratoryPanel.SetActive(false);
            tabDescription.SetActive(false);
            laboratoryBtn.SetActive(false);
            bckgrndOverlay.alpha = 1;
        }        


        if (tutorial == null || tutorial.pages == null || tutorial.pages.Count == 0)
        {
            Debug.LogWarning("Le tutoriel est vide ou invalide.");
            return;
        }



        currentTutorial = tutorial;
        currentPageIndex = 0;
        isTutorialRunning = true; // Indique qu'un tutoriel est en cours

        // Applique la position du panneau
        //tutorialDisplay.UpdatePanelPosition(tutorial.panelPosition);

        tutorialDisplay.tutorialUIPanel.SetActive(true); // Affiche le panneau du tutoriel
        ShowPage(currentPageIndex);
    }

    

    private void ShowPage(int pageIndex)
    {
        if (currentTutorial == null || pageIndex < 0 || pageIndex >= currentTutorial.pages.Count)
        {
            Debug.LogWarning("Index de page invalide ou tutoriel non défini.");
            return;
        }

        TutorialPage page = currentTutorial.pages[pageIndex];
        tutorialDisplay.UpdateUI(page, currentPageIndex, currentTutorial.pages.Count);
    }

    public void NextPage()
    {
        if (currentPageIndex < currentTutorial.pages.Count - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
        else
        {
            Debug.Log("Fin du tutoriel atteinte.");
            EndTutorial();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowPage(currentPageIndex);
        }
        else
        {
            Debug.Log("Première page atteinte.");
        }
    }

    public void EndTutorial()
    {
        if (currentTutorial.tutorialName != "Welcome")
        {
            timer.SetActive(true);
            laboratoryPanel.SetActive(true);
            tabDescription.SetActive(true);
            laboratoryBtn.SetActive(true); 
            bckgrndOverlay.alpha = 0; 
        }

        currentTutorial = null;
        currentPageIndex = 0;
        tutorialDisplay.tutorialUIPanel.SetActive(false); // Masque le panneau du tutoriel
        isTutorialRunning = false;

        // Passe au tutoriel suivant dans la file d'attente
        StartNextTutorial();
    }


    public void TriggerTutorial(string action)
    {
        if (isTutorialRunning) return; // Ignore si un tutoriel est déjà en cours

        foreach (var tutorial in tutorials)
        {
            if (tutorial.triggerAction == action)
            {
                StartTutorial(tutorial);
                return;
            }
        }

        Debug.LogWarning($"Aucun tutoriel trouvé pour l'action : {action}");
    }


}
