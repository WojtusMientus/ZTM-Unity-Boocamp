using System.Collections;
using System.Collections.Generic;
using RPG.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace RPG.UI
{
    public class UIPauseState : UIBaseState
    {
        public UIPauseState(UIController uiController) : base(uiController) { }

        public override void EnterState()
        {
            PlayerInput playerInputCmp = GameObject.FindGameObjectWithTag(Constants.GAME_MANAGAER_TAG).GetComponent<PlayerInput>();
            VisualElement pauseContainer = controller.root.Q<VisualElement>("PauseContainer");

            playerInputCmp.SwitchCurrentActionMap(Constants.UI_ACTION_MAP);

            pauseContainer.style.display = DisplayStyle.Flex;

            Time.timeScale = 0;
        }

        public override void SelectButton()
        {

        }
    }
}