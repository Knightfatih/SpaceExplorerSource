using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Collections;
using Benchmark.ECS;
using System;
using UnityEngine.UIElements;

namespace Benchmark
{
    public class Spawner : MonoBehaviour
    {
        [Header("Options")]
        [SerializeField] public int initialNumToSpawn;
        [SerializeField] public int seed = 12345;
        [SerializeField] public float heightRange = 50f;
        [SerializeField] private float maxScale = 1f;
        [SerializeField] private float maxSpeed = 25f;

        [Header("Unit")]
        [SerializeField] private Mesh unitMesh;
        [SerializeField] private GameObject classicPrefab;
        [SerializeField] private GameObject ECSConversionPrefab;

        [Header("Materials")]
        [SerializeField] private Material[] planetMaterials;

        private EntityManager entityManager;
        private EntityArchetype archetype;
        private Entity unitEntityPrefab;
        public Mode mode;
        //private World defaultWorld;
        private System.Random rng;

        public static Spawner spawner;

        public List<Entity> spheres = new List<Entity>();
        public List<GameObject> sphereClassic = new List<GameObject>();

        void Awake()
        {
            if (spawner == null)
            {
                spawner = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            rng = new System.Random(seed);

            if (GameManager.Instance != null)
            {
                mode = GameManager.Instance.Mode;
            }

            if (mode == Mode.ECSConversion)
            {
                SetupECSConversion();
            }

            if (mode == Mode.ECSPure)
            {
                SetupECSPure();
            }

            SpawnUnit(initialNumToSpawn);
            Debug.Log(initialNumToSpawn);
        }

        public void DeletingEntities()
        {
            foreach (Entity ent in spheres)
            {
                entityManager.DestroyEntity(ent);
            }
            spheres.Clear();

            foreach(GameObject gameObject in sphereClassic)
            {
                Destroy(gameObject);
            }
            sphereClassic.Clear();
        }

        private void SetupECSPure()
        {
            if (mode == Mode.ECSPure)
            {
                var defaultWorld = World.DefaultGameObjectInjectionWorld;
                entityManager = defaultWorld.EntityManager;
                archetype = entityManager.CreateArchetype
                (
                    typeof(Translation),
                    typeof(Rotation),
                    typeof(RenderMesh),
                    typeof(LocalToWorld),
                    typeof(MoveForward),
                    typeof(Scale),
                    typeof(RenderBounds)
                );
            }
        }

        private void SetupECSConversion()
        {
            if (mode == Mode.ECSConversion)
            {
                var defaultWorld = World.DefaultGameObjectInjectionWorld;
                entityManager = defaultWorld.EntityManager;
                GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
                unitEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ECSConversionPrefab, settings);
            }
        }

        public void SpawnUnit(int numToSpawn)
        {
            GameManager.Instance.HeadsUpDisplay.ShowTotalText();

            switch (mode)
            {
                case Mode.Classic:
                    SpawnClassic(numToSpawn);
                    break;
                case Mode.ECSConversion:
                    SpawnECSConversion(numToSpawn);
                    break;
                case Mode.ECSPure:
                    SpawnECSPure(numToSpawn);
                    break;
            }
        }

        private void SpawnClassic(int numToSpawn)
        {
            if (classicPrefab == null)
                return;

            for (int i = 0; i < numToSpawn; i++)
            {
                GameObject instance = Instantiate(classicPrefab);
                instance.transform.position = GetNoisePosition(rng);
                instance.transform.localScale = new float3(GetRandomScale(rng));
                Renderer renderer = instance.GetComponent<Renderer>();
                renderer.material = GetRandomMaterial();

                sphereClassic.Add(instance);
            }
        }

        private void SpawnECSConversion(int numToSpawn)
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                Entity myEntity = entityManager.Instantiate(unitEntityPrefab);
                entityManager.SetComponentData(myEntity, new Translation { Value = GetNoisePosition(rng) });
                entityManager.AddComponentData(myEntity, new MoveForward { speed = GetRandomSpeed(rng) });
                entityManager.AddComponentData(myEntity, new Scale { Value = GetRandomScale(rng) });
                entityManager.SetSharedComponentData(myEntity, new RenderMesh
                {
                    mesh = unitMesh,
                    material = GetRandomMaterial()
                });
                spheres.Add(myEntity);
            }
        }

        private void SpawnECSPure(int numToSpawn)
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                Entity myEntity = entityManager.CreateEntity(archetype);

                entityManager.AddComponentData(myEntity, new Translation { Value = GetNoisePosition(rng) });
                entityManager.AddComponentData(myEntity, new MoveForward { speed = GetRandomSpeed(rng) });
                entityManager.AddComponentData(myEntity, new Scale { Value = GetRandomScale(rng) });
                entityManager.SetSharedComponentData(myEntity, new RenderMesh
                {
                    mesh = unitMesh,
                    material = GetRandomMaterial()
                });
                spheres.Add(myEntity);
            }
        }

        public float3 GetNoisePosition(System.Random rng)
        {
            float3 position = float3.zero;

            float x = Mathf.PerlinNoise((float)rng.NextDouble(), 0);
            float y = Mathf.PerlinNoise((float)rng.NextDouble(), 1);
            float z = Mathf.PerlinNoise((float)rng.NextDouble(), 2);

            float randomX = Mathf.Lerp(GameManager.Instance.LeftBounds, GameManager.Instance.RightBounds, x);
            float randomY = Mathf.Lerp(-heightRange, heightRange, y);
            float randomZ = Mathf.Lerp(GameManager.Instance.UpperBounds, GameManager.Instance.BottomBounds, z);

            position += new float3(randomX, randomY, randomZ);

            return position;
        }
        
        public Material GetRandomMaterial()
        {
            int randomIndex = rng.Next(0, planetMaterials.Length);
            return planetMaterials[randomIndex];
        }

        private float GetRandomSpeed(System.Random rng)
        {
            float minSpeed = 1f;
            return UnityEngine.Random.Range(minSpeed, maxSpeed);
        }

        private float GetRandomScale(System.Random rng)
        {
            const float scaleMin = 0.1f;
            return UnityEngine.Random.Range(scaleMin, maxScale);
        }
    }
}
