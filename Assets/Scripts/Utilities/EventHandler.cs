using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;

    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(location, list);
    }

    public static event Action<int, Vector3> InstantiateItemInScene;

    public static void CallInstantiateItemInScene(int id, Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(id, pos);
    }

    public static event Action<int, Vector3, ItemType> DropItemEvent;

    public static void CallDropItemEvent(int id, Vector3 pos, ItemType itemType)
    {
        DropItemEvent?.Invoke(id, pos, itemType);
    }

    public static event Action<ItemDetails, bool> ItemSelectedEvent;

    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }

    public static event Action<int, int> GameMinuteEvent;

    public static void CallGameMinuteEvent(int minute, int hour)
    {
        GameMinuteEvent?.Invoke(minute, hour);
    }

    public static event Action<int, Season> GameDayEvent;

    public static void CallGameDayEvent(int day, Season season)
    {
        GameDayEvent?.Invoke(day, season);
    }

    public static event Action<int, int, int, int, Season> GameDateEvent;

    public static void CallGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        GameDateEvent?.Invoke(hour, day, month, year, season);
    }

    public static event Action<string, Vector3> TransitionEvent;

    public static void CallTransitionEvent(string sceneName, Vector3 pos)
    {
        TransitionEvent?.Invoke(sceneName, pos);
    }

    public static event Action BeforeSceneUnloadEvent;

    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneLoadedEvent;

    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;

    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }

    public static event Action<Vector3, ItemDetails> MouseClickedEvent;

    public static void CallMouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
    {
        MouseClickedEvent?.Invoke(pos, itemDetails);
    }

    public static event Action<Vector3, ItemDetails> ExecuteAfterAnimation;

    public static void CallExecuteAfterAnimationEvent(Vector3 pos, ItemDetails itemDetails)
    {
        ExecuteAfterAnimation?.Invoke(pos, itemDetails);
    }

    public static event Action<int, TileDetails> PlantSeedEvent;

    public static void CallPlantSeedEvent(int id, TileDetails tileDetails)
    {
        PlantSeedEvent?.Invoke(id, tileDetails);
    }

    public static event Action<int> HarvestAtPlayerPosition;

    public static void CallHarvestAtPlayerPosition(int id)
    {
        HarvestAtPlayerPosition?.Invoke(id);
    }

    public static event Action RefreshCurrentMap;

    public static void CallRefreshCurrentMap()
    {
        RefreshCurrentMap?.Invoke();
    }

    public static event Action<ParticleEffectType, Vector3> ParticleEffectEvent;

    public static void CallParticleEffectEvent(ParticleEffectType effectType, Vector3 pos)
    {
        ParticleEffectEvent?.Invoke(effectType, pos);
    }
}