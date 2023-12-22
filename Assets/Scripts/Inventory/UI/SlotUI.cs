using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Farm.Inventory
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Image slotHighlight;
        [SerializeField] private Button button;

        public SlotType slotType;
        public bool isSelected;
        public int slotIndex;

        public ItemDetails itemDetails;
        public int itemAmount;

        #region Event Functions

        private void Start()
        {
            isSelected = false;
            if (itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }

        #endregion

        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            itemAmount = amount;

            slotImage.sprite = item.itemIcon;
            slotImage.enabled = true;
            amountText.text = amount.ToString();
            button.interactable = true;
        }
    }
}