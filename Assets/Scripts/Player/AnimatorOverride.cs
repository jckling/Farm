using System.Collections;
using System.Collections.Generic;
using Farm.Inventory;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] animators;
    public SpriteRenderer holdItem;

    public List<AnimatorType> animatorTypes;
    private Dictionary<string, Animator> animatorNameDict = new Dictionary<string, Animator>();

    #region Event Functions

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
    }

    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        foreach (var anim in animators)
        {
            animatorNameDict.Add(anim.name, anim);
        }
    }

    #endregion

    private void SwitchAnimator(PartType partType)
    {
        foreach (var item in animatorTypes)
        {
            if (item.partType == partType)
            {
                animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }

    #region EventHandler Functions

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        // WORKFLOW
        PartType currentType = itemDetails.itemType switch
        {
            ItemType.Seed => PartType.Carry,
            ItemType.Commodity => PartType.Carry,
            ItemType.HoeTool => PartType.Hoe,
            ItemType.ChopTool => PartType.Chop,
            ItemType.BreakTool => PartType.Break,
            ItemType.ReapTool => PartType.Reap,
            ItemType.WaterTool => PartType.Water,
            ItemType.CollectTool => PartType.Collect,
            _ => PartType.None
        };

        if (isSelected == false)
        {
            currentType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            if (currentType == PartType.Carry)
            {
                holdItem.enabled = true;
                holdItem.sprite = itemDetails.itemOnWorldSprite;
            }
            else
            {
                holdItem.enabled = false;
            }
        }

        SwitchAnimator(currentType);
    }

    private void OnBeforeSceneUnloadEvent()
    {
        holdItem.enabled = false;
        SwitchAnimator(PartType.None);
    }

    private void OnHarvestAtPlayerPosition(int itemID)
    {
        Sprite itemSprite = InventoryManager.Instance.GetItemDetails(itemID).itemOnWorldSprite;
        if (holdItem.enabled == false)
        {
            StartCoroutine(ShowItem(itemSprite));
        }
    }

    #endregion

    private IEnumerator ShowItem(Sprite itemSprite)
    {
        holdItem.sprite = itemSprite;
        holdItem.enabled = true;
        yield return new WaitForSeconds(.8f);
        holdItem.enabled = false;
    }
}