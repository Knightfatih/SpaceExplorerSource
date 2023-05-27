using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Benchmark
{
    public class FPS : MonoBehaviour
    {
        private float deltaTime = 0.0f;

        public TextMeshPro textMesh;

        // Update is called once per frame
        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            float fps = 1.0f / deltaTime;

            string text = Mathf.Ceil(fps).ToString();

            textMesh.text = text;
        }
    }

}