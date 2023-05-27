using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

namespace Benchmark.ECS
{
    public class MovementSystemJobsBurst : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            float upperBounds = GameManager.Instance.UpperBounds;
            float lowerBounds = GameManager.Instance.BottomBounds;

            JobHandle jobHandle = Entities.
                ForEach((ref Translation trans, ref Rotation rot, in MoveForward moveForward) =>
                {
                    trans.Value += new float3(0f, 0f, moveForward.speed * deltaTime);

                    if (trans.Value.z >= upperBounds)
                    {
                        trans.Value.z = lowerBounds;
                    }

                    //rot.Value = math.mul(rot.Value, quaternion.RotateY(math.radians(moveForward.rotation * deltaTime)));

                }).Schedule(inputDeps);

            return jobHandle;
        }
    }
}
