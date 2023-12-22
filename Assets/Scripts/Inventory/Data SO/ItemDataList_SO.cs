using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList_SO", menuName = "Farm / Create ItemDataList_SO")]
public class ItemDataList_SO : ScriptableObject
{
    public List<ItemDetails> itemDetailsList;
}