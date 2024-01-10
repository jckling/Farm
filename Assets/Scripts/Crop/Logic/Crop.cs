using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;

    private int harvestActionCount;
    private TileDetails tileDetails;

    public void ProcessToolAction(ItemDetails tool, TileDetails tileDetails)
    {
        this.tileDetails = tileDetails;
        int requireActionCount = cropDetails.GetTotalRequireActionCount(tool.itemID);
        if (requireActionCount == -1)
        {
            return;
        }

        // TODO: hasAnimation
        if (harvestActionCount < requireActionCount)
        {
            harvestActionCount++;
            // TODO: particle effect
            // TODO: Sound
        }

        if (harvestActionCount == requireActionCount)
        {
            if (cropDetails.generateAtPlayerPosition)
            {
                SpawnHarvestItems();
            }
        }
    }

    private void SpawnHarvestItems()
    {
        for (int i = 0; i < cropDetails.producedItemID.Length; i++)
        {
            int amountToProduce = cropDetails.producedMinAmount[i] == cropDetails.producedMaxAmount[i]
                ? cropDetails.producedMinAmount[i]
                : Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMaxAmount[i] + 1);

            for (int j = 0; j < amountToProduce; j++)
            {
                if (cropDetails.generateAtPlayerPosition)
                {
                    EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID[i]);
                }
                else
                {
                    // TODO: 石头、树木等
                }
            }
        }

        if (tileDetails != null)
        {
            tileDetails.daySinceLastHarvest++;
            if (cropDetails.daysToRegrow > 0 && tileDetails.daySinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                EventHandler.CallRefreshCurrentMap();
            }
            else
            {
                tileDetails.daySinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
                // tileDetails.daySinceDug = -1;
            }

            Destroy(gameObject);
        }
    }
}