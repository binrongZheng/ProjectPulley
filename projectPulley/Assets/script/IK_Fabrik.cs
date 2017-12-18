﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IK_Fabrik : MonoBehaviour {

	public Transform[] joints;
	public Transform target;

	private Vector3[] copy;		
	private float[] distances;		
	private bool done;

	private float threshold_distance = 0.1f;
	private float maxIterations = 50;

	void Start () {
		distances = new float[joints.Length - 1];
		copy = new Vector3[joints.Length];
	}

	void Update () {

		//copiar els joints a copy i calcular distancies
		for (int i = 0; i < joints.Length;i++){
			copy [i] = joints[i].position;
			if (i < joints.Length - 1) {
				distances [i] = (joints [i + 1].position - joints [i].position).magnitude;
			}
		}

		//Comprovem si hem acabat
		done = Vector3.Distance(target.position, joints[joints.Length-1].position) < threshold_distance;

		if (!done) {
			float targetRootDist = Vector3.Distance (copy [0], target.position);

			//Mirar si ems hem passat
			if (targetRootDist > distances.Sum ()) {
				for (int i = 0; i < copy.Length - 1; i++) {
					float dist = (target.position - copy [i]).magnitude;
					float ratio = distances [i] / dist;

					copy [i + 1] = (1 - ratio) * copy [i] + ratio * target.position;
				}
			}
			else {
				
				int counter = 0;
				//El target esta a l'alcanç
				while (!done && counter < maxIterations){
					counter++;

					//STAGE 1: FORWARD REACHING
					copy[copy.Length-1] = target.position;
					for (int i = copy.Length - 1; i > 0; i--) {
						Vector3 temp = (copy [i - 1] - copy [i]).normalized;
						temp = temp * distances [i - 1];
						copy [i - 1] = temp + copy [i];
					}

					//STAGE 2: BACKWARD REACHING
					copy[0] = joints[0].position;
					for (int i = 0; i < copy.Length - 2; i++) {
						Vector3 temp = (copy [i + 1] - copy [i]).normalized;
						temp = temp * distances [i];
						copy [i + 1] = temp + copy [i];
					}

					//tornem a calcular el done
					done = Vector3.Distance(target.position, joints[joints.Length-1].position) < threshold_distance;
				}
			}

			//Rotem els joints
			for (int i = 0; i <= joints.Length -2; i++){
				joints [i].position = copy [i];

				Vector3 a, b;
				a = joints [i + 1].position - joints [i].position;
				b = copy [i + 1] - copy [i];
				Vector3 axis = Vector3.Cross (a, b).normalized;

				float cosa = Vector3.Dot(a,b) / (a.magnitude * b.magnitude);
				float sina = Vector3.Cross (a.normalized, b.normalized).magnitude;

				float alpha = Mathf.Atan2 (sina, cosa);

				Quaternion q = new Quaternion (axis.x * Mathf.Sin (alpha / 2), axis.y * Mathf.Sin (alpha / 2), axis.z * Mathf.Sin (alpha / 2), Mathf.Cos (alpha / 2));
				joints [i].position = copy [i];
				joints [i].rotation = q * joints [i].rotation;
			}
		}
	}

}
