
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Utility;
using RPG.Core;
using System.Collections.Generic;

namespace RPG.Quest
{
    public class TreasureChest : MonoBehaviour
    {
        [SerializeField] private Animator animatorCmp;
        [SerializeField] private QuestItemSO questItem;

        private bool isInteractable = false;
        private bool hasBeenOpened = false;

        private void Start()
        {
            if (PlayerPrefs.HasKey("PlayerItems"))
            {
                List<string> playerItems = PlayerPrefsUtility.GetString("PlayerItems");

                playerItems.ForEach(CheckItem);
            }
        }
        private void OnTriggerEnter()
        {
            if (hasBeenOpened) return;

            animatorCmp.SetBool(Constants.IS_SHAKING_ANIMATOR, true);
            isInteractable = true;
        }

        private void OnTriggerExit()
        {
            animatorCmp.SetBool(Constants.IS_SHAKING_ANIMATOR, false);
            isInteractable = false;
        }


        public void HandleInteract(InputAction.CallbackContext context)
        {
            if (!isInteractable || hasBeenOpened || !context.performed)
                return;

            EventManager.RaiseTreasureChestUnlocksed(questItem, true);
            animatorCmp.SetBool(Constants.IS_SHAKING_ANIMATOR, false);
            hasBeenOpened = true;

            AudioSource audioComponent = GetComponent<AudioSource>();

            if (audioComponent.clip == null) return;
            
            audioComponent.Play();
        }

        private void CheckItem(string itemName)
        {
            if (itemName != questItem.name) return;

            hasBeenOpened = true;
            animatorCmp.SetBool(Constants.IS_SHAKING_ANIMATOR, false);
            EventManager.RaiseTreasureChestUnlocksed(questItem, false);
        }
    }

}


