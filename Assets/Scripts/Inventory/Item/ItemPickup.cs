using UnityEngine;

namespace Farm.Inventory
{
    public class ItemPickup : MonoBehaviour
    {
        #region Event Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                if (item.itemDetails.canPickedup)
                {
                    InventoryManager.Instance.AddItem(item, true);
                }
            }
        }

        #endregion
    }
}