using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    float startZoom;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        startZoom = cam.fieldOfView;
    }

    public void StartZooming()
    {
        StartCoroutine(moveToX(startZoom, 179, 1));
    }
    public void StartUnzooming()
    {
        StartCoroutine(moveToX(179, startZoom, 1));
    }

    public void StartZoomingSettings()
    {
        StartCoroutine(moveToX(startZoom, 100, 1));
    }

    public void StartUnzoomingSettings()
    {
        StartCoroutine(moveToX(100, startZoom, 1));
    }

    public void StartZoomingCredits()
    {
        StartCoroutine(moveToX(20, startZoom, 1));
    }

    public void StartUnzoomingCredits()
    {
        StartCoroutine(moveToX(startZoom, 20, 1));
    }


    IEnumerator moveToX(float startZoom, float endZoom, float duration)
    {
        float counter = 0;

        //Get the current position of the object to be moved
        float startPos = startZoom;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(startZoom, endZoom, counter / duration);
            yield return null;
        }
    }
}
