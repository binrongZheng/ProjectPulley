using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using ourEngine;

public class prova : UnityEngine.MonoBehaviour {

	ourVector3 provant = new ourVector3(1,0,0);
	ourVector3 provant2 = new ourVector3(5,0,0);

	// Use this for initialization
	void Start () {
		ourVector3 res = ourVector3.Cross (provant, provant2);

		print (provant.Normalize().y);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
