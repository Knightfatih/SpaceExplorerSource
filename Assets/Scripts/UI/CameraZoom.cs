using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float startZoom;
        private UnityEngine.Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            cam = UnityEngine.Camera.main;
            startZoom = cam.fieldOfView;
        }

        public void StartZoom(float targetZoom)
        {
            StartCoroutine(ZoomCoroutine(startZoom, targetZoom, 1f));
        }

        public void StartUnzoom(float targetZoom)
        {
            StartCoroutine(ZoomCoroutine(targetZoom, startZoom, 1f));
        }

        private IEnumerator ZoomCoroutine(float start, float end, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                cam.fieldOfView = Mathf.Lerp(start, end, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cam.fieldOfView = end;
        }
    }

}
