using UnityEngine;


namespace RPG.Character
{
    public class AIPatrolState : AIBaseState
    {
        public override void EnterState(EnemyController enemy)
        {
            enemy.patrolCmp.ResetTimers();
        }

        public override void UpdateState(EnemyController enemy)
        {
            if (enemy.distanceFromPlayer < enemy.chaseRange)
            {
                enemy.SwitchState(enemy.chaseState);
                return;
            }

            Vector3 oldPosition = enemy.patrolCmp.GetNextPosition();

            enemy.patrolCmp.CalculateNextPosition();
            
            Vector3 currentPostion = enemy.transform.position;
            Vector3 newPostiion = enemy.patrolCmp.GetNextPosition();
            Vector3 offset = newPostiion - currentPostion;

            enemy.movementCmp.MoveAgentByOffset(offset);

            Vector3 farthereOutPosition = enemy.patrolCmp.GetFarthererOutPosition();

            Vector3 newForwardVector = farthereOutPosition - currentPostion;
            newForwardVector.y = 0;

            enemy.movementCmp.RotatePlayer(newForwardVector);

            if (oldPosition == newPostiion)
                enemy.movementCmp.isMoving = false;
        }
    }
}


