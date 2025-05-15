using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
public class TutorialDisplay : MonoBehaviour
{
    [Header("R�f�rences UI")]
    public GameObject tutorialUIPanel; // Panneau principal du tutoriel
    public TMP_Text tutorialTitle; // Titre du tutoriel
    public TMP_Text tutorialText; // Texte explicatif
    public Image tutorialImage; // Image ou ic�ne
    public VideoPlayer tutorialVideo; // Vid�o explicative

    [Header("Canvas Groups des Boutons")]
    public CanvasGroup nextButtonGroup; // CanvasGroup pour le bouton "Next"
    public CanvasGroup previousButtonGroup; // CanvasGroup pour le bouton "Previous"
    public CanvasGroup doneButtonGroup; // CanvasGroup pour le bouton "Done"

    public void UpdateUI(TutorialPage page, int currentPageIndex, int totalPages)
    {
        // Met � jour le texte
        tutorialText.text = page.text;

        // Met � jour le titre
        tutorialTitle.text = page.title;
     


        // Met � jour l'image
        if (page.image != null)
        {
            tutorialImage.sprite = page.image;
            tutorialImage.gameObject.SetActive(true);
        }
        else
        {
            tutorialImage.gameObject.SetActive(false);
        }

        // Met � jour la vid�o
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
            // Si c'est la premi�re page
            SetCanvasGroupVisibility(nextButtonGroup, true);
            SetCanvasGroupVisibility(previousButtonGroup, false);
            SetCanvasGroupVisibility(doneButtonGroup, false);
        }
        else if (currentPageIndex == totalPages - 1)
        {
            // Si c'est la derni�re page
            SetCanvasGroupVisibility(nextButtonGroup, false);
            SetCanvasGroupVisibility(previousButtonGroup, true);
            SetCanvasGroupVisibility(doneButtonGroup, true);
        }
        else
        {
            // Si c'est une page interm�diaire
            SetCanvasGroupVisibility(nextButtonGroup, true);
            SetCanvasGroupVisibility(previousButtonGroup, true);
            SetCanvasGroupVisibility(doneButtonGroup, false);
        }
    }


    private void SetCanvasGroupVisibility(CanvasGroup canvasGroup, bool isVisible)
    {
        canvasGroup.alpha = isVisible ? 1f : 0f; // Contr�le la visibilit�
        canvasGroup.interactable = isVisible;    // Active ou d�sactive les interactions
        canvasGroup.blocksRaycasts = isVisible;  // Active ou d�sactive les clics
    }


}
