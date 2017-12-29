using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
	//public
	public enum SystemType{fixedPulley,movablePulley,twoPulleySystem}; 
	public enum PulleyType{withHook,withShackle,tailBoard}; 
	public enum SheaveDiametre{eight=8,ten=10,twelve=12,fourteen=14,eighteen=18,twenty=20,twenty_four=24}; 

	public SystemType systemType;
	public PulleyType pulleyType;
	public SheaveDiametre sheaveDiametre;
	public float boxMass;
	public float staticCoef;
	public float alpha;
	public float ropeDiametre;

	[Range (0.0f,3.0f)]
	public float inputDistance;

	//private
	float pulleyMass;
	float pulleyLimit;
	float eulerNum = 2.71828182f;
	float p_load;
	float drumFriction=222.4f;
	float beta;
	float P_Rope_Metre=27;			//27N/m
	float overHaulingFactor;
	float[] longituds;
	float[] tension=new float[3];
	int numTension;
	float outputForce;
	float MA;
	float[] pulleyForce; 
	int numPulley;
	float gravity=9.81f;
	float diferentDistanece;

	//Per moure la caixa
	private float velocity;
	private float maxY;
	public Transform load;

	// Use this for initialization
	void Start () {
		switch (systemType) {
		case SystemType.fixedPulley:
			MA = 1;
			numPulley = 0;
			numTension = 2;
			longituds = new float[]{ 3, 3 };
			break;
		case SystemType.movablePulley:
			MA = 2;
			numPulley = 1;
			numTension = 2;
			longituds = new float[]{ 3, 3 };
			break;
		case SystemType.twoPulleySystem:
			MA = 2;
			numPulley = 1;
			numTension = 3;
			longituds = new float[]{ 3, 1.5f,1.5f };

			break;
		default:
			break;
		}

		beta=180-alpha;

		setPulleyValue();

		p_load = boxMass * gravity; 

		for(int i=0;i<numPulley;i++){
			p_load += pulleyMass*gravity;
		}

		if (MA == 1) overHaulingFactor = 1.02f;
		else if (MA == 2) overHaulingFactor = 2.1f;

		 
		//CALCULEM TENSIONS
		//en els nostres 3 casos la primera tensio és equivalent a aixo
		tension[0]=p_load/MA;
		//setegem la resta
		for(int i=1;i<numTension;i++){
			//if (i == numTension - 1) {
				tension [i] = tension [i - 1] * Mathf.Pow (eulerNum,staticCoef*beta*Mathf.Deg2Rad);
				//longituds [i] += inputDistance;

			/*} else {
				tension [i] = tension [i - 1] * Mathf.Pow (eulerNum,staticCoef*180*Mathf.Deg2Rad);
				longituds [i] -= inputDistance / MA;
			}*/
		}

		//CALCULEM LONGITUDS
		if (systemType == SystemType.fixedPulley){
			longituds [0] -= inputDistance;
			longituds [1] += inputDistance;

			//oF = T2 + (l1-l2)*P_rope/m
			outputForce = tension [numTension - 1] + ( (longituds [0]-longituds[1]) * P_Rope_Metre);
		}

		//calculem la posicio final de la caixa
		maxY = load.position.y + inputDistance;

	}
	
	// Update is called once per frame
	void Update () {

		//Movem la caixa i les politges mobils aplicant la outputForce que hem calculat
		if (load.position.y <= maxY){
			velocity += (outputForce/boxMass)*Time.deltaTime/10;
			load.position += new Vector3(0, velocity * Time.deltaTime/10, 0);
		}

	}

	void setPulleyValue(){
		switch (sheaveDiametre) {
		case SheaveDiametre.eight:
			if (pulleyType == PulleyType.withHook) {
				pulleyMass = 34;
				pulleyLimit = 196133;//Newton
				ropeDiametre = 1;
			} else if (pulleyType == PulleyType.withShackle) {
				pulleyMass = 39.44f;
				pulleyLimit= 196133;
				ropeDiametre = 1;
			} else if (pulleyType == PulleyType.tailBoard) {
				pulleyMass = 19.04f;
				pulleyLimit= 196133;
				ropeDiametre = 1;
			}
			break;
		}
	}
}
