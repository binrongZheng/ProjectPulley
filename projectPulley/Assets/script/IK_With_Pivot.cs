using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_With_Pivot : MonoBehaviour {

	public Transform[] joints;
	public Transform target;

	public Transform pivot;

	private Vector3[] copy;
	private Vector3 pivotCopy;
	private float[] distances;		
	private bool done;

	private bool aligned;

	private float threshold_distance = 0.1f;

	// Use this for initialization
	void Start () {
		pivotCopy = pivot.position;
		copy = new Vector3[joints.Length];
	}
	
	// Update is called once per frame
	void Update () {

		// Copy the joints positions to work with
		for (int i = 0; i< joints.Length;i++){
			copy [i] = joints [i].position;
		}



		//comparar vectors
		Vector3 v1 = target.position - pivotCopy;
		Vector3 n1 = Vector3.Cross (v1.normalized, new Vector3 (0, 1, 0)).normalized;
		Vector3 v2 = copy [copy.Length - 1] - pivotCopy;
		Vector3 n2 = Vector3.Cross (v2.normalized, new Vector3 (0, 1, 0)).normalized;

		float myCos = Vector3.Dot (n1, n2) / (n1.magnitude * n2.magnitude); //tmb es pot no dividir i fer v1.normlaized i v2.normalized
		float mySen = Vector3.Cross (n1.normalized, n2.normalized).magnitude;
		float alpha = Mathf.Atan2 (mySen, myCos);			

		//PRIMER ROTEM LA BASE
		if (alpha > 0.1f) {
			Vector3 axis = new Vector3 (0, 1, 0);

			Quaternion newQ = Quaternion.AngleAxis (alpha, axis);

			Quaternion newRot = newQ * (Quaternion)pivot.rotation;
			pivot.rotation = newRot;
		}
		else { //DESPRES APLIQUEM FABRIK
			
		}
	}

	void CalculateDistances (Vector3[] nodes) {
		for (int i = 0; i < nodes.Length-1; i++) { 
			distances [i] = Vector3.Distance (nodes [i], nodes [i + 1]);
		}
	}
}
