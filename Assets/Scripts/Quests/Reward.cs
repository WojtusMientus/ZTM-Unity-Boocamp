using RPG.Core;
using UnityEngine;

namespace RPG.Quest
{
    public class Reward : MonoBehaviour
    {
        [SerializeField] private RewardSO reward;
        private bool rewardTaken = false;

        public void SendReward()
        {
            if (rewardTaken) return;

            rewardTaken = true;
            EventManager.RaiseReward(reward);
            EventManager.RaiseShowQuestItemIcon(false);

            AudioSource audioComponent = GetComponent<AudioSource>();

            if (audioComponent.clip == null) return;

            audioComponent.Play();
        }


    }
}