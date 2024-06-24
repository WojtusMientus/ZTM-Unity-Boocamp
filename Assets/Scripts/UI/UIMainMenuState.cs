using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using RPG.Core;
using UnityEngine;
using System;

namespace RPG.UI
{
    public class UIMainMenuState : UIBaseState
    {
        private int sceneIndex;
        public UIMainMenuState(UIController ui) : base(ui) { }

        public override void EnterState()
        {
            if (PlayerPrefs.HasKey("SceneIndex"))
            {
                sceneIndex = PlayerPrefs.GetInt("SceneIndex");
                AddButton();
            }

            controller.mainMenuContainer.style.display = DisplayStyle.Flex;

            controller.buttons =  controller.mainMenuContainer.Query<Button>(null, "menu-button").ToList();

            controller.buttons[0].AddToClassList("active");
        }

        public override void SelectButton()
        {
            Button button = controller.buttons[controller.currentSelection];

            if (button.name == "StartButton")
            {
                PlayerPrefs.DeleteAll();
                controller.StartCoroutine(SceneTransition.Initiate(1));
            }
            else
                controller.StartCoroutine(SceneTransition.Initiate(sceneIndex));
        }

        private void AddButton()
        {
            Button continueButton = new Button();
            continueButton.AddToClassList("menu-button");
            continueButton.text = "Continue";

            VisualElement mainMenuButtons = controller.mainMenuContainer.Q<VisualElement>("Buttons");

            mainMenuButtons.Add(continueButton);
        }
    }
}