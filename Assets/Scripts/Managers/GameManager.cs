using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Entities;
using Benchmark.ECS;
using Menu;

[System.Serializable]
public enum Mode
{
    Classic,
    ECSConversion,
    ECSPure
}

namespace Benchmark
{
    [RequireComponent(typeof(UI))]
    public class GameManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Mode mode;
        [SerializeField] bool useJobs;
        [SerializeField] bool useBurst;

        [Header("Spawn")]
        [SerializeField] private Spawner spawner;
        [SerializeField] public int spawnIncrement;

        [Header("Boundaries")]
        [SerializeField] public float upperBounds;
        [SerializeField] public float leftBounds;
        [SerializeField] public float rightBounds;
        [SerializeField] public float bottomBounds;

        private UI ui;

        public float UpperBounds => upperBounds;
        public float BottomBounds => bottomBounds;
        public float LeftBounds => leftBounds;
        public float RightBounds => rightBounds;
        public Mode Mode => mode;
        public UI HeadsUpDisplay => ui;

        public float moveSpeed; //Movement script using

        public static GameManager Instance;

        public bool demo = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            spawner = Object.FindObjectOfType<Spawner>();
            ui = GetComponent<UI>();

        }

        private void Start()
        {
            SetSystemMode();
        }

        private void Update()
        {
            if (!demo)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !PauseMenu.gameIsPaused)
                {
                    spawner?.SpawnUnit(spawnIncrement);
                    Debug.Log(spawnIncrement);
                }
            }
        }


        private void SetSystemMode()
        {
            ui.ShowBurstText(useBurst);
            ui.ShowJobsText(useJobs);

            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<MovementSystem>().Enabled = false;
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<MovementSystemJobs>().Enabled = false;
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<MovementSystemJobsBurst>().Enabled = false;

            if (mode != Mode.Classic)
            {
                if (!useJobs)
                {
                    World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<MovementSystem>().Enabled = true;
                    Debug.Log("Enabling MovementSystem");
                }
                else if (useBurst)
                {
                    World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<MovementSystemJobsBurst>().Enabled = true;
                    Debug.Log("Enabling MovementSystemJobsBurst");
                }
                else if (!useBurst)
                {
                    World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<MovementSystemJobs>().Enabled = true;
                    Debug.Log("Enabling MovementSystemJobs");
                }

            }
        }
    }
}
