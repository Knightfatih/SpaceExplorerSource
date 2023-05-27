using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Benchmark;

namespace demo
{
    [System.Serializable]
    public class Hipparcos : MonoBehaviour
    {
        [SerializeField] private Mesh starMesh;
        [SerializeField] private Material planetMaterial_1;
        [SerializeField] private Material planetMaterial_2;
        [SerializeField] private Material planetMaterial_3;
        [SerializeField] private Material planetMaterial_4;

        [SerializeField] private TextMeshPro planetPopulation;
        private EntityManager entityManager;
        private EntityArchetype starArchetype;

        public List<Entity> demoSpheres = new List<Entity>();

        public TextAsset test;

        // Start is called before the first frame update
        void Awake()
        {
            string jsonData = test.text;
            Catalogue catalogue = JsonConvert.DeserializeObject<Catalogue>(jsonData);

            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            starArchetype = entityManager.CreateArchetype(
               typeof(Translation),
               typeof(RenderMesh),
               typeof(RenderBounds),
               typeof(LocalToWorld)
               );

            StartCoroutine(InitializeStars(catalogue));

            ShowPopulation();
        }

        void ShowPopulation()
        {
            planetPopulation.text = "Stars: " + demoSpheres.Count.ToString();
        }

        private IEnumerator InitializeStars(Catalogue catalogue)
        {
            foreach (Star star in catalogue.Stars)
            {
                SpawnStar(star);
            }
            yield return null;
        }

        void SpawnStar(Star star)
        {
            Material starMaterial;

            if (star.Bv < -0.30) { starMaterial = planetMaterial_1; }
            else if (star.Bv < -0.4) { starMaterial = planetMaterial_2; }
            else if (star.Bv < 1.6) { starMaterial = planetMaterial_3; }
            else { starMaterial = planetMaterial_4; }

            Entity newStarEntity = entityManager.CreateEntity(starArchetype);
            entityManager.SetComponentData(newStarEntity, new Translation { Value = new float3(star.Xly, star.Yly, star.Zly) });
            entityManager.SetSharedComponentData(newStarEntity, new RenderMesh { mesh = starMesh, material = starMaterial });
            demoSpheres.Add(newStarEntity);
        }

        public void DeletingEntities()
        {
            foreach (Entity ent in demoSpheres)
            {
                entityManager.DestroyEntity(ent);
            }
            demoSpheres.Clear();
        }
    }

}
