using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using RPG.Utility;
using RPG.Core;

namespace RPG.UI
{
    public class UIQuestItemState : UIBaseState
    {
        private VisualElement questItemContainer;
        private Label questItemText;
        private PlayerInput playerInputComponent;

        public UIQuestItemState(UIController ui) : base(ui) { }

        public override void EnterState()
        {
            playerInputComponent = GameObject.FindGameObjectWithTag(Constants.GAME_MANAGAER_TAG).GetComponent<PlayerInput>();

            playerInputComponent.SwitchCurrentActionMap(Constants.UI_ACTION_MAP);

            questItemContainer = controller.root.Q<VisualElement>("QuestItemContainer");
            questItemText = questItemContainer.Q<Label>("quest-item-label");

            questItemContainer.style.display = DisplayStyle.Flex;

            EventManager.RaiseToggleUI(true);

            controller.canPause = false;
        }

        public override void SelectButton()
        {
            questItemContainer.style.display = DisplayStyle.None;
            playerInputComponent.SwitchCurrentActionMap(Constants.GAMEPLAY_ACTION_MAP);

            EventManager.RaiseToggleUI(false);

            controller.canPause = true;
        }

        public void SetQuestItemLabel(string name)
        {
            questItemText.text = name;
        }

    }
}