using UnityEngine;
using UnityEngine.Events;
using RPG.Quest;
using RPG.Character;

namespace RPG.Core
{
    public static class EventManager
    {
        public static event UnityAction<float> OnChangePlayerHealth;
        public static event UnityAction<int> OnChangePotionCount;
        public static event UnityAction<TextAsset, GameObject> OnInitiateDialogue;
        public static event UnityAction<QuestItemSO, bool> OnTreasureChestUnlocked;
        public static event UnityAction<bool> OnToggleUI;
        public static event UnityAction<RewardSO> OnReward;
        public static event UnityAction<Collider, int> OnPortalEnter;
        public static event UnityAction<bool> OnLoadQuestItemIcon;
        public static event UnityAction<bool> OnCutsceneUpdated;
        public static event UnityAction OnVictory;
        public static event UnityAction OnGameOver;

        public static void RaiseChangePlayerHealth(float newHealthPoints)
        {
            OnChangePlayerHealth?.Invoke(newHealthPoints);
        }

        public static void ChangePotionCount(int newPotionCount)
        {
            OnChangePotionCount?.Invoke(newPotionCount);
        }

        public static void RaiseInitiateDialogue(TextAsset inkJSON, GameObject NPC)
        {
            OnInitiateDialogue?.Invoke(inkJSON, NPC);
        }

        public static void RaiseTreasureChestUnlocksed(QuestItemSO item, bool showUI)
        {
            OnTreasureChestUnlocked?.Invoke(item, showUI);
        }

        public static void RaiseToggleUI(bool isOpened)
        {
            OnToggleUI?.Invoke(isOpened);
        }

        public static void RaiseReward(RewardSO reward)
        {
            OnReward?.Invoke(reward);
        }

        public static void RaisePortalEnter(Collider player, int nextSceneIndex)
        {
            OnPortalEnter?.Invoke(player, nextSceneIndex);
        }

        public static void RaiseShowQuestItemIcon(bool showUI)
        {
            OnLoadQuestItemIcon?.Invoke(showUI);
        }

        public static void RaiseCutsceneUpdated(bool isEnabled)
        {
            OnCutsceneUpdated?.Invoke(isEnabled);
        }

        public static void RaiseVictory()
        {
            OnVictory.Invoke();
        }

        public static void RaiseGameOver()
        {
            OnGameOver.Invoke();
        }

    }
}