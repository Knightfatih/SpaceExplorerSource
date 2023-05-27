using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Unity.Entities;

namespace Benchmark.ECS
{
    [GenerateAuthoringComponent]
    public struct MoveForward : IComponentData
    {
        public float speed;
    }
}
