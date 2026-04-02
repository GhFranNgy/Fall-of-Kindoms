using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public RectTransform rectTransform;
    public InventoryItem currentItem;

    private bool isHighlighted = false;
    private Vector3 normalScale = Vector3.one;
    private Vector3 targetScale;

    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float smoothSpeed = 10f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        targetScale = normalScale;
    }

    private void Update()
    {
        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }

    public void SetHighlight(bool value)
    {
        isHighlighted = value;
        targetScale = isHighlighted ? normalScale * scaleMultiplier : normalScale;
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
    }

    public void Clear()
    {
        currentItem = null;
    }
}