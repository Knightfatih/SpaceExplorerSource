using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

namespace Benchmark.ECS
{
    public class EntityIndex : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JobHandle jobHandle = Entities.
                ForEach((int entityInQueryIndex) => //ref Spawner spawner
                {
                    //Debug.Log(entityInQueryIndex);
                    //entityInQueryIndex = spawner.entityIndex;

                }).Schedule(inputDeps);

            return jobHandle;
        }
    }
}