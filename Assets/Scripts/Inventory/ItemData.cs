using UnityEngine;

public enum ItemType
{
    None,
    Building,
    Item,
    Tool,
    Resources
}

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;

    [Header("Stacking")]
    public int maxStack = 1;

    [Header("Type")]
    public ItemType itemType;

    [Header("Prefab (what player holds)")]
    public GameObject handPrefab;

    [Header("Prefab (what player places)")]
    public GameObject placePrefab;
}