using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsManager : MonoBehaviour {

	public GameObject arrowForces;
	public GameObject velocityForces;

	public GameObject trailLoad,trailTarget;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			arrowForces.SetActive(!arrowForces.activeSelf);
		}
		if (Input.GetKeyDown(KeyCode.G)) {
			velocityForces.SetActive(!velocityForces.activeSelf);
		}
		if (Input.GetKeyDown(KeyCode.B)) {
			trailLoad.SetActive(!trailLoad.activeSelf);
			trailTarget.SetActive(!trailTarget.activeSelf);
		}
	}


}
