using UnityEngine;

namespace RPG.Utility
{
    public class Billboard : MonoBehaviour
    {
        private new GameObject camera;


        private void Awake()
        {
            camera = GameObject.FindGameObjectWithTag(Constants.CAMERA_TAG);
        }

        private void LateUpdate()
        {
            Vector3 cameraDirection = transform.position + camera.transform.forward;

            transform.LookAt(cameraDirection);
        }

    }
}