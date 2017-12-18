using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W) && transform.position.y < 3){
			transform.position += new Vector3(0,2 * Time.deltaTime,0);
		}
		if (Input.GetKey(KeyCode.S) && transform.position.y > 0){
			transform.position -= new Vector3(0,2 * Time.deltaTime,0);
		}
	}
}
