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
    private List<TutorialPage> allTutorialPages = new List<TutorialPage>(); // Liste de toutes les pages des tutoriels


    [Header("Références")]
    public TutorialDisplay tutorialDisplay; // Référence au gestionnaire d'affichage

    private int currentPageIndex = 0;
    private TutorialData currentTutorial;
    private bool isTutorialRunning = false; // Indique si un tutoriel est en cours

    public GameObject bckgrndOverlayTuto;

 

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
        TriggerTutorial("Welcome"); // Démarre le tutoriel de bienvenue
    }


    public void StartTutorial(TutorialData tutorial)
    {

        if (tutorial == null || tutorial.pages == null || tutorial.pages.Count == 0)
        {
            Debug.LogWarning("Le tutoriel est vide ou invalide.");
            return;
        }

        currentTutorial = tutorial;
        currentPageIndex = 0;
        isTutorialRunning = true; // Indique qu'un tutoriel est en cours

        bckgrndOverlayTuto.SetActive(true);
        Time.timeScale = 0;


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
        bckgrndOverlayTuto.SetActive(false);
        Time.timeScale = 1;
        currentTutorial = null;
        currentPageIndex = 0;
        tutorialDisplay.tutorialUIPanel.SetActive(false); // Masque le panneau du tutoriel
        isTutorialRunning = false;

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
