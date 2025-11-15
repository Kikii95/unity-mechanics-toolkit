using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityMechanicsToolkit.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public Item item;
        public int quantity;

        public InventorySlot(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public bool IsEmpty => item == null || quantity <= 0;

        public bool CanAddItem(Item newItem)
        {
            return IsEmpty || (item == newItem && quantity < item.maxStackSize);
        }

        public void Clear()
        {
            item = null;
            quantity = 0;
        }
    }

    public class InventorySystem : MonoBehaviour
    {
        [Header("Inventory Settings")]
        [SerializeField] private int inventorySize = 20;

        private List<InventorySlot> slots;

        [Header("Events")]
        public UnityEvent<Item, int> onItemAdded;
        public UnityEvent<Item, int> onItemRemoved;
        public UnityEvent onInventoryChanged;

        public int InventorySize => inventorySize;
        public List<InventorySlot> Slots => slots;

        private void Awake()
        {
            InitializeInventory();
        }

        public void InitializeInventory()
        {
            slots = new List<InventorySlot>();
            for (int i = 0; i < inventorySize; i++)
            {
                slots.Add(new InventorySlot(null, 0));
            }
        }

        public bool AddItem(Item item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
                return false;

            // Try to stack with existing items
            foreach (InventorySlot slot in slots)
            {
                if (slot.item == item && slot.quantity < item.maxStackSize)
                {
                    int spaceAvailable = item.maxStackSize - slot.quantity;
                    int amountToAdd = Mathf.Min(quantity, spaceAvailable);

                    slot.quantity += amountToAdd;
                    quantity -= amountToAdd;

                    onItemAdded?.Invoke(item, amountToAdd);
                    onInventoryChanged?.Invoke();

                    if (quantity <= 0)
                        return true;
                }
            }

            // Find empty slots for remaining items
            while (quantity > 0)
            {
                InventorySlot emptySlot = FindEmptySlot();
                if (emptySlot == null)
                {
                    Debug.Log("Inventory is full!");
                    return false;
                }

                int amountToAdd = Mathf.Min(quantity, item.maxStackSize);
                emptySlot.item = item;
                emptySlot.quantity = amountToAdd;
                quantity -= amountToAdd;

                onItemAdded?.Invoke(item, amountToAdd);
                onInventoryChanged?.Invoke();
            }

            return true;
        }

        public bool RemoveItem(Item item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
                return false;

            int totalQuantity = GetItemQuantity(item);
            if (totalQuantity < quantity)
                return false;

            int remainingToRemove = quantity;

            for (int i = slots.Count - 1; i >= 0 && remainingToRemove > 0; i--)
            {
                if (slots[i].item == item)
                {
                    int amountToRemove = Mathf.Min(remainingToRemove, slots[i].quantity);
                    slots[i].quantity -= amountToRemove;
                    remainingToRemove -= amountToRemove;

                    if (slots[i].quantity <= 0)
                    {
                        slots[i].Clear();
                    }
                }
            }

            onItemRemoved?.Invoke(item, quantity);
            onInventoryChanged?.Invoke();
            return true;
        }

        public int GetItemQuantity(Item item)
        {
            int total = 0;
            foreach (InventorySlot slot in slots)
            {
                if (slot.item == item)
                {
                    total += slot.quantity;
                }
            }
            return total;
        }

        public bool HasItem(Item item, int quantity = 1)
        {
            return GetItemQuantity(item) >= quantity;
        }

        public InventorySlot FindEmptySlot()
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.IsEmpty)
                    return slot;
            }
            return null;
        }

        public void ClearInventory()
        {
            foreach (InventorySlot slot in slots)
            {
                slot.Clear();
            }
            onInventoryChanged?.Invoke();
        }

        public InventorySlot GetSlotAt(int index)
        {
            if (index >= 0 && index < slots.Count)
                return slots[index];
            return null;
        }
    }
}
