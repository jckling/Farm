using System;
using UnityEngine;

[Serializable]
public class CropDetails
{
    public int seedItemID;

    [Header("不同阶段的生长天数")] public int[] growthDays;

    public int TotalGrowthDays
    {
        get
        {
            int amount = 0;
            foreach (var days in growthDays)
            {
                amount += days;
            }

            return amount;
        }
    }

    [Header("不同阶段的Prefab")] public GameObject[] growthPrefabs;
    [Header("不同阶段的图片")] public Sprite[] growthSprites;
    [Header("播种季节")] public Season[] seasons;

    [Space] [Header("收获工具")] public int[] harvestToolItemID;
    [Header("收获次数")] public int[] requireActionCount;
    [Header("转换")] public int transferItemID;

    [Space] [Header("果实信息")] public int[] producedItemID;
    public int[] producedMinAmount;
    public int[] producedMaxAmount;
    public Vector2 spawnRadius;

    [Header("再次生长")] public int daysToRegrow;
    public int regrowTimes;

    [Header("其他选项")] public bool generateAtPlayerPosition;
    public bool hasAnimation;
    public bool hasParticleEffect;

    public bool CheckToolValid(int toolID)
    {
        foreach (var itemID in harvestToolItemID)
        {
            if (toolID == itemID)
            {
                return true;
            }
        }

        return false;
    }

    public int GetTotalRequireActionCount(int toolID)
    {
        for (int i = 0; i < harvestToolItemID.Length; i++)
        {
            if (harvestToolItemID[i] == toolID)
            {
                return requireActionCount[i];
            }
        }

        return -1;
    }
}