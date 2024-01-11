using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    public TileDetails tileDetails;
    public bool CanHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;

    private int harvestActionCount;
    private Animator anim;
    private Transform playerTransform => FindObjectOfType<Player>().transform;

    public void ProcessToolAction(ItemDetails tool, TileDetails tileDetails)
    {
        this.tileDetails = tileDetails;
        int requireActionCount = cropDetails.GetTotalRequireActionCount(tool.itemID);
        if (requireActionCount == -1)
        {
            return;
        }

        anim = GetComponentInChildren<Animator>();

        if (harvestActionCount < requireActionCount)
        {
            harvestActionCount++;

            if (anim != null && cropDetails.hasAnimation)
            {
                anim.SetTrigger(playerTransform.position.x < transform.position.x ? "RotateRight" : "RotateLeft");
            }

            if (cropDetails.hasParticleEffect)
            {
                EventHandler.CallParticleEffectEvent(cropDetails.effectType,
                    transform.position + cropDetails.effectPos);
            }

            // TODO: Sound
        }

        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generateAtPlayerPosition || !cropDetails.hasAnimation)
            {
                SpawnHarvestItems();
            }
            else if (cropDetails.hasAnimation)
            {
                anim.SetTrigger(playerTransform.position.x < transform.position.x ? "FallRight" : "FallLeft");
                StartCoroutine(SpawnHarvestAfterAnimation());
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
                    var dirX = transform.position.x > playerTransform.position.x ? 1 : -1;
                    var spawnPos =
                        new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
                            transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y),
                            0);
                    EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
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

    private IEnumerator SpawnHarvestAfterAnimation()
    {
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            yield return null;
        }

        SpawnHarvestItems();

        if (cropDetails.transferItemID > 0)
        {
            CreateTransferCrop();
        }
    }

    private void CreateTransferCrop()
    {
        tileDetails.seedItemID = cropDetails.transferItemID;
        tileDetails.daySinceLastHarvest = -1;
        tileDetails.growthDays = 0;

        EventHandler.CallRefreshCurrentMap();
    }
}