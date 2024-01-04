using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed, item;
    private Sprite currentSprite;
    private Image cursorImage;
    private RectTransform cursorCanvas;

    #region Event Functions

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
    }

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(normal);
    }

    private void Update()
    {
        if (cursorCanvas == null)
        {
            return;
        }

        cursorImage.transform.position = Input.mousePosition;
        SetCursorImage(!InteractWithUI() ? currentSprite : normal);
    }

    #endregion

    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        if (!isSelected)
        {
            currentSprite = normal;
        }
        else
        {
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.ChopTool => tool,
                ItemType.HoeTool => tool,
                ItemType.WaterTool => tool,
                ItemType.BreakTool => tool,
                ItemType.ReapTool => tool,
                ItemType.Furniture => tool,
                _ => normal
            };
        }
    }

    private bool InteractWithUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}