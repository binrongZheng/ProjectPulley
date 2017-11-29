using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

namespace ourEngine {

	public class ourVector3 {
		public float x,y,z;

		public ourVector3 () {
			x = y = z = 0;
		}
		public ourVector3(float _x, float _y, float _z){
			x = _x;
			y = _y;
			z = _z;
		}

		public float GetMagnitude () {
			return UnityEngine.Mathf.Sqrt (UnityEngine.Mathf.Pow (x, 2) + UnityEngine.Mathf.Pow (y, 2) + UnityEngine.Mathf.Pow (z, 2));
		}

		public static ourVector3 operator + (ourVector3 v1, ourVector3 v2){
			ourVector3 res = new ourVector3();
			res.x = v1.x + v2.x;
			res.y = v1.y + v2.y;
			res.z = v1.z + v2.z;
			return res;
		}
		public static ourVector3 operator - (ourVector3 v1, ourVector3 v2){
			ourVector3 res = new ourVector3();
			res.x = v1.x - v2.x;
			res.y = v1.y - v2.y;
			res.z = v1.z - v2.z;
			return res;
		}

		public static float Dot (ourVector3 v1, ourVector3 v2){
			float res = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
			return res;
		}
		public static ourVector3 Cross (ourVector3 v1, ourVector3 v2){
			ourVector3 res = new ourVector3();
			res.x = v1.y * v2.z - v1.z * v2.y;
			res.y = v1.z * v2.x - v1.x * v2.z;
			res.z = v1.x * v2.y - v1.y * v2.x;
			return res;
		}
		public static float Distance (ourVector3 v1, ourVector3 v2){
			ourVector3 res = v1 - v2;
			return res.GetMagnitude();
		}

		public ourVector3 Normalize () {
			float magnitude = GetMagnitude ();
			ourVector3 res = new ourVector3 (x / magnitude, y / magnitude, z / magnitude);
			return res;
		} 

	}
}
