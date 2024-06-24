using UnityEngine;
using RPG.Utility;
using UnityEngine.InputSystem;
using RPG.Core;
using RPG.Quest;
using Ink.Parsed;
using System.Collections.Generic;

namespace RPG.Character
{
    public class NPCController : MonoBehaviour
    {
        public TextAsset inkJSON;
        private Canvas canvasComponent;
        private Reward rewardComponent;

        public QuestItemSO desiredQuestItems;
        public bool hasQuestItem = false;

        private void Start()
        {
            if (PlayerPrefs.HasKey("NPCItems"))
            {
                List<string> npcItems = PlayerPrefsUtility.GetString("NPCItems");

                npcItems.ForEach(CheckNPCQuestItem);
            }
        }
        private void Awake()
        {
            canvasComponent = GetComponentInChildren<Canvas>();
            rewardComponent = GetComponent<Reward>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.PLAYER_TAG))
                canvasComponent.enabled = true; 
        }
        private void OnTriggerExit(Collider other)
        { 
            if (other.CompareTag(Constants.PLAYER_TAG))
                canvasComponent.enabled = false;
        }

        public void HandleInteract(InputAction.CallbackContext context)
        {
            if (!context.performed || !canvasComponent.enabled) return;

            if (inkJSON == null)
                return;

            EventManager.RaiseInitiateDialogue(inkJSON, gameObject);
        }

        public bool CheckPlayerForQuestItem()
        {
            if (hasQuestItem) return true;

            Inventory inventoryCompoment = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG).GetComponent<Inventory>();

            hasQuestItem = inventoryCompoment.HasItem(desiredQuestItems);

            if (rewardComponent != null && hasQuestItem)
            {
                rewardComponent.SendReward();
            }

            return hasQuestItem;
        }

        private void CheckNPCQuestItem(string itemName)
        {
            if (itemName.Equals(desiredQuestItems.itemName))
            {
                hasQuestItem = true;
                EventManager.RaiseShowQuestItemIcon(false);
            }
        }
    }
}