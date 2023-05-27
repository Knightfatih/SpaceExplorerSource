using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Benchmark
{
    public class UI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshPro spawnTotalTextMesh;
        [SerializeField] private TextMeshPro modeTextMesh;
        [SerializeField] private TextMeshPro jobsTextMesh;
        [SerializeField] private TextMeshPro burstTextMesh;
        public bool mainMenu = true;

        public TextMeshPro cameraPosText;

        private void Start()
        {
            ShowModeText();
        }

        void Update()
        {
            if (!mainMenu)
            {
                Vector3 cameraPosition = UnityEngine.Camera.main.transform.position;
                string cameraPosString = "Coordinates:\n"
                    + "X: " + cameraPosition.x.ToString("F2") + "\n"
                    + "Y: " + cameraPosition.y.ToString("F2") + "\n"
                    + "Z: " + cameraPosition.z.ToString("F2");
                cameraPosText.text = cameraPosString;
            }
        }

        public void ShowTotalText()
        {
            if (!mainMenu)
            {
                if (spawnTotalTextMesh != null && Spawner.spawner.mode == Mode.ECSConversion || Spawner.spawner.mode == Mode.ECSPure)
                {
                    spawnTotalTextMesh.text = " " + (Spawner.spawner.spheres.Count + Spawner.spawner.initialNumToSpawn).ToString();
                }
                if (spawnTotalTextMesh != null && Spawner.spawner.mode == Mode.Classic)
                {
                    spawnTotalTextMesh.text = " " + (Spawner.spawner.sphereClassic.Count + Spawner.spawner.initialNumToSpawn).ToString();
                }
            }
            //if (spawnTotalTextMesh != null)
            //    spawnTotalTextMesh.text = totalSpawned.ToString();
        }

        public void ShowModeText()
        {
            if (modeTextMesh != null)
            {
                switch (GameManager.Instance.Mode)
                {
                    case Mode.Classic:
                        modeTextMesh.text = "Classic";
                        break;
                    case Mode.ECSConversion:
                        modeTextMesh.text = "ECS Conversion";
                        break;
                    case Mode.ECSPure:
                        modeTextMesh.text = "ECS Pure";
                        break;
                }
            }
        }

        public void ShowBurstText(bool state)
        {
            if (burstTextMesh != null)
            {
                burstTextMesh.text = (state) ? "ON" : "OFF";
            }
            if (!mainMenu)
            {
                burstTextMesh.transform.parent.gameObject.SetActive((GameManager.Instance.Mode != Mode.Classic));
            }
        }

        public void ShowJobsText(bool state)
        {
            if (jobsTextMesh != null)
            {
                jobsTextMesh.text = (state) ? "ON" : "OFF";
            }
            if (!mainMenu)
            {
                jobsTextMesh.transform.parent.gameObject.SetActive((GameManager.Instance.Mode != Mode.Classic));
            }
        }
    }

}
