using UnityEngine;

namespace RPG.Audio
{
    public class AttackSFX : MonoBehaviour
    {

        private AudioSource audioComponent;

        private void Awake()
        {
            audioComponent = GetComponent<AudioSource>();
        }

        public void OnStartAttack()
        {
            if (audioComponent.clip == null) return;

            audioComponent.Play();
        }
    }
}