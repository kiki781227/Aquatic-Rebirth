using CardData;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRect;
    private Vector2 originalLocalPointerPosition; // Stockera la position du cklick sur l'ecran mais en coordonnee Canva
    private Vector3 originalPanelLocalPosition; // Stockera la position locale actuel de la carte dans le Canva
    private Vector3 originalScale;

    //private CanvasGroup canvasGroup;
 

    [HideInInspector] public bool hasBeenCounted;  
    [HideInInspector] public bool isDragging;
    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private GameObject glowEffect;
    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();  // Pour avoir le RecTransform du canva de la carte, manipuler pour que l'objet ne sort pas de l'ecran
        originalScale = rectTransform.localScale; // Stocke l'echelle initiale de l'objet
        //canvasGroup = GetComponent<CanvasGroup>();

   

    }

    // Permet d'activer des effets visuels sur la carte lorsque le curseur est dessus
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("PointerEnter activer");
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    // Enleve les effets lorsque le curseur sort de la carte
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("PointerExit activer");
        glowEffect.SetActive(false);
        rectTransform.localScale = originalScale;
    }

    // Convertir la position de l'écran en coordonnées locales du Canvas et enregistre la position actuelle de l'objet UI (carte) dans une variable
    public void OnPointerDown(PointerEventData eventData)
    {
       
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out originalLocalPointerPosition);

        originalPanelLocalPosition = rectTransform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Coin Part
        hasBeenCounted = false;
        isDragging = true;
        //Debug.Log(isDraggin);       
        transform.SetAsLastSibling();
        //canvasGroup.blocksRaycasts = false;
    }

    // Permet de calculer le deplacement lors du drag de la carte grace a sa position initiale lors du click (OnPointerDown)
    public void OnDrag(PointerEventData eventData)
    {
   

        glowEffect.SetActive(false);
        Vector2 localPointerPosition; //Stocke sa position lors du drag
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
            Vector3 newPosition = originalPanelLocalPosition + offsetToOriginal;
            rectTransform.localPosition = ClampToCanvas(newPosition);
            
        }
    }

  

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        //Debug.Log(isDragging);
        //canvasGroup.blocksRaycasts = false;
    }


    



    // Fonction qui limite la position de la carte à l'intérieur du Canvas et defini sa position
    private Vector3 ClampToCanvas(Vector3 position)
    {
        Vector3 clampedPosition = position;

        // Dimensions du Canvas
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Dimensions de la carte
        float cardWidth = rectTransform.rect.width;
        float cardHeight = rectTransform.rect.height;

        // Calcul des limites
        float minX = -canvasWidth / 2 + cardWidth / 2;
        float maxX = canvasWidth / 2 - cardWidth / 2;
        float minY = -canvasHeight / 2 + cardHeight / 2;
        float maxY = canvasHeight / 2 - cardHeight / 2;

        // Empêcher la carte de dépasser du Canvas
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        // Renvoi de la nouvelle position ajustée
        return clampedPosition;
    }


}
