using System.Linq;
using UnityEngine;

namespace Farm.CropPlant
{
    public class CropManager : Singleton<CropManager>
    {
        public CropDataList_SO cropDataList_SO;

        private Transform cropParent;
        private Grid currentGrid;
        private Season currentSeason;

        #region Event functions

        private void OnEnable()
        {
            EventHandler.PlantSeedEvent += OnPlantSeedEvent;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
            EventHandler.GameDayEvent += OnGameDayEvent;
        }


        private void OnDisable()
        {
            EventHandler.PlantSeedEvent -= OnPlantSeedEvent;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
            EventHandler.GameDayEvent -= OnGameDayEvent;
        }

        #endregion

        #region EventHandler Functions

        private void OnPlantSeedEvent(int id, TileDetails tileDetails)
        {
            CropDetails currentCrop = GetCropDetails(id);
            if (currentCrop != null && SeasonAvailable(currentCrop) && tileDetails.seedItemID == -1)
            {
                tileDetails.seedItemID = id;
                tileDetails.growthDays = 0;
                DisplayCropPlant(tileDetails, currentCrop);
            }
            else if (tileDetails.seedItemID != -1)
            {
                DisplayCropPlant(tileDetails, currentCrop);
            }
        }

        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            cropParent = GameObject.FindWithTag("CropParent").transform;
        }

        private void OnGameDayEvent(int day, Season season)
        {
            currentSeason = season;
        }

        #endregion

        public CropDetails GetCropDetails(int id)
        {
            return cropDataList_SO.cropDetailsList.Find(c => c.seedItemID == id);
        }

        private bool SeasonAvailable(CropDetails crop)
        {
            return crop.seasons.Any(season => season == currentSeason);
        }

        private void DisplayCropPlant(TileDetails tileDetails, CropDetails cropDetails)
        {
            int growthStages = cropDetails.growthDays.Length;
            int currentStage = 0;
            int dayCounter = cropDetails.TotalGrowthDays;

            for (int i = growthStages - 1; i >= 0; i--)
            {
                if (tileDetails.growthDays >= dayCounter)
                {
                    currentStage = i;
                    break;
                }

                dayCounter -= cropDetails.growthDays[i];
            }

            GameObject cropPrefab = cropDetails.growthPrefabs[currentStage];
            Sprite cropSprite = cropDetails.growthSprites[currentStage];

            Vector3 pos = new Vector3(tileDetails.gridX + 0.5f, tileDetails.gridY + 0.5f, 0);
            GameObject cropInstance = Instantiate(cropPrefab, pos, Quaternion.identity, cropParent);

            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
            cropInstance.GetComponent<Crop>().cropDetails = cropDetails;
        }
    }
}