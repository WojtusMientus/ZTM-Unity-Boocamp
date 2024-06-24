using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace RPG.UI
{
    public class UIVictoryState : UIBaseState
    {   
        public UIVictoryState(UIController uiController) : base(uiController) { }

        public override void EnterState()
        {
            PlayerInput playerInputCmp = GameObject.FindGameObjectWithTag(Constants.GAME_MANAGAER_TAG).GetComponent<PlayerInput>();

            VisualElement gameOverContainer = controller.root.Q<VisualElement>("VictoryContainer");

            playerInputCmp.SwitchCurrentActionMap(Constants.UI_ACTION_MAP);

            gameOverContainer.style.display = DisplayStyle.Flex;

            controller.canPause = false;
        }

        public override void SelectButton()
        {
            PlayerPrefs.DeleteAll();
            controller.StartCoroutine(SceneTransition.Initiate(0));
        }


    }
}