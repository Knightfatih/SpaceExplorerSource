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
        [SerializeField] public int seed = 12345;
        [SerializeField] public int initialNumToSpawn;
        [SerializeField] public float HeightRange = 1;
        [SerializeField] private float maxScale = 1f;
        [SerializeField] private float maxSpeed = 25f;

        [Header("Unit")]
        [SerializeField] private Mesh unitMesh;
        [SerializeField] private GameObject classicPrefab;
        [SerializeField] private GameObject ECSConversionPrefab;

        [Header("Materials")]
        [SerializeField] private Material planetMaterial_1;
        [SerializeField] private Material planetMaterial_2;
        [SerializeField] private Material planetMaterial_3;
        [SerializeField] private Material planetMaterial_4;
        [SerializeField] private Material planetMaterial_5;
        [SerializeField] private Material planetMaterial_6;
        [SerializeField] private Material planetMaterial_7;
        [SerializeField] private Material planetMaterial_8;

        private int totalSpawned;
        private EntityManager entityManager;
        private World defaultWorld;
        private EntityArchetype archetype;
        private Entity unitEntityPrefab;
        public Mode mode;
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
                //Debug.Log("Entites have been Deleted");
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
                defaultWorld = World.DefaultGameObjectInjectionWorld;
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
                defaultWorld = World.DefaultGameObjectInjectionWorld;
                entityManager = defaultWorld.EntityManager;
                GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
                unitEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ECSConversionPrefab, settings);
            }
        }

        public void SpawnUnit(int numToSpawn)
        {
            //totalSpawned += numToSpawn;
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
                //Debug.Log(sphereClassic.Count);
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
                entityManager.AddComponentData(myEntity, new Scale { Value = GetRandomScale(rng) });

                entityManager.SetSharedComponentData(myEntity, new RenderMesh
                {
                    mesh = unitMesh,
                    material = GetRandomMaterial()
                });

                entityManager.AddComponentData(myEntity, new MoveForward { speed = GetRandomSpeed(rng) });

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
            float randomY = Mathf.Lerp(-HeightRange, HeightRange, y);
            float randomZ = Mathf.Lerp(GameManager.Instance.UpperBounds, GameManager.Instance.BottomBounds, z);

            position += new float3(randomX, randomY, randomZ);
            //Debug.Log(position);

            return position;
        }

        //private float3 GetRandomPosition()
        //{
        //    float randomX = UnityEngine.Random.Range(GameManager.Instance.LeftBounds, GameManager.Instance.RightBounds);
        //    float randomY = UnityEngine.Random.Range(-1f, 1f) * HeightRange;
        //    float randomZ = UnityEngine.Random.Range(GameManager.Instance.UpperBounds, GameManager.Instance.BottomBounds);
        //    return new float3(randomX, randomY, randomZ);
        //}
        
        public Material GetRandomMaterial()
        {
            Material[] planetMaterials = new Material[]
            {
                planetMaterial_1,
                planetMaterial_2,
                planetMaterial_3,
                planetMaterial_4,
                planetMaterial_5,
                planetMaterial_6,
                planetMaterial_7,
                planetMaterial_8
            };

            //int randomIndex = UnityEngine.Random.Range(0, planetMaterials.Length);
            int randomIndex = rng.Next(0, planetMaterials.Length);
            return planetMaterials[randomIndex];
        }

        private float GetRandomSpeed(System.Random rng) //Delete System.Random rng if doesnt work
        {
            float minSpeed = 1f;
            return UnityEngine.Random.Range(minSpeed, maxSpeed);
        }

        private float GetRandomScale(System.Random rng) //Delete System.Random rng if doesnt work
        {
            const float scaleMin = 0.1f;
            return UnityEngine.Random.Range(scaleMin, maxScale);
        }
    }
}
