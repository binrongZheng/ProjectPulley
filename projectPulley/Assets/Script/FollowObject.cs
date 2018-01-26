using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ourEngine;

public class FollowObject : MonoBehaviour {

	private Vector3 offSet;
	public Transform target;

	// Use this for initialization
	void Start () {
		offSet = (target.position - transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.position - offSet;
	}
	void LateUpdate () {
		
	}
}
