using UnityEngine;

namespace UnityMechanicsToolkit.Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Unity Mechanics Toolkit/Inventory/Item")]
    public class Item : ScriptableObject
    {
        [Header("Item Info")]
        public string itemName = "New Item";
        [TextArea(3, 5)]
        public string description = "Item description";
        public Sprite icon;

        [Header("Item Properties")]
        public int maxStackSize = 1;
        public bool isConsumable = false;
        public bool isDroppable = true;
        public bool isSellable = true;

        [Header("Values")]
        public int buyPrice = 0;
        public int sellPrice = 0;

        public virtual void Use(GameObject user)
        {
            Debug.Log($"Used {itemName}");
        }

        public virtual Item Clone()
        {
            return Instantiate(this);
        }
    }
}
