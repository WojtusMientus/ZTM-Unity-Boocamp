using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Quest
{
    public class Inventory : MonoBehaviour
    {
        public List<QuestItemSO> items = new List<QuestItemSO>();

        private void OnEnable()
        {
            EventManager.OnTreasureChestUnlocked += HandleTreasureChestUnlocked;
        }

        private void OnDisable()
        {
            EventManager.OnTreasureChestUnlocked -= HandleTreasureChestUnlocked;
        }
        public void HandleTreasureChestUnlocked(QuestItemSO newItem, bool showUI)
        {
            items.Add(newItem);
        }

        public bool HasItem(QuestItemSO desiredItem)
        {
            return items.Contains(desiredItem);
        }

    }
}