using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using RPG.Core;
using System;
using RPG.Quest;


namespace RPG.UI
{
    public class UIController : MonoBehaviour
    {
        private UIDocument uiDocumentCmp;
        public VisualElement root;
        public List<Button> buttons = new List<Button>();
        public VisualElement mainMenuContainer;
        public VisualElement playerInfoContainer;
        public Label healthLabel;
        public Label potionLabel;
        public VisualElement questItemIcon;

        public UIBaseState currentState;
        public UIMainMenuState mainMenuState;
        public UIDialogueState dialogueState;
        public UIQuestItemState questItemState;
        public UIVictoryState victoryState;
        public UIGameOverState gameOverState;
        public UIPauseState pauseState;
        public UIUnpauseState unpauseState;

        public int currentSelection = 0;
        public bool canPause = true;

        [SerializeField] private AudioClip gameOverAudio;
        [SerializeField] private AudioClip victoryAudio;
        private AudioSource audioComponent;

        private void Awake()
        {
            mainMenuState = new UIMainMenuState(this);
            dialogueState = new UIDialogueState(this);
            questItemState = new UIQuestItemState(this);
            victoryState = new UIVictoryState(this);
            gameOverState = new UIGameOverState(this);
            pauseState = new UIPauseState(this);
            unpauseState = new UIUnpauseState(this);

            audioComponent = GetComponent<AudioSource>();

            uiDocumentCmp = GetComponent<UIDocument>();
            root = uiDocumentCmp.rootVisualElement;

            mainMenuContainer = root.Q<VisualElement>("MainMenuContainer");
            playerInfoContainer = root.Q<VisualElement>("PlayerInfoContainer");

            healthLabel = playerInfoContainer.Q<Label>("health-label");
            potionLabel = playerInfoContainer.Q<Label>("potion-label");
            questItemIcon = playerInfoContainer.Q<VisualElement>("QuestItemIcon");
        }

        private void OnEnable()
        {
            EventManager.OnChangePlayerHealth += HandleChangePlayerHealth;
            EventManager.OnChangePotionCount += HandleChangePotionCount;
            EventManager.OnInitiateDialogue += HandleInitiateDialogue;
            EventManager.OnTreasureChestUnlocked += HanldeTreasureChestUnlocked;
            EventManager.OnLoadQuestItemIcon += HandleQuestItemIcon;
            EventManager.OnVictory += HandleVictory;
            EventManager.OnGameOver += HandleGameOver;
        }


        private void OnDisable()
        {
            EventManager.OnChangePlayerHealth -= HandleChangePlayerHealth;
            EventManager.OnChangePotionCount -= HandleChangePotionCount;
            EventManager.OnInitiateDialogue -= HandleInitiateDialogue;
            EventManager.OnTreasureChestUnlocked -= HanldeTreasureChestUnlocked;
            EventManager.OnLoadQuestItemIcon -= HandleQuestItemIcon;
            EventManager.OnVictory -= HandleVictory;
            EventManager.OnGameOver -= HandleGameOver;
        }

        private void Start()
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (sceneIndex == 0)
            {
                currentState = mainMenuState;
                currentState.EnterState();
            }

            else
                playerInfoContainer.style.display = DisplayStyle.Flex;
        }

        public void HandleInteract(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            currentState.SelectButton();
        }

        public void HandleNavigate(InputAction.CallbackContext context)
        {
            if (!context.performed || buttons.Count == 0) 
                return;

            buttons[currentSelection].RemoveFromClassList("active");

            Vector2 input = context.ReadValue<Vector2>();
            currentSelection += input.x > 0 ? 1: -1;
            currentSelection = Mathf.Clamp(currentSelection, 0, buttons.Count - 1);


            buttons[currentSelection].AddToClassList("active");
        }

        private void HandleChangePlayerHealth(float newHealthPoints)
        {
            healthLabel.text = newHealthPoints.ToString();
        }

        private void HandleChangePotionCount(int newPotionCount)
        {
            potionLabel.text = newPotionCount.ToString();
        }

        private void HandleInitiateDialogue(TextAsset inkJSON, GameObject NPC)
        {
            currentState = dialogueState;
            currentState.EnterState();

            (currentState as UIDialogueState).SetStory(inkJSON, NPC);
        }

        private void HanldeTreasureChestUnlocked(QuestItemSO item, bool showUI)
        {
            questItemIcon.style.display = DisplayStyle.Flex;

            if (!showUI) return;

            currentState = questItemState;
            currentState.EnterState();
            (currentState as UIQuestItemState).SetQuestItemLabel(item.name);
        }

        private void HandleQuestItemIcon(bool showUI)
        {
            if (!showUI)
                questItemIcon.style.display = DisplayStyle.None;
        }

        private void HandleVictory()
        {
            currentState = victoryState;

            audioComponent.clip = victoryAudio;
            audioComponent.Play();

            currentState.EnterState();
        }

        private void HandleGameOver()
        {
            currentState = gameOverState;

            audioComponent.clip = gameOverAudio;
            audioComponent.Play();

            currentState.EnterState();
        }

        public void HandlePause(InputAction.CallbackContext context)
        {
            if (!context.performed || !canPause) return;

            currentState = currentState == pauseState ? unpauseState : pauseState;

            currentState.EnterState();
        }


    }
}



