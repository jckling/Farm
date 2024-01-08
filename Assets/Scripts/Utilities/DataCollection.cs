using System;
using UnityEngine;

[Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public int itemUseRadius;
    public bool canPickedup;
    public bool canDropped;
    public bool canCarried;
    public int itemPrice;
    [Range(0, 1)] public float sellPercentage;
}


[Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}

[Serializable]
public struct AnimatorType
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}

[Serializable]
public class SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

[Serializable]
public class SceneItem
{
    public int itemID;
    public SerializableVector3 position;
}

[Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;
    public GridType gridType;
    public bool boolTypeValue;
}

[Serializable]
public class TileDetails
{
    public int gridX, gridY;
    public bool canDig;
    public bool canDropItem;
    public bool canPlaceFurniture;
    public bool isNPCObstacles;
    public int daySinceDug = -1;
    public int daySinceWatered = -1;
    public int seedItemID = -1;
    public int growthDays = -1;
    public int daySinceLastHarvest = -1;
}