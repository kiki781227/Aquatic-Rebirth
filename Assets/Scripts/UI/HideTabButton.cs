using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabHide : MonoBehaviour
{
    public GameObject hideButton;
    public GameObject showButton;
    public GameObject tabOperations;
    public TabManager tabManager;
    public CanvasGroup tabDescriCanvasGroup;
    private AudioSource audioSource;

    private bool isTutorialTriggered = false; // Booléen pour suivre si 



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Hide();
    }

    public void Hide()
    {
        tabDescriCanvasGroup.alpha = 0;
        audioSource.Play();
        tabManager.ShowTab("Quest");
        tabOperations.SetActive(false);
        hideButton.SetActive(false);
        showButton.SetActive(true);
    }

    public void Show()
    {
        // Vérifie si le tutoriel a déjà été déclenché
        if (!isTutorialTriggered)
        {
            TutorialManager.Instance.TriggerTutorial("Laboratory");
            isTutorialTriggered = true; // Marque le tutoriel comme déclenché
        }

        tabDescriCanvasGroup.alpha = 1;
        audioSource.Play();
        tabOperations.SetActive(true);
        hideButton.SetActive(true);
        showButton.SetActive(false);
    }
}
