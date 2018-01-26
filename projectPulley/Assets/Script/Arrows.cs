using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arrows : MonoBehaviour {

	public GameObject[] arrowForces;

	public Transform[] initPos;

	public GameObject[] arrowTensioForces;
	public GameObject[] initTensioForces;
	public GameObject[] FinTensioForces;


	private float i = 0;
	private float pL= 0;
	private float pulleyL= 0;
	private float pulleyL2= 0;
	private float[] pulleyT;


	private Vector3[] vectorDir;
	private float[] TensionAngle;

	// Use this for initialization
	void Start () {
		pulleyL = Manager.pulleyForce [0]/10000;
		pulleyL = Mathf.Clamp (pulleyL,0f,0.2f);
		arrowForces[2].transform.localScale=new Vector3(pulleyL,arrowForces[2].transform.localScale.y,arrowForces[2].transform.localScale.z);
		vectorDir=new Vector3[3];
		TensionAngle = new float[3];
		pulleyT=new float[3];

		if(Manager.systemType==Manager.SystemType.twoPulleySystem){
			pulleyL2 = Manager.pulleyForce [1]/10000;
			pulleyL2 = Mathf.Clamp (pulleyL2,0f,0.2f);
			arrowForces[3].transform.localScale=new Vector3(pulleyL,arrowForces[3].transform.localScale.y,arrowForces[3].transform.localScale.z);

		}
	}

	// Update is called once per frame
	void Update () {
		
		//print (pulleyL);


		//pL = Manager.p_load / 1500;

		if (Manager.systemType == Manager.SystemType.fixedPulley) {
			i = Manager.outputForce / 1500;
			//print (i);
			i = Mathf.Clamp (i,0f,0.2f);
			//print (i);
			arrowForces[0].transform.position = initPos[0].position;
			arrowForces[0].transform.localScale=new Vector3(i,arrowForces[0].transform.localScale.y,arrowForces[0].transform.localScale.z);

			pL = Manager.p_load / 1500;
			//print (pL);
			pL = Mathf.Clamp (pL,0f,0.2f);
			//print (pL);
			arrowForces[1].transform.position = initPos[1].position-new Vector3(0,initPos[1].localScale.y/2,0);
			arrowForces[1].transform.localScale=new Vector3(pL,arrowForces[1].transform.localScale.y,arrowForces[1].transform.localScale.z);


			for (int i = 0; i < arrowTensioForces.Length; i++) {
				pulleyT [i] = Manager.tension [i] / 10000;
				pulleyT [i] = Mathf.Clamp (pulleyT[i],0,0.16f);

				arrowTensioForces[i].transform.localScale=new Vector3(pulleyT[i],arrowTensioForces[i].transform.localScale.y,arrowTensioForces[i].transform.localScale.z);

			}

			arrowTensioForces [0].transform.position = initPos [0].position;
			vectorDir [0] = FinTensioForces[0].transform.position+new Vector3(FinTensioForces[0].transform.localScale.x/2,0,0) - initPos [0].position;
			TensionAngle [0]=Mathf.Atan2(vectorDir[0].y,-vectorDir[0].x)*Mathf.Rad2Deg;
			arrowTensioForces [0].transform.rotation = Quaternion.AngleAxis (TensionAngle[0],new Vector3(0,0,-1));

			arrowTensioForces [1].transform.position = initPos [1].position+new Vector3(0, initPos[0].localScale.y/2,0);
			vectorDir [1] = FinTensioForces[1].transform.position-new Vector3(FinTensioForces[1].transform.localScale.x/2,0,0)  - initPos [1].position;
			TensionAngle[1]=Mathf.Atan2(vectorDir[1].y,-vectorDir[1].x)*Mathf.Rad2Deg;
			arrowTensioForces[1].transform.rotation= Quaternion.AngleAxis (TensionAngle[1],new Vector3(0,0,-1));

		}
		else if(Manager.systemType == Manager.SystemType.movablePulley){
			i = Manager.outputForce / 1500;
			//print (i);
			i = Mathf.Clamp (i,0f,0.2f);
			//print (i);
			arrowForces[0].transform.position = initPos[0].position;
			arrowForces[0].transform.localScale=new Vector3(i,arrowForces[0].transform.localScale.y,arrowForces[0].transform.localScale.z);

			pL = Manager.p_load / 1500;
			//print (pL);
			pL = Mathf.Clamp (pL,0f,0.2f);
			//print (pL);
			arrowForces[1].transform.position = initPos[1].position+new Vector3(0,0,initPos[1].localScale.z/2);
			arrowForces[1].transform.localScale=new Vector3(pL,arrowForces[1].transform.localScale.y,arrowForces[1].transform.localScale.z);


			for (int i = 0; i < arrowTensioForces.Length; i++) {
				pulleyT [i] = Manager.tension [i] / 10000;
				pulleyT [i] = Mathf.Clamp (pulleyT[i],0,0.3f);

				arrowTensioForces[i].transform.localScale=new Vector3(pulleyT[i],arrowTensioForces[i].transform.localScale.y,arrowTensioForces[i].transform.localScale.z);

			}

			arrowTensioForces [0].transform.position = initTensioForces [0].transform.position-new Vector3(initTensioForces[0].transform.localScale.x/2,0,0);
			vectorDir [0] = FinTensioForces[0].transform.position+new Vector3(FinTensioForces[0].transform.localScale.x/2,0,0) - initTensioForces [0].transform.position;
			TensionAngle [0]=Mathf.Atan2(vectorDir[0].y,-vectorDir[0].x)*Mathf.Rad2Deg;
			arrowTensioForces [0].transform.rotation = Quaternion.AngleAxis (TensionAngle[0],new Vector3(0,0,-1));

			arrowTensioForces [1].transform.position = initTensioForces [1].transform.position+new Vector3(initTensioForces[0].transform.localScale.x/2,0,0);
			vectorDir [1] = FinTensioForces[1].transform.position-new Vector3(FinTensioForces[1].transform.localScale.x/2,0,0)  - initTensioForces [1].transform.position;
			TensionAngle[1]=Mathf.Atan2(vectorDir[1].y,-vectorDir[1].x)*Mathf.Rad2Deg;
			arrowTensioForces[1].transform.rotation= Quaternion.AngleAxis (TensionAngle[1],new Vector3(0,0,-1));

		}
		else if(Manager.systemType == Manager.SystemType.twoPulleySystem){
			i = Manager.outputForce / 1500;
			//print (i);
			i = Mathf.Clamp (i,0f,0.2f);
			//print (i);
			arrowForces[0].transform.position = initPos[0].position;
			arrowForces[0].transform.localScale=new Vector3(i,arrowForces[0].transform.localScale.y,arrowForces[0].transform.localScale.z);

			pL = Manager.p_load / 1500;
			//print (pL);
			pL = Mathf.Clamp (pL,0f,0.2f);
			//print (pL);
			arrowForces[1].transform.position = initPos[1].position+new Vector3(0,0,-initPos[1].localScale.z/2);
			arrowForces[1].transform.localScale=new Vector3(pL,arrowForces[1].transform.localScale.y,arrowForces[1].transform.localScale.z);

			for (int i = 0; i < arrowTensioForces.Length; i++) {
				pulleyT [i] = Manager.tension [i] / 10000;
				pulleyT [i] = Mathf.Clamp (pulleyT[i],0,0.16f);

				arrowTensioForces[i].transform.localScale=new Vector3(pulleyT[i],arrowTensioForces[i].transform.localScale.y,arrowTensioForces[i].transform.localScale.z);

			}

			arrowTensioForces [0].transform.position = initTensioForces [0].transform.position-new Vector3(initTensioForces[0].transform.localScale.x/2,0,0);
			vectorDir [0] = FinTensioForces[0].transform.position+new Vector3(FinTensioForces[0].transform.localScale.x/2,0,0) - initTensioForces [0].transform.position;
			TensionAngle [0]=Mathf.Atan2(vectorDir[0].y,-vectorDir[0].x)*Mathf.Rad2Deg;
			arrowTensioForces [0].transform.rotation = Quaternion.AngleAxis (TensionAngle[0],new Vector3(0,0,-1));

			arrowTensioForces [1].transform.position = initTensioForces [1].transform.position+new Vector3(initTensioForces[1].transform.localScale.x/2,0,0);
			vectorDir [1] = FinTensioForces[1].transform.position-new Vector3(FinTensioForces[1].transform.localScale.x,0,0)  - initTensioForces [1].transform.position;
			TensionAngle[1]=Mathf.Atan2(vectorDir[1].y,-vectorDir[1].x)*Mathf.Rad2Deg;
			arrowTensioForces[1].transform.rotation= Quaternion.AngleAxis (TensionAngle[1],new Vector3(0,0,-1));

			arrowTensioForces [2].transform.position = initTensioForces [2].transform.position;
			vectorDir [2] = FinTensioForces[2].transform.position+new Vector3(FinTensioForces[2].transform.localScale.x/2,0,0)  - initTensioForces [2].transform.position;
			TensionAngle[2]=Mathf.Atan2(vectorDir[2].y,-vectorDir[2].x)*Mathf.Rad2Deg;
			arrowTensioForces[2].transform.rotation= Quaternion.AngleAxis (TensionAngle[2],new Vector3(0,0,-1));

		}
	}
}
