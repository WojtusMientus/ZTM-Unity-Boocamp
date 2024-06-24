using UnityEngine;


namespace RPG.Character
{
    public class AIReturnState: AIBaseState
    {

        private Vector3 targetPosition;

        public override void EnterState(EnemyController enemy)
        {
            enemy.movementCmp.UpdateAgentSpeed(enemy.stats.walkSpeed, true);

            if (enemy.patrolCmp != null)
            {
                targetPosition = enemy.patrolCmp.GetNextPosition();
                enemy.movementCmp.MoveAgentByDestination(targetPosition);
            }
            else
                enemy.movementCmp.MoveAgentByDestination(enemy.originalPosition);
        }

        public override void UpdateState(EnemyController enemy)
        {
            if (enemy.distanceFromPlayer < enemy.chaseRange)
            {
                enemy.SwitchState(enemy.chaseState);
                return;
            }

            if (enemy.movementCmp.ReachedDestination())
            {
                if (enemy.patrolCmp != null)
                {
                    enemy.SwitchState(enemy.patrolState);
                    return;
                }
                else
                {
                    enemy.movementCmp.isMoving = false;
                    enemy.movementCmp.RotatePlayer(enemy.movementCmp.originalForwardVector);
                }
            }

            else
            {
                if (enemy.patrolCmp != null)
                {
                    Vector3 newForwardVec = targetPosition - enemy.transform.position;
                    newForwardVec.y = 0;
                    enemy.movementCmp.RotatePlayer(newForwardVec);
                }
                else
                {
                    Vector3 newForwardVector = enemy.originalPosition - enemy.transform.position;
                    newForwardVector.y = 0;
                    enemy.movementCmp.RotatePlayer(newForwardVector);
                }
            }

        }
    }
}