using System.Collections;
using System.Collections.Generic;
using RPG.Character;
using UnityEngine;


namespace RPG.Character
{
    public class AIDefeatedState : AIBaseState
    {   public override void EnterState(EnemyController enemy)
        {
            AudioSource audioComopnent = enemy.GetComponent<AudioSource>();

            if (audioComopnent.clip == null) return;

            audioComopnent.Play();
        }

        public override void UpdateState(EnemyController enemy)
        {

        }
    }
}


