using UnityEngine.UIElements;
using UnityEngine;
using Ink.Runtime;
using RPG.Utility;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using RPG.Character;

namespace RPG.UI
{
    public class UIDialogueState : UIBaseState
    {
        private VisualElement dialogueContainer;
        private Label dialogueText;
        private VisualElement nextButton;
        private VisualElement choicesGroup;
        private Story currentStory;
        private PlayerInput playerInputComponent;
        private NPCController npcControllerComponent;

        private bool hasChoices = false;

        public UIDialogueState(UIController ui) : base(ui) { }

        public override void EnterState()
        {
            dialogueContainer = controller.root.Q<VisualElement>("DialogueContainer");
            dialogueText = dialogueContainer.Q<Label>("dialgoue-text");
            nextButton = dialogueContainer.Q<VisualElement>("dialogue-next-button");
            choicesGroup = dialogueContainer.Q<VisualElement>("choices-group");


            dialogueContainer.style.display = DisplayStyle.Flex;

            playerInputComponent = GameObject.FindGameObjectWithTag(Constants.GAME_MANAGAER_TAG).GetComponent<PlayerInput>();
            playerInputComponent.SwitchCurrentActionMap(Constants.UI_ACTION_MAP);

            controller.canPause = false;
        }

        public override void SelectButton()
        {
            UpdateDialogue();
        }

        public void SetStory(TextAsset inkJSON, GameObject NPC)
        {
            currentStory = new Story(inkJSON.text);

            currentStory.BindExternalFunction("VerifyQuest", VerifyQuest);

            npcControllerComponent = NPC.GetComponent<NPCController>();

            if (npcControllerComponent.hasQuestItem)
            {
                currentStory.ChoosePathString("postCompletion");
            }

            UpdateDialogue();
        }

        public void UpdateDialogue()
        {
            if (hasChoices)
            {
                currentStory.ChooseChoiceIndex(controller.currentSelection);
            }

            if (!currentStory.canContinue)
            {
                ExitDialogue();
                return;
            }

            dialogueText.text = currentStory.Continue();

            hasChoices = currentStory.currentChoices.Count > 0;

            if (hasChoices)
            {
                HandleNewChoices(currentStory.currentChoices);
            }
            else
            {
                nextButton.style.display = DisplayStyle.Flex;
                choicesGroup.style.display = DisplayStyle.None;
            }
        }
        private void HandleNewChoices(List<Choice> choices)
        {
            nextButton.style.display = DisplayStyle.None;
            choicesGroup.style.display = DisplayStyle.Flex;

            choicesGroup.Clear();
            controller.buttons?.Clear();

            choices.ForEach(CreateNewChoiceButton);


            controller.buttons = choicesGroup.Query<Button>().ToList();
            controller.buttons[0].AddToClassList("active");
        }

        private void CreateNewChoiceButton(Choice choice)
        {
            Button choiceButton = new Button();

            choiceButton.AddToClassList("menu-button");
            choiceButton.text = choice.text;
            choiceButton.style.marginRight = 20;
            choiceButton.style.width = 250;

            choicesGroup.Add(choiceButton);

            controller.currentSelection = 0;
        }

        private void ExitDialogue()
        {
            dialogueContainer.style.display = DisplayStyle.None;
            playerInputComponent.SwitchCurrentActionMap(Constants.GAMEPLAY_ACTION_MAP);

            controller.canPause = true;
        }

        public void VerifyQuest()
        {
            currentStory.variablesState["questCompleted"] = npcControllerComponent.CheckPlayerForQuestItem();

        }

    }
}