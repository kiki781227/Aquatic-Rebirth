using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;


[CreateAssetMenu(fileName = "NewTutorial", menuName = "Tutorial/TutorialData")]
public class TutorialData : ScriptableObject
{
    [Header("Nom du tutoriel")]
   
    public string tutorialName;

    [Header("Pages du tutoriel")]
    public List<TutorialPage> pages;


    [Header("Déclenchement")]
    public string triggerAction;


}

[System.Serializable]
public class TutorialPage
{
    public string title; // Titre de la page
    [TextArea(10, 10)]
    public string text; // Texte explicatif
    public Sprite image; // Image ou icône
    public VideoClip video; // Vidéo explicative
    public GameObject highlightObject; // Objet à illuminer pour les tutoriels TaskBased

}

