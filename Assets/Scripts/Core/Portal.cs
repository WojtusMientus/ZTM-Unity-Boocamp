using UnityEngine;
using RPG.Utility;
using UnityEngine.SceneManagement;


namespace RPG.Core
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int nextSceneIndex;
        public Transform spawnPoint;
        private Collider colliderComponent;

        private void Awake()
        {
            colliderComponent = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Constants.PLAYER_TAG)) return;

            colliderComponent.enabled = false;

            EventManager.RaisePortalEnter(other, nextSceneIndex);

            StartCoroutine(SceneTransition.Initiate(nextSceneIndex));
        }
    }
}