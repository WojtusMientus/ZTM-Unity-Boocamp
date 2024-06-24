using System;
using RPG.Utility;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Movement : MonoBehaviour
    {
        [NonSerialized] public Vector3 originalForwardVector;
        [NonSerialized] public bool isMoving = false;
        [SerializeField] private Animator animatorCmp;

        private NavMeshAgent navMeshAgent;
        private Vector3 movementVector;
        private bool clampAnimationSpeed = true;



        private void Awake()
        {
            originalForwardVector = transform.forward;
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            navMeshAgent.updateRotation = false;
        }

        private void Update()
        {
            MovePlayer();
            MovementAnimator();
            if (CompareTag(Constants.PLAYER_TAG))
                RotatePlayer(movementVector);
        }

        private void MovePlayer()
        {
            Vector3 finalMovement = movementVector * Time.deltaTime * navMeshAgent.speed;
            navMeshAgent.Move(finalMovement);
        }

        public void RotatePlayer(Vector3 newForwardVector)
        {
            if (newForwardVector == Vector3.zero) return;

            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.LookRotation(newForwardVector);

            transform.rotation = Quaternion.Lerp(startRotation, endRotation, Time.deltaTime * navMeshAgent.angularSpeed);

        }

        public void HandleMove(InputAction.CallbackContext context)
        {
            if (context.performed) isMoving = true;
            if (context.canceled) isMoving = false;

            Vector2 input = context.ReadValue<Vector2>();
            movementVector = new Vector3(input.x, 0.0f, input.y);
        }

        public void MoveAgentByDestination(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
            isMoving = true;
        }

        public void StopMovingAgent()
        {
            navMeshAgent.ResetPath();
            isMoving = false;
        }

        public bool ReachedDestination()
        {
            if (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance || navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude != 0.0f) 
                return false;

            return true;
        }

        public void MoveAgentByOffset(Vector3 offset)
        {
            navMeshAgent.Move(offset);
            isMoving = true;
        }


        public void UpdateAgentSpeed(float newSpeed, bool shouldClampSpeed)
        {
            navMeshAgent.speed = newSpeed;
            clampAnimationSpeed = shouldClampSpeed;
        }

        public void MovementAnimator()
        {
            float speed = animatorCmp.GetFloat(Constants.SPEED_ANIMATOR);
            float smoothening = Time.deltaTime * navMeshAgent.acceleration;

            if (isMoving)
                speed += smoothening;
            else
                speed -= smoothening;

            speed = Mathf.Clamp01(speed);

            if (CompareTag(Constants.ENEMY_TAG) && clampAnimationSpeed)
            {
                speed = Mathf.Clamp(speed, 0f, 0.5f);
            }

            animatorCmp.SetFloat(Constants.SPEED_ANIMATOR, speed);
        }

    }
}


