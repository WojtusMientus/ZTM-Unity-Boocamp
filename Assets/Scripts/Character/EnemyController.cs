using UnityEngine;
using RPG.Utility;
using RPG.Core;
using System;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;


namespace RPG.Character
{
    public class EnemyController : MonoBehaviour
    {
        [NonSerialized] public Movement movementCmp;
        [NonSerialized] public Vector3 originalPosition;
        [NonSerialized] public GameObject player;
        [NonSerialized] public Patrol patrolCmp;

        [NonSerialized] public bool isUIOpened = false;

        public string enemyID = string.Empty;
        public float chaseRange = 2.5f;
        public float attackRange = 0.75f;

        [NonSerialized] public float distanceFromPlayer;

        private Health healthCmp;
        [NonSerialized] public Combat combatCmp;
        public CharacterStatusSO stats;


        private AIBaseState currentState;
        [NonSerialized] public AIReturnState returnState = new AIReturnState();
        [NonSerialized] public AIChaseState chaseState = new AIChaseState();
        [NonSerialized] public AIAttackState attackState = new AIAttackState();
        [NonSerialized] public AIDefeatedState defeatedState = new AIDefeatedState();
        [NonSerialized] public AIPatrolState patrolState = new AIPatrolState();

        private void Awake()
        {
            if (enemyID.Length == 0)
                Debug.LogWarning($"{name} DOES NOT HAVE AN ID");

            player = GameObject.FindWithTag(Constants.PLAYER_TAG);
            patrolCmp = GetComponent<Patrol>();
            movementCmp = GetComponent<Movement>();
            healthCmp = GetComponent<Health>();
            combatCmp = GetComponent<Combat>();
             

            currentState = returnState;
            originalPosition = transform.position;
        }

        private void Start()
        {
            currentState.EnterState(this);
            healthCmp.healthPoints = stats.health;
            combatCmp.damage = stats.damage;

            if (healthCmp != null)
            {
                healthCmp.sliderComponent.maxValue = stats.health;
                healthCmp.sliderComponent.value = stats.health;
            }

            List<string> enemiesDefeated = PlayerPrefsUtility.GetString("EnemiesDefeated");

            foreach (string ID in enemiesDefeated)
                if (ID.Equals(enemyID))
                    Destroy(gameObject);
        }

        private void OnEnable()
        {
            healthCmp.OnStartDefeated += HandleStartDefeated;
            EventManager.OnToggleUI += HandleToggleUI;
        }

        private void OnDisable()
        {
            healthCmp.OnStartDefeated -= HandleStartDefeated;
            EventManager.OnToggleUI += HandleToggleUI;
        }

        private void Update()
        {
            CalcualteDistance();
            currentState.UpdateState(this);
        }

        public void SwitchState(AIBaseState newState)
        {
            currentState = newState;
            currentState.EnterState(this);
        }

        private void CalcualteDistance()
        {
            if (player == null) return;

            Vector3 enemyPosition = transform.position;
            Vector3 playerPosition = player.transform.position;

            distanceFromPlayer = Vector3.Distance(enemyPosition, playerPosition);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        private void HandleStartDefeated()
        {
            SwitchState(defeatedState);
        }

        private void HandleToggleUI(bool isOpened)
        {
            isUIOpened = isOpened;
        }


    }
}

