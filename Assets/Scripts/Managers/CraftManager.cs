using CardData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance { get; private set; }
    public List<CraftingRecipe> recipes;
    public CardSpawner cardSpawner;
    private float actualCooldown;
    private bool inCooldown = false;


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
        LoadRecipes();
    }

    public void LoadRecipes()
    {
        recipes.AddRange(Resources.LoadAll<CraftingRecipe>("Recipe/OceanQuest"));
        recipes.AddRange(Resources.LoadAll<CraftingRecipe>("Recipe/Tool"));
        recipes.AddRange(Resources.LoadAll<CraftingRecipe>("Recipe/ToolProduced"));
    }


    public void Craft(CraftingSlot craftingSlot)
    {
        // Nettoyer les objets détruits avant de commencer le crafting
        RemoveDestroyedObjects(craftingSlot.cardObjectsInSlot);

        StartCoroutine(CraftCoroutine(craftingSlot));
    }

    private IEnumerator CraftCoroutine(CraftingSlot craftingSlot)
    {
        List<GameObject> cardObjectsInSlot = craftingSlot.cardObjectsInSlot;
        Dictionary<Card, int> cardsInSlot = new Dictionary<Card, int>();
        Card humanCard = null;
        Card toolCard = null;
        GameObject humanCardObject = null;

        foreach (GameObject cardObject in cardObjectsInSlot)
        {
            CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                Card cardData = cardDisplay.cardData;
                if (cardsInSlot.ContainsKey(cardData))
                {
                    cardsInSlot[cardData]++;
                }
                else
                {
                    cardsInSlot[cardData] = 1;
                }

                if (cardData.cardType == CardType.Human)
                {
                    humanCard = cardData;
                    humanCardObject = cardObject;
                }
                else if(cardData.cardType == CardType.ToolCrafted)
                {
                    toolCard = cardData; // La carte humaine peut etre detruite au cours d'un craft
                }
            }
        }

        // Si aucune carte humaine n'est trouvée, ou si elle est déjà détruite
        if (humanCard == null || humanCardObject == null)
        {
            Debug.Log("Aucune carte humaine trouvée dans le slot de crafting ou la carte humaine a été détruite.");
            RespawnCards(craftingSlot);
            yield break;
        }

        foreach (CraftingRecipe recipe in recipes)
        {
            if (ContainsIngredients(cardsInSlot, recipe.GetIngredientsDictionary()))
            {
                craftingSlot.StartCooldown(recipe.cooldown);
                inCooldown = true;

                // Attendre que le cooldown soit terminé
                while (inCooldown)
                {

                    yield return null; // Attendre une frame
                }

                // Vérifiez si la carte humaine existe toujours
                if (humanCardObject == null)
                {
                    Debug.Log("La carte humaine a été détruite pendant le cooldown.");
                    RemoveDestroyedObjects(craftingSlot.cardObjectsInSlot);
                    RespawnCards(craftingSlot);
                    yield break;
                }

                if(toolCard != null)
                {
                    cardSpawner.SpawnCard(toolCard);
                }

                // Instructions après le cooldown
                cardSpawner.SpawnCard(recipe.result);
                cardSpawner.SpawnCard(humanCard);
                
                DestroyIngredients(craftingSlot, recipe.GetIngredientsDictionary());
                craftingSlot.ClearSlot();
                yield break;
            }
        }

        // Si aucun crafting n'a réussi, respawn les cartes
        RespawnCards(craftingSlot);
    }


    private bool ContainsIngredients(Dictionary<Card, int> cardsInSlot, Dictionary<Card, int> ingredients)
    {

        foreach (KeyValuePair<Card, int> ingredient in ingredients)
        {
            // Vérifiez si la carte est présente dans le slot de crafting et si la quantité est suffisante
            if (!cardsInSlot.ContainsKey(ingredient.Key) || cardsInSlot[ingredient.Key] < ingredient.Value)
            {
                return false;
            }
        }
        return true;
    }

    private void DestroyIngredients(CraftingSlot craftingSlot, Dictionary<Card, int> ingredients)
    {
         foreach (GameObject cardObject in craftingSlot.cardObjectsInSlot)
        {
            Debug.Log("Destroying card: " + cardObject.GetComponent<CardDisplay>().cardData.cardName);
            Destroy(cardObject);
        }
    }

    private void RespawnCards(CraftingSlot craftingSlot)
    {
        foreach (GameObject cardObject in craftingSlot.cardObjectsInSlot)
        {
            cardSpawner.SpawnCard(cardObject.GetComponent<CardDisplay>().cardData);
        }
        craftingSlot.ClearSlot();
    }

    public void UpdateCooldwon(float cooldown)
    {

        actualCooldown = cooldown;

        if (actualCooldown <= 0) inCooldown = false;
        else inCooldown = true;
    }

    private void RemoveDestroyedObjects(List<GameObject> cardObjectsInSlot)
    {
        //int initialCount = cardObjectsInSlot.Count;
        cardObjectsInSlot.RemoveAll(card => card == null);
        //int removedCount = initialCount - cardObjectsInSlot.Count;

        //if (removedCount > 0)
        //{
        //    Debug.Log($"Removed {removedCount} destroyed objects from cardObjectsInSlot.");
        //}
    }

}


