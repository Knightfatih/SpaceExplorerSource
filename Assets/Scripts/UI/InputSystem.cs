using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Benchmark
{
    public class InputSystem : MonoBehaviour
    {
        [Header("Spawner Manager Script")]
        public InputField numberToSpawnField;
        public InputField seedField;
        public InputField heightRangeField;

        [Header("Game Manager Script")]
        public InputField upperBoundsField;
        public InputField leftBoundsField;
        public InputField rightBoundsField;
        public InputField bottomBoundsField;
        //public InputField spawnIncrementField;

        [Header("Booleans")]
        public Toggle low;
        public Toggle mid;
        public Toggle high;

        public void OnSubmit()
        {
            if (low.isOn)
            {
                numberToSpawnField.text = "100";
                seedField.text = "12345";
                heightRangeField.text = "50";
                upperBoundsField.text = "1000";
                leftBoundsField.text = "-1000";
                rightBoundsField.text = "1000";
                bottomBoundsField.text = "-1000";
            }
            if (mid.isOn)
            {
                numberToSpawnField.text = "1000";
                seedField.text = "12345";
                heightRangeField.text = "50";
                upperBoundsField.text = "1000";
                leftBoundsField.text = "-1000";
                rightBoundsField.text = "1000";
                bottomBoundsField.text = "-1000";
            }
            if (high.isOn)
            {
                numberToSpawnField.text = "10000";
                seedField.text = "12345";
                heightRangeField.text = "50";
                upperBoundsField.text = "1000";
                leftBoundsField.text = "-1000";
                rightBoundsField.text = "1000";
                bottomBoundsField.text = "-1000";
            }

            //Spawner
            int numberToSpawnValue = int.Parse(numberToSpawnField.text);
            int seedValue = int.Parse(seedField.text);
            float heightRangeValue = float.Parse(heightRangeField.text);

            //Game Manager
            float upperBoundsValue = float.Parse(upperBoundsField.text);
            float leftBoundsValue = float.Parse(leftBoundsField.text);
            float rightBoundsValue = float.Parse(rightBoundsField.text);
            float bottomBoundsValue = float.Parse(bottomBoundsField.text);
            //int spawnIncrementValue = int.Parse(spawnIncrementField.text);

            //Spawner Reference
            Spawner.spawner.initialNumToSpawn = numberToSpawnValue;
            Spawner.spawner.seed = seedValue;
            Spawner.spawner.HeightRange = heightRangeValue;

            //Game Manager Reference
            GameManager.Instance.upperBounds = upperBoundsValue;
            GameManager.Instance.leftBounds = leftBoundsValue;
            GameManager.Instance.rightBounds = rightBoundsValue;
            GameManager.Instance.bottomBounds = bottomBoundsValue;
            GameManager.Instance.spawnIncrement = numberToSpawnValue;
        }
    }
}