using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IK_FABRIK2 : MonoBehaviour
{
	public Transform[] joints;
	public Transform target;

	private Vector3[] copy;
	private float[] distances;
	private bool done;

	float threshold_distance = 0.1f;
	public int maxIterations=10;

	void Start()
	{
		distances = new float[joints.Length - 1];
		copy = new Vector3[joints.Length];
	}

	void Update()
	{
		// Copy the joints positions to work with
		// and calculate all the distances
		//TODO1
		for (int i = 0; i < joints.Length; i++) {
			copy[i] = joints[i].position;
			if (i < joints.Length - 1)
			{
				distances[i] = (joints[i + 1].position - joints[i].position).magnitude;
			}
		}


		done = (Vector3.Distance(target.position, joints[joints.Length - 1].position) < threshold_distance) ;
		if (!done)
		{
			float targetRootDist = Vector3.Distance(copy[0], target.position);

			// Update joint positions
			if (targetRootDist > distances.Sum())
			{
				// The target is unreachable
				//TODO3
				for (int i = 0; i < copy.Length - 1; i++) {
					float dist = (target.position - copy[i]).magnitude;
					float ratio = distances[i] / dist;

					copy[i + 1] = (1 - ratio) * copy[i] + ratio * target.position;

				}
			}
			else
			{
				// The target is reachable
				int counter=0;
				while (!done&&counter<maxIterations)
				{
					counter++;
					// STAGE 1: FORWARD REACHING
					//TODO
					copy[copy.Length-1]=target.position;
					for (int i = copy.Length - 1; i > 0; i--) {
						//agafar vector unitari i mutipllicar per la distancia i sumar la position anterior
						Vector3 temp =(copy[i-1]-copy[i]).normalized;
						temp *=  distances [i - 1];
						copy[i-1]=temp+copy[i];
					}
					// STAGE 2: BACKWARD REACHING
					//TODO
					copy[0]=joints[0].position;
					for (int i = 0; i < copy.Length - 2; i++) {
						//agafar vector unitari i mutipllicar per la distancia i sumar la position anterior
						Vector3 temp =(copy[i+1]-copy[i]).normalized;
						temp *=  distances [i];
						copy[i+1]=temp+copy[i];
					}
				}
			}

			// Update original joint rotations
			for (int i = 0; i <= joints.Length - 2; i++)
			{
				//TODO 3.

				//without rotations
				//joints[i].position = copy[i];

				//with rotations:
				Vector3 a, b;
				a =  joints[i+1].position - joints[i].position;
				b = copy[i + 1] - copy[i];
				Vector3 axis = Vector3.Cross(a, b).normalized;

				float cosa = Vector3.Dot(a, b) / (a.magnitude * b.magnitude);
				float sina = Vector3.Cross(a.normalized, b.normalized).magnitude;

				float alpha = Mathf.Atan2(sina, cosa);

				//float alpha = Mathf.Acos(Vector3.Dot(a, b) / (a.magnitude * b.magnitude));



				Quaternion q = new Quaternion(axis.x * Mathf.Sin(alpha / 2), axis.y * Mathf.Sin(alpha / 2), axis.z * Mathf.Sin(alpha / 2), Mathf.Cos(alpha / 2));
				joints[i].position = copy[i];
				joints[i].rotation = q * joints[i].rotation;

			}          
		}
	}

}
