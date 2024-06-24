using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using RPG.Utility;


namespace RPG.Core
{
    public static class SceneTransition
    {
        public static IEnumerator Initiate(int sceneIndex)
        {
            AudioSource audioComopnent = GameObject.FindGameObjectWithTag(Constants.GAME_MANAGAER_TAG).GetComponent<AudioSource>();

            float durationOfSilencing = 2f;

            while (audioComopnent.volume > 0)
            {
                audioComopnent.volume -= Time.deltaTime / durationOfSilencing;

                yield return new WaitForEndOfFrame();
            }

            SceneManager.LoadScene(sceneIndex);
        }
    }
}


