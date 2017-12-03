using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ourEngine;

public class prova : UnityEngine.MonoBehaviour {

	ourVector3 provant = new ourVector3(1,0,0);
	ourVector3 provant2 = new ourVector3(5,0,0);

	// Use this for initialization
	void Start () {

		float alpha = 0.5f;
		Vector3 axis = new Vector3(5,3,7);

		Quaternion newQ;
		newQ.w = Mathf.Cos (alpha / 2);
		newQ.x = axis.x * Mathf.Sin (alpha / 2);
		newQ.y = axis.y * Mathf.Sin (alpha / 2);
		newQ.z = axis.z * Mathf.Sin (alpha / 2);

		ourQuaternion newQ2 = new ourQuaternion();
		newQ2.w = Mathf.Cos (alpha / 2);
		newQ2.x = axis.x * Mathf.Sin (alpha / 2);
		newQ2.y = axis.y * Mathf.Sin (alpha / 2);
		newQ2.z = axis.z * Mathf.Sin (alpha / 2);

		print (newQ.x + "," + newQ.y + "," + newQ.z + "," + newQ.w + " VS " + newQ2.x + "," + newQ2.y + "," + newQ2.z + "," + newQ2.w);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
