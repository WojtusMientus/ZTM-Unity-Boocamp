using UnityEngine;
using UnityEngine.Playables;
using RPG.Utility;

namespace RPG.Core
{
    public class CinematicController : MonoBehaviour
    {
        private PlayableDirector playableDirector;
        private Collider colliderComponent;

        [SerializeField] private bool customPlayOnAwake = false;

        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
            colliderComponent = GetComponent<Collider>();
        }

        private void Start()
        {
            colliderComponent.enabled = !PlayerPrefs.HasKey("SceneIndex");

            if (PlayerPrefs.GetInt("WasInScene") > 0 || !customPlayOnAwake) return;

            playableDirector.Play();
            colliderComponent.enabled = false;
        }

        private void OnEnable()
        {
            playableDirector.played += HandlePlayed;
            playableDirector.stopped += HandleStopped;
        }

        private void OnDisable()
        {
            playableDirector.played -= HandlePlayed;
            playableDirector.stopped -= HandleStopped;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Constants.PLAYER_TAG)) return;

            colliderComponent.enabled = false;
            playableDirector.Play();
        }

        private void HandlePlayed(PlayableDirector playableDirector)
        {
            EventManager.RaiseCutsceneUpdated(false);
        }

        private void HandleStopped(PlayableDirector playableDirector)
        {
            EventManager.RaiseCutsceneUpdated(true);
        }

    }
}