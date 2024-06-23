using UnityEngine;

namespace Utility
{

    public static class CameraShake
    {
        private static CameraShakeHelper cameraShakeHelper;
        private static Camera targetCamera;
        private static float remainingDuration;
        private static float magnitude;
        private static Vector3 originalPos;
        private static bool isShaking;

        //create helper object for the camera shake
        private static void EnsureHelperExists()
        {
            if (cameraShakeHelper == null)
            {
                GameObject helperObject = new GameObject("CameraShakeHelper");
                cameraShakeHelper = helperObject.AddComponent<CameraShakeHelper>();
                Object.DontDestroyOnLoad(helperObject);
            }
        }

        /// <summary>
        /// set the data in the monobeahaviour to handle the shake
        /// The value are set to case of death
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="duration"></param>
        /// <param name="mag"></param>
        public static void Shake(Camera cam, float duration = 0.4f, float mag =0.2f)
        {
            if (cam == null) return;
            EnsureHelperExists();

            // Check if a shake is already in progress
            if (isShaking && targetCamera == cam)
            {
                // Combine the duration and magnitude of the shakes
                remainingDuration += duration;
                magnitude = Mathf.Max(magnitude, mag);
            }
            else
            {
                targetCamera = cam;
                remainingDuration = duration;
                magnitude = mag;
                originalPos = targetCamera.transform.localPosition;
                isShaking = true;
            }
        }

        /// <summary>
        /// monobeahviour that handle the shake of the camera in the late Update
        /// </summary>
        private class CameraShakeHelper : MonoBehaviour
        {
            void LateUpdate()
            {
                if (!isShaking) return;

                //check tartget camera exist and not destroyed
                if (targetCamera == null)
                {
                    isShaking = false;
                    return;
                }

                if (remainingDuration > 0)
                {
                    float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
                    float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

                    targetCamera.transform.localPosition = new Vector3(originalPos.x + xOffset, originalPos.y + yOffset, originalPos.z);

                    remainingDuration -= Time.deltaTime;
                }
                else
                {
                    targetCamera.transform.localPosition = originalPos;
                    isShaking = false;
                }
            }
        }
    }
}

