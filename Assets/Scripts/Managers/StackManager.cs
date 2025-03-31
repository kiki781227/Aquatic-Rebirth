using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackManager : MonoBehaviour
{
    public GameObject parentStack;
    private VerticalLayoutGroup VerticalLayoutGroup;
    private void Start()
    {
        VerticalLayoutGroup = parentStack.GetComponent<VerticalLayoutGroup>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.SetParent(parentStack.transform, false); // Ne pas modifier la position globale
        StartCoroutine(UpdateLayout());
    }

    private IEnumerator UpdateLayout()
    {
        yield return new WaitForEndOfFrame(); // Attendre la fin de la frame
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentStack.GetComponent<RectTransform>());
    }

}
