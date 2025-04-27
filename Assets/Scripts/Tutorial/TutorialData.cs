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

    //[Header("Type de tutoriel")]
    //public TutorialType tutorialType; // Type de tutoriel (Standard ou TaskBased)

    [Header("Déclenchement")]
    public bool triggerAutomatically; // Si le tutoriel doit se déclencher automatiquement
   
    public string triggerAction;

    //[Header("Position du panneau")]
    //public Vector3 panelPosition; // Position souhaitée pour le panneau
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

//public enum TutorialType
//{
//    Standard,   // Tutoriel classique avec boutons et titre
//    TaskBased   // Tutoriel basé sur une tâche, avec illumination d'un objet
//}
