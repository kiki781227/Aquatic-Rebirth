using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
public class TutorialDisplay : MonoBehaviour
{
    [Header("Références UI")]
    public GameObject tutorialUIPanel; // Panneau principal du tutoriel
    public TMP_Text tutorialTitle; // Titre du tutoriel
    public TMP_Text tutorialText; // Texte explicatif
    public Image tutorialImage; // Image ou icône
    public VideoPlayer tutorialVideo; // Vidéo explicative

    [Header("Canvas Groups des Boutons")]
    public CanvasGroup nextButtonGroup; // CanvasGroup pour le bouton "Next"
    public CanvasGroup previousButtonGroup; // CanvasGroup pour le bouton "Previous"
    public CanvasGroup doneButtonGroup; // CanvasGroup pour le bouton "Done"

    public void UpdateUI(TutorialPage page, int currentPageIndex, int totalPages)
    {
        // Met à jour le texte
        tutorialText.text = page.text;

        // Met à jour le titre
        tutorialTitle.text = page.title;
     


        // Met à jour l'image
        if (page.image != null)
        {
            tutorialImage.sprite = page.image;
            tutorialImage.gameObject.SetActive(true);
        }
        else
        {
            tutorialImage.gameObject.SetActive(false);
        }

        // Met à jour la vidéo
        if (page.video != null)
        {
            tutorialVideo.clip = page.video;
            tutorialVideo.gameObject.SetActive(true);
            tutorialVideo.Play();
        }
        else
        {
            tutorialVideo.gameObject.SetActive(false);
        }

        UpdateButtons(currentPageIndex, totalPages);
    }

    public void UpdateButtons(int currentPageIndex, int totalPages)
    {
        if (totalPages == 1)
        {
            // Si une seule page, seul le bouton "Done" est visible
            SetCanvasGroupVisibility(nextButtonGroup, false);
            SetCanvasGroupVisibility(previousButtonGroup, false);
            SetCanvasGroupVisibility(doneButtonGroup, true);
        }
        else if (currentPageIndex == 0)
        {
            // Si c'est la première page
            SetCanvasGroupVisibility(nextButtonGroup, true);
            SetCanvasGroupVisibility(previousButtonGroup, false);
            SetCanvasGroupVisibility(doneButtonGroup, false);
        }
        else if (currentPageIndex == totalPages - 1)
        {
            // Si c'est la dernière page
            SetCanvasGroupVisibility(nextButtonGroup, false);
            SetCanvasGroupVisibility(previousButtonGroup, true);
            SetCanvasGroupVisibility(doneButtonGroup, true);
        }
        else
        {
            // Si c'est une page intermédiaire
            SetCanvasGroupVisibility(nextButtonGroup, true);
            SetCanvasGroupVisibility(previousButtonGroup, true);
            SetCanvasGroupVisibility(doneButtonGroup, false);
        }
    }


    private void SetCanvasGroupVisibility(CanvasGroup canvasGroup, bool isVisible)
    {
        canvasGroup.alpha = isVisible ? 1f : 0f; // Contrôle la visibilité
        canvasGroup.interactable = isVisible;    // Active ou désactive les interactions
        canvasGroup.blocksRaycasts = isVisible;  // Active ou désactive les clics
    }


}
