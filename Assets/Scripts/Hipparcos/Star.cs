using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace demo
{
    [System.Serializable]
    public class Star
    {
        //JSON Example
        //{"Hip": 1, "Nc": 1, "Rarad": 0.0000159148, "Derad": 0.0190068680, "Plx": 4.55, "Bv": 0.482, "Dly": 716.83, "Xly": 716.70, "Yly": 0.01, "Zly": 13.62},
        //"Hip": The Hipparcos catalog number
        //"Nc": The number of components in the system
        //"Rarad": The right ascension of the object in radians
        //"Derad": The declination of the object in radians
        //"Plx": The parallax of the object in milliarcseconds
        //"Bv": The B-V color index of the object
        //"Dly": The light travel time from the object in days
        //"Xly": The X component of the object's position in light years.
        //"Yly": The Y
        //"Zly": The Z 
        public int Hip { get; set; }
        public int Nc { get; set; }
        public float Rarad { get; set; }
        public float Derad { get; set; }
        public float Plx { get; set; }
        public float Bv { get; set; }
        public float Dly { get; set; }
        public float Xly { get; set; }
        public float Yly { get; set; }
        public float Zly { get; set; }
    }
}
