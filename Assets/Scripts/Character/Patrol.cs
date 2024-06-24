using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.AI;

namespace RPG.Character
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField] private GameObject splineGameObject;
        [SerializeField] private float walkDuration = 3.0f;
        [SerializeField] private float pauseDuration = 2.0f;

        private SplineContainer splineCmp;
        private NavMeshAgent agentCmp;

        private float splinePosition = 0.0f;
        private float splineLength = 0.0f;
        private float lengthWalked = 0.0f;
        private float walkTime = 0.0f;
        private float pauseTime = 0.0f;
        private bool isWalking = true;


        private void Awake()
        {
            if (splineGameObject == null)
                Debug.LogWarning($"{gameObject.name} does not have a Spline");

            splineCmp = splineGameObject.GetComponent<SplineContainer>();
            splineLength = splineCmp.CalculateLength();
            agentCmp = GetComponent<NavMeshAgent>();
        }

        public Vector3 GetNextPosition() => splineCmp.EvaluatePosition(splinePosition);

        public void CalculateNextPosition()
        {
            walkTime += Time.deltaTime;

            if (walkTime > walkDuration)
                isWalking = false;

            if (!isWalking)
            {
                pauseTime += Time.deltaTime;

                if (pauseTime < pauseDuration)
                    return;

                ResetTimers();
            }

            lengthWalked = (lengthWalked + Time.deltaTime * agentCmp.speed);

            if (lengthWalked > splineLength)
                lengthWalked = 0;

            splinePosition = Mathf.Clamp01(lengthWalked / splineLength);
        }

        public void ResetTimers()
        {
            pauseTime = 0.0f;
            walkTime = 0.0f;
            isWalking = true;
        }


        public Vector3 GetFarthererOutPosition()
        {
            float tempSplinePosition = splinePosition + 0.02f;

            if (tempSplinePosition >= 1)
                tempSplinePosition -= 1;

            return splineCmp.EvaluatePosition(tempSplinePosition);
        }

    }
}

