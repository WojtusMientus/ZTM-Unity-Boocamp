using System;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Utility;
using static UnityEngine.EventSystems.EventTrigger;

namespace RPG.Character
{
    public class Combat : MonoBehaviour
    {
        [NonSerialized] public float damage = 0f;

        [NonSerialized] public bool isAtacking = false;
        private Animator animatorComponent;
        private BubbleEvent bubbleEventComponent;


        private void Awake()
        {
            animatorComponent = GetComponentInChildren<Animator>();
            bubbleEventComponent = GetComponentInChildren<BubbleEvent>();
        }

        private void OnEnable()
        {
            bubbleEventComponent.OnBubbleStartAttack += HandleBubbleStartAttack;
            bubbleEventComponent.OnBubbleCompleteAttack += HandleBubbleCompleteAttack;
            bubbleEventComponent.OnBubbleHit += HandleBubbleHit;
        }

        private void OnDisable()
        {
            bubbleEventComponent.OnBubbleStartAttack -= HandleBubbleStartAttack;
            bubbleEventComponent.OnBubbleCompleteAttack -= HandleBubbleCompleteAttack;
            bubbleEventComponent.OnBubbleHit -= HandleBubbleHit;
        }

        public void HandleAttack(InputAction.CallbackContext context)
        {
            if (context.performed) return;
            StartAttack();
        }

        public void StartAttack()
        {
            if (isAtacking) return;

            animatorComponent.SetFloat(Constants.SPEED_ANIMATOR, 0);
            animatorComponent.SetTrigger(Constants.ATTACK_ANIMATOR);
        }

        private void HandleBubbleStartAttack()
        {
            isAtacking = true;
        }

        private void HandleBubbleCompleteAttack()
        {
            isAtacking = false;
        }

        private void HandleBubbleHit()
        {
            RaycastHit[] targets = Physics.BoxCastAll(transform.position + transform.forward, transform.localScale / 2, transform.forward, transform.rotation, 1f);

            foreach (RaycastHit target in targets)
            {
                if (CompareTag(target.transform.tag)) continue;

                Health healthComponent = target.transform.gameObject.GetComponent<Health>();

                if (healthComponent == null) continue;

                healthComponent.TakeDamage(damage);
            }
        }

        public void CancelAttack()
        {
            animatorComponent.ResetTrigger(Constants.ATTACK_ANIMATOR);
        }

    }

}
