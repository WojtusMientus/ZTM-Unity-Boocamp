using System.Collections;
using System.Collections.Generic;
using RPG.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace RPG.UI
{
    public class UIUnpauseState : UIBaseState
    {
        public UIUnpauseState(UIController uiController) : base(uiController) { }

        public override void EnterState()
        {
            PlayerInput playerInputCmp = GameObject.FindGameObjectWithTag(Constants.GAME_MANAGAER_TAG).GetComponent<PlayerInput>();
            VisualElement pauseContainer = controller.root.Q<VisualElement>("PauseContainer");

            playerInputCmp.SwitchCurrentActionMap(Constants.GAMEPLAY_ACTION_MAP);

            pauseContainer.style.display = DisplayStyle.None;

            Time.timeScale = 1;
        }

        public override void SelectButton()
        {

        }
    }
}