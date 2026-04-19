using UnityEngine;
using UnityEngine.EventSystems;

public class HelperUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;
    [SerializeField] private GameObject visualizer;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            Vector2 newPos = localPoint - offset;

            float leftLimit = rectTransform.rect.width * rectTransform.pivot.x - 175;

            if (newPos.x > leftLimit)
                newPos.x = leftLimit;

            rectTransform.localPosition = newPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {}

    void Update()
    {
    }

    public void Visualise()
    {
        if(visualizer.activeInHierarchy)
        {
            visualizer.SetActive(false);
        }
        else
        {
            visualizer.SetActive(true);
        }
    }
}