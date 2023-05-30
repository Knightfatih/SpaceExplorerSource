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

        [Header("Booleans")]
        public Toggle low;
        public Toggle mid;
        public Toggle high;

        public void OnSubmit()
        {
            int numberToSpawnValue;
            int.TryParse(numberToSpawnField.text, out numberToSpawnValue);

            int seedValue;
            int.TryParse(seedField.text, out seedValue);

            float heightRangeValue;
            float.TryParse(heightRangeField.text, out heightRangeValue);

            float upperBoundsValue;
            float.TryParse(upperBoundsField.text, out upperBoundsValue);

            float leftBoundsValue;
            float.TryParse(leftBoundsField.text, out leftBoundsValue);

            float rightBoundsValue;
            float.TryParse(rightBoundsField.text, out rightBoundsValue);

            float bottomBoundsValue;
            float.TryParse(bottomBoundsField.text, out bottomBoundsValue);

            if (low.isOn)
            {
                numberToSpawnField.text = "100";
                Values();
            }
            else if (mid.isOn)
            {
                numberToSpawnField.text = "1000";
                Values();
            }
            else if(high.isOn)
            {
                numberToSpawnField.text = "10000";
                Values();
            }

            //Spawner Reference
            Spawner.spawner.initialNumToSpawn = numberToSpawnValue;
            Spawner.spawner.seed = seedValue;
            Spawner.spawner.heightRange = heightRangeValue;

            //Game Manager Reference
            GameManager.Instance.upperBounds = upperBoundsValue;
            GameManager.Instance.leftBounds = leftBoundsValue;
            GameManager.Instance.rightBounds = rightBoundsValue;
            GameManager.Instance.bottomBounds = bottomBoundsValue;
            GameManager.Instance.spawnIncrement = numberToSpawnValue;
        }

        private void Values()
        {
            seedField.text = "12345";
            heightRangeField.text = "50";
            upperBoundsField.text = "1000";
            leftBoundsField.text = "-1000";
            rightBoundsField.text = "1000";
            bottomBoundsField.text = "-1000";
        }
    }
}