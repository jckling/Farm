using Farm.CropPlant;
using Farm.Map;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed, item;
    private Sprite currentSprite;
    private Image cursorImage;
    private RectTransform cursorCanvas;

    private Camera mainCamera;
    private Grid currentGrid;

    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;

    private bool cursorEnable;
    private bool cursorPositionValid;

    private ItemDetails currentItem;
    private Transform playerTransform => FindObjectOfType<Player>().transform;

    #region Event Functions

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(normal);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (cursorCanvas == null)
        {
            return;
        }

        cursorImage.transform.position = Input.mousePosition;
        if (!InteractWithUI() && cursorEnable)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
            CheckPlayerInput();
        }
        else
        {
            SetCursorImage(normal);
        }
    }

    #endregion

    #region EventHandler Functions

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        if (!isSelected)
        {
            currentItem = null;
            currentSprite = normal;
            cursorEnable = false;
        }
        else
        {
            // WORKFLOW
            currentItem = itemDetails;
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.Furniture => tool,
                ItemType.HoeTool => tool,
                ItemType.ChopTool => tool,
                ItemType.BreakTool => tool,
                ItemType.ReapTool => tool,
                ItemType.WaterTool => tool,
                ItemType.CollectTool => tool,
                _ => normal
            };
            cursorEnable = true;
        }
    }

    private void OnBeforeSceneUnloadEvent()
    {
        cursorEnable = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
    }

    #endregion

    #region Cursor

    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }

    private void SetCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
    }

    private void SetCursorInvalid()
    {
        cursorPositionValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.5f);
    }

    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            -mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);

        var playerGridPos = currentGrid.WorldToCell(playerTransform.position);
        if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadius ||
            Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadius)
        {
            SetCursorInvalid();
            return;
        }

        TileDetails currentTile = GridMapManager.Instance.GetTileDetailsFromMousePosition(mouseGridPos);
        if (currentTile != null)
        {
            CropDetails currentCropDetails = CropManager.Instance.GetCropDetails(currentTile.seedItemID);
            Crop crop = GridMapManager.Instance.GetCropObject(mouseWorldPos);

            // WORKFLOW
            switch (currentItem.itemType)
            {
                case ItemType.Seed:
                    if (currentTile.daySinceDug > -1 && currentTile.seedItemID == -1)
                    {
                        SetCursorValid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }

                    break;
                case ItemType.Commodity:
                    if (currentTile.canDropItem && currentItem.canDropped)
                    {
                        SetCursorValid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }

                    break;
                case ItemType.Furniture:
                    break;
                case ItemType.HoeTool:
                    if (currentTile.canDig)
                    {
                        SetCursorValid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }

                    break;
                case ItemType.ChopTool:
                    if (crop != null)
                    {
                        if (crop.CanHarvest && crop.cropDetails.CheckToolValid(currentItem.itemID))
                        {
                            SetCursorValid();
                        }
                        else
                        {
                            SetCursorInvalid();
                        }
                    }
                    else
                    {
                        SetCursorInvalid();
                    }

                    break;
                case ItemType.BreakTool:
                    break;
                case ItemType.ReapTool:
                    break;
                case ItemType.WaterTool:
                    if (currentTile.daySinceDug > -1 && currentTile.daySinceWatered == -1)
                    {
                        SetCursorValid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }

                    break;
                case ItemType.CollectTool:
                    if (currentCropDetails != null)
                    {
                        if (currentCropDetails.CheckToolValid(currentItem.itemID))
                        {
                            if (currentTile.growthDays >= currentCropDetails.TotalGrowthDays)
                            {
                                SetCursorValid();
                            }
                            else
                            {
                                SetCursorInvalid();
                            }
                        }
                    }
                    else
                    {
                        SetCursorInvalid();
                    }

                    break;
            }
        }
        else
        {
            SetCursorInvalid();
        }
    }

    private bool InteractWithUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && cursorPositionValid)
        {
            EventHandler.CallMouseClickedEvent(mouseWorldPos, currentItem);
        }
    }

    #endregion
}