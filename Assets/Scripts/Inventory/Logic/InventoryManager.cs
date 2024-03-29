using UnityEngine;

namespace Farm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")] public ItemDataList_SO itemDataList_SO;
        [Header("背包数据")] public InventoryBag_SO playerBag_SO;

        #region Event Functions

        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
        }

        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
        }

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag_SO.itemList);
        }

        #endregion

        #region EventHandler Functions

        private void OnDropItemEvent(int id, Vector3 pos, ItemType itemType)
        {
            RemoveItem(id, 1);
        }

        private void OnHarvestAtPlayerPosition(int itemID)
        {
            var index = GetItemIndexInBag(itemID);
            AddItemAtIndex(itemID, index, 1);

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag_SO.itemList);
        }

        #endregion

        public ItemDetails GetItemDetails(int id)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == id);
        }

        public void AddItem(Item item, bool destroy)
        {
            var index = GetItemIndexInBag(item.itemID);
            AddItemAtIndex(item.itemID, index, 1);
            if (destroy)
            {
                Destroy(item.gameObject);
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag_SO.itemList);
        }

        private int GetItemIndexInBag(int id)
        {
            for (int i = 0; i < playerBag_SO.itemList.Count; i++)
            {
                if (playerBag_SO.itemList[i].itemID == id)
                {
                    return i;
                }
            }

            return -1;
        }

        private bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag_SO.itemList.Count; i++)
            {
                if (playerBag_SO.itemList[i].itemID == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddItemAtIndex(int id, int index, int amount)
        {
            if (index == -1)
            {
                if (!CheckBagCapacity())
                {
                    return;
                }

                var item = new InventoryItem { itemID = id, itemAmount = amount };
                for (int i = 0; i < playerBag_SO.itemList.Count; i++)
                {
                    if (playerBag_SO.itemList[i].itemID == 0)
                    {
                        playerBag_SO.itemList[i] = item;
                        break;
                    }
                }
            }
            else
            {
                int currentAmount = playerBag_SO.itemList[index].itemAmount + amount;
                playerBag_SO.itemList[index] = new InventoryItem { itemID = id, itemAmount = currentAmount };
            }
        }

        public void SwapItem(int fromIndex, int toIndex)
        {
            InventoryItem currentItem = playerBag_SO.itemList[fromIndex];
            InventoryItem targetItem = playerBag_SO.itemList[toIndex];

            if (targetItem.itemID != 0)
            {
                playerBag_SO.itemList[fromIndex] = targetItem;
                playerBag_SO.itemList[toIndex] = currentItem;
            }
            else
            {
                playerBag_SO.itemList[toIndex] = currentItem;
                playerBag_SO.itemList[fromIndex] = new InventoryItem();
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag_SO.itemList);
        }

        public void RemoveItem(int id, int removeAmount)
        {
            var index = GetItemIndexInBag(id);
            if (playerBag_SO.itemList[index].itemAmount > removeAmount)
            {
                var amount = playerBag_SO.itemList[index].itemAmount - removeAmount;
                var item = new InventoryItem
                {
                    itemID = id,
                    itemAmount = amount
                };

                playerBag_SO.itemList[index] = item;
            }
            else if (playerBag_SO.itemList[index].itemAmount == removeAmount)
            {
                var item = new InventoryItem();
                playerBag_SO.itemList[index] = item;
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag_SO.itemList);
        }
    }
}