using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Farm.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("种地瓦片切换信息")] public RuleTile digTile;
        public RuleTile waterTile;
        private Tilemap digTilemap;
        private Tilemap waterTilemap;

        [Header("地图信息")] public List<MapData_SO> mapDataList;
        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();
        private Grid currentGrid;

        private Season currentSeason;

        #region Event Functions

        private void OnEnable()
        {
            EventHandler.ExecuteAfterAnimation += OnExecuteAfterAnimation;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
            EventHandler.GameDayEvent += OnGameDayEvent;
            EventHandler.RefreshCurrentMap += RefreshMap;
        }

        private void OnDisable()
        {
            EventHandler.ExecuteAfterAnimation -= OnExecuteAfterAnimation;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
            EventHandler.GameDayEvent -= OnGameDayEvent;
            EventHandler.RefreshCurrentMap -= RefreshMap;
        }

        private void Start()
        {
            foreach (var mapData in mapDataList)
            {
                InitTileDetailsDict(mapData);
            }
        }

        #endregion

        private void InitTileDetailsDict(MapData_SO mapData)
        {
            foreach (TileProperty tileProperty in mapData.tileProperties)
            {
                TileDetails tileDetails = new TileDetails
                {
                    gridX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y
                };

                string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + mapData.sceneName;
                if (GetTileDetails(key) != null)
                {
                    tileDetails = GetTileDetails(key);
                }

                switch (tileProperty.gridType)
                {
                    case GridType.Diggable:
                        tileDetails.canDig = tileProperty.boolTypeValue;
                        break;
                    case GridType.DropItem:
                        tileDetails.canDropItem = tileProperty.boolTypeValue;
                        break;
                    case GridType.PlaceFurniture:
                        tileDetails.canPlaceFurniture = tileProperty.boolTypeValue;
                        break;
                    case GridType.NPCObstacles:
                        tileDetails.isNPCObstacles = tileProperty.boolTypeValue;
                        break;
                }

                if (GetTileDetails(key) != null)
                {
                    tileDetailsDict[key] = tileDetails;
                }
                else
                {
                    tileDetailsDict.Add(key, tileDetails);
                }
            }
        }

        private TileDetails GetTileDetails(string key)
        {
            return tileDetailsDict.ContainsKey(key) ? tileDetailsDict[key] : null;
        }

        public TileDetails GetTileDetailsFromMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
            return GetTileDetails(key);
        }

        #region EventHandler Functions

        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            digTilemap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
            waterTilemap = GameObject.FindWithTag("Water").GetComponent<Tilemap>();

            RefreshMap();
        }

        private void OnExecuteAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = GetTileDetailsFromMousePosition(mouseGridPos);

            if (currentTile != null)
            {
                Crop currentCrop = GetCropObject(mouseWorldPos);

                // WORKFLOW
                switch (itemDetails.itemType)
                {
                    case ItemType.Seed:
                        EventHandler.CallPlantSeedEvent(itemDetails.itemID, currentTile);
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos, itemDetails.itemType);
                        break;
                    case ItemType.Commodity:
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos, itemDetails.itemType);
                        break;
                    case ItemType.Furniture:
                        break;
                    case ItemType.HoeTool:
                        SetDigGround(currentTile);
                        currentTile.daySinceDug = 0;
                        currentTile.canDig = false;
                        currentTile.canDropItem = false;
                        // TODO: 音效
                        break;
                    case ItemType.ChopTool:
                        currentCrop?.ProcessToolAction(itemDetails, currentCrop.tileDetails);
                        break;
                    case ItemType.BreakTool:
                        break;
                    case ItemType.ReapTool:
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.daySinceWatered = 0;
                        // 音效
                        break;
                    case ItemType.CollectTool:
                        currentCrop.ProcessToolAction(itemDetails, currentTile);
                        break;
                }

                UpdateTileDetails(currentTile);
            }
        }

        private void OnGameDayEvent(int day, Season season)
        {
            currentSeason = season;

            foreach (var (key, tileDetails) in tileDetailsDict)
            {
                if (tileDetails.daySinceWatered > -1)
                {
                    tileDetails.daySinceWatered = -1;
                }

                if (tileDetails.daySinceDug > -1)
                {
                    tileDetails.daySinceDug++;
                }

                if (tileDetails.daySinceDug == 5 && tileDetails.seedItemID == -1)
                {
                    tileDetails.daySinceDug = -1;
                    tileDetails.canDig = true;
                    tileDetails.growthDays = -1;
                }

                if (tileDetails.seedItemID != -1)
                {
                    tileDetails.growthDays++;
                }
            }

            RefreshMap();
        }

        #endregion

        private void SetDigGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if (digTilemap != null)
            {
                digTilemap.SetTile(pos, digTile);
            }
        }

        private void SetWaterGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if (waterTilemap != null)
            {
                waterTilemap.SetTile(pos, waterTile);
            }
        }

        private void UpdateTileDetails(TileDetails tileDetails)
        {
            string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + SceneManager.GetActiveScene().name;
            if (tileDetailsDict.ContainsKey(key))
            {
                tileDetailsDict[key] = tileDetails;
            }
        }

        private void DisplayMap(string sceneName)
        {
            foreach (var (key, tileDetails) in tileDetailsDict)
            {
                if (key.Contains(sceneName))
                {
                    if (tileDetails.daySinceDug > -1)
                    {
                        SetDigGround(tileDetails);
                    }

                    if (tileDetails.daySinceWatered > -1)
                    {
                        SetWaterGround(tileDetails);
                    }

                    if (tileDetails.seedItemID > -1)
                    {
                        EventHandler.CallPlantSeedEvent(tileDetails.seedItemID, tileDetails);
                    }
                }
            }
        }

        private void RefreshMap()
        {
            if (digTilemap != null)
            {
                digTilemap.ClearAllTiles();
            }

            if (waterTilemap != null)
            {
                waterTilemap.ClearAllTiles();
            }

            foreach (var crop in FindObjectsOfType<Crop>())
            {
                Destroy(crop.gameObject);
            }

            DisplayMap(SceneManager.GetActiveScene().name);
        }

        public Crop GetCropObject(Vector3 mouseWorldPos)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mouseWorldPos);
            Crop currentCrop = null;
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<Crop>())
                {
                    currentCrop = collider.GetComponent<Crop>();
                }
            }

            return currentCrop;
        }
    }
}