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

    [Header("D�clenchement")]
    public bool triggerAutomatically; // Si le tutoriel doit se d�clencher automatiquement
   
    public string triggerAction;

    //[Header("Position du panneau")]
    //public Vector3 panelPosition; // Position souhait�e pour le panneau
}

[System.Serializable]
public class TutorialPage
{
    public string title; // Titre de la page
    [TextArea(10, 10)]
    public string text; // Texte explicatif
    public Sprite image; // Image ou ic�ne
    public VideoClip video; // Vid�o explicative
    public GameObject highlightObject; // Objet � illuminer pour les tutoriels TaskBased

}

//public enum TutorialType
//{
//    Standard,   // Tutoriel classique avec boutons et titre
//    TaskBased   // Tutoriel bas� sur une t�che, avec illumination d'un objet
//}
