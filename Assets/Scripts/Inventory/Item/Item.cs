using UnityEngine;

namespace Farm.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;

        private BoxCollider2D collider2D;
        private SpriteRenderer spriteRenderer;
        public ItemDetails itemDetails;

        #region Event Functions

        private void Awake()
        {
            collider2D = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            if (itemID != 0)
            {
                Init(itemID);
            }
        }

        #endregion

        public void Init(int id)
        {
            itemID = id;
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);
            if (itemDetails != null)
            {
                var sprite = spriteRenderer.sprite;
                sprite = itemDetails.itemOnWorldSprite != null
                    ? itemDetails.itemOnWorldSprite
                    : itemDetails.itemIcon;
                spriteRenderer.sprite = sprite;

                collider2D.size = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
                collider2D.offset = new Vector2(0, sprite.bounds.center.y);
            }
        }
    }
}