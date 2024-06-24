using System;
using UnityEngine;
using UnityEngine.Events;
using RPG.Utility;
using RPG.Core;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RPG.Character
{
    public class Health : MonoBehaviour
    {
        public event UnityAction OnStartDefeated = () => { };

        [NonSerialized] public float healthPoints = 0f;
        [NonSerialized] public int potionCount = 1;
        [SerializeField] private float healAmount = 15f;

        private BubbleEvent bubbleEventComponent;
        private Animator animatorComponent;
        [NonSerialized] public Slider sliderComponent;

        private bool isDefeated = false;

        private void Awake()
        {
            animatorComponent = GetComponentInChildren<Animator>();
            bubbleEventComponent = GetComponentInChildren<BubbleEvent>();
            sliderComponent = GetComponentInChildren<Slider>();
        }

        private void OnEnable()
        {
            bubbleEventComponent.OnBubbleCompleteDefeat += HandleBubbleCompleteDefeat;
        }

        private void OnDisable()
        {
            bubbleEventComponent.OnBubbleCompleteDefeat -= HandleBubbleCompleteDefeat;
        }

        private void Start()
        {
            if (CompareTag(Constants.PLAYER_TAG))
                EventManager.ChangePotionCount(potionCount);
        }

        public void TakeDamage(float damageAmount)
        {
            healthPoints = Mathf.Max(healthPoints - damageAmount, 0);

            if (CompareTag(Constants.PLAYER_TAG))
                EventManager.RaiseChangePlayerHealth(healthPoints);

            if (sliderComponent != null)
                sliderComponent.value = healthPoints;

            if (healthPoints == 0)
                Defeated();
        }

        private void Defeated()
        {
            if (isDefeated) return;

            if (CompareTag(Constants.ENEMY_TAG))
                OnStartDefeated.Invoke();

            animatorComponent.SetTrigger(Constants.DEFEATED_ANIMATOR);
            isDefeated = true;
        }

        private void HandleBubbleCompleteDefeat()
        {
            if (CompareTag(Constants.PLAYER_TAG))
                EventManager.RaiseGameOver();

            if (CompareTag(Constants.BOSS_TAG))
                EventManager.RaiseVictory();

            Destroy(gameObject);
        }

        public void HandleHeal(InputAction.CallbackContext context)
        {
            if (!context.performed || potionCount == 0) return;

            potionCount--;
            healthPoints += healAmount;

            EventManager.ChangePotionCount(potionCount);
            EventManager.RaiseChangePlayerHealth(healthPoints);
        }

    }

}
