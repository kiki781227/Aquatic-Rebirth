using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardData;

public class QuestManager : MonoBehaviour
{
    // Instance Singleton pour un accès facile depuis les slots
    public static QuestManager Instance { get; private set; }

    // Tableau contenant les slots de la phase de quête actuelle
    public QuestSlot[] questSlots;

    // Variables pour gérer les phases
    public int currentPhase = 0;
    private List<Card> questCardIdeaData = new List<Card>();
    private Dictionary<int, Card[]> phaseQuests = new Dictionary<int, Card[]>();
    

    private void Awake()
    {
        // Mise en place du Singleton
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
        

        if(DeckManager.Instance.questCardIdea != null)
        {
            questCardIdeaData = DeckManager.Instance.questCardIdea;
        }

        phaseQuests = new Dictionary<int, Card[]>()
        {
            { 0, new Card[] { questCardIdeaData[0], questCardIdeaData[3], questCardIdeaData[1], questCardIdeaData[4] } },// BeachVac, CoralPlug, BioBoom, GhostGrab
            { 1, new Card[] { questCardIdeaData[6], questCardIdeaData[7], questCardIdeaData[4], questCardIdeaData[5] } },// ReefStar, Seabin, GhostGrab*, Kelp 
            { 2, new Card[] { questCardIdeaData[2], questCardIdeaData[8], questCardIdeaData[3], questCardIdeaData[7] } },// Biorock, ShellBled, CoralPlug*, Seabin*
            { 3, new Card[] { questCardIdeaData[9], questCardIdeaData[1], questCardIdeaData[6], questCardIdeaData[2] } } // Sorbent, BioBoom*, ReefStar*, Biorock*
        };

        InitializeQuest();
    }


    // Passe à la phase suivante ou termine la quête
    public void NextPhase()
    {
        currentPhase++;
        ResetAllSlot();
        InitializeQuest();   
    }
   
    public void InitializeQuest()
    {
        Card[] actualphaseQuest = phaseQuests[currentPhase];
        int index = 0;
        foreach(QuestSlot slot in questSlots)
        {

                slot.cardDisplay.cardData = actualphaseQuest[index];
                slot.cardDisplay.UpdateCardDisplay();
                index++;
        }
        
    } 

    
    private void ResetAllSlot()
    {
        // Réinitialiser les slots pour la nouvelle phase
        foreach (QuestSlot slot in questSlots)
        {
            slot.isFilled = false;

            // Reset l'affichage du progres de la quete a 0
            slot.cardDisplay.ResetQuestProgress();
        }
    }

}
