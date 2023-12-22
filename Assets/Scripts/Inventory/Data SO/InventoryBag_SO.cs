using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList_SO", menuName = "Farm / Create InventoryBag_SO")]
public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> itemList;
}