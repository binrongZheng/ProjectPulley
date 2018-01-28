using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsVel : MonoBehaviour {
	public GameObject[] arrowVelocity;
	public GameObject[] initPos;
	public GameObject[] FinPos;


	private Vector3[] vectorDir;
	private float[] TensionAngle;

	// Use this for initialization
	void Start () {
		vectorDir=new Vector3[3];
		TensionAngle = new float[3];
	
	}
	
	// Update is called once per frame
	void Update () {
		arrowVelocity [0].transform.position = initPos [0].transform.position+new Vector3(0,initPos[0].transform.localScale.y/2,0) ;
		vectorDir [0] = FinPos[0].transform.position-new Vector3(FinPos[0].transform.localScale.x/2,0,0) - initPos [0].transform.position;
		TensionAngle [0]=Mathf.Atan2(vectorDir[0].y,-vectorDir[0].x)*Mathf.Rad2Deg;
		arrowVelocity [0].transform.rotation = Quaternion.AngleAxis (TensionAngle[0],new Vector3(0,0,-1));
	}
}
