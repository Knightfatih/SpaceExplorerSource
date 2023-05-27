using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Benchmark.Classic
{
    public class Movement : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            Vector3 pos = transform.position;
            pos += transform.forward * GameManager.Instance.moveSpeed * Time.deltaTime;

            if (pos.z > GameManager.Instance.UpperBounds)
            {
                pos.z = GameManager.Instance.BottomBounds;
            }

            transform.position = pos;
        }
    }
}
