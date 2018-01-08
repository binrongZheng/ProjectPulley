using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
	//public
	public enum SystemType{fixedPulley,movablePulley,twoPulleySystem}; 
	public enum PulleyType{withHook,withShackle,tailBoard}; 
	public enum SheaveDiametre{eight=8,ten=10,twelve=12,fourteen=14,eighteen=18,twenty=20,twenty_four=24}; 

	public SystemType systemType;
	static public PulleyType pulleyType;
	static public SheaveDiametre sheaveDiametre;
	static public float boxMass;
	static public float staticCoef;
	static public float alpha;
	static public float ropeDiametre;

	[Range (0.0f,1.5f)]
	static public float inputDistance;

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
	float ropeLength = 10;
	float ropeLimit;

	//Per moure la caixa
	private float velocity;
	private float maxY;
	public Transform load;
	public Transform target;

	// Use this for initialization
	void Start () {
		switch (systemType) {
		case SystemType.fixedPulley:
			MA = 1;
			numPulley = 0;
			numTension = 2;
			longituds = new float[]{ 2.5f, 2.5f };
			pulleyForce = new float[1];
			break;
		case SystemType.movablePulley:
			MA = 2;
			numPulley = 1;
			numTension = 2;
			longituds = new float[]{ 2.5f, 2.5f };
			break;
		case SystemType.twoPulleySystem:
			MA = 2;
			numPulley = 1;
			numTension = 3;
			longituds = new float[]{ 2.5f, 1.25f,1.25f };

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
			//longituds [0] -= inputDistance;
			//longituds [1] += inputDistance;

			//oF = T2 + (l1-l2)*P_rope/m
			outputForce = tension [numTension - 1] + ( (load.position.y-target.position.y) * P_Rope_Metre);
			pulleyForce[0] = ((( (ropeLength/2 * P_Rope_Metre) + drumFriction) * overHaulingFactor)+tension[0])*2; //2 pq alpha es 0 i per tant el factor es aixi
		}

		//CALCULEM LONGITUDS
		if (systemType == SystemType.movablePulley){
			//longituds [0] -= inputDistance;
			//longituds [1] += inputDistance;

			//oF = T2 + (l1-l2)*P_rope/m
			outputForce = tension [numTension-1] + ( (load.position.y-target.position.y) * P_Rope_Metre);
			//pulleyForce[0] = ((( (ropeLength/2 * P_Rope_Metre) + drumFriction) * overHaulingFactor)+tension[0])*2; //2 pq alpha es 0 i per tant el factor es aixi
		}

		//calculem la posicio final de la caixa
		maxY = load.position.y + inputDistance;

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel("optionScene");
		
		if (systemType == SystemType.fixedPulley){
		//Movem la caixa i les politges mobils aplicant la outputForce que hem calculat
			if (load.position.y <= maxY && Input.GetKey(KeyCode.S)){
				target.position -= new Vector3(0, velocity * Time.deltaTime / 5, 0); 
				load.position += new Vector3(0, velocity * Time.deltaTime / 5, 0)/MA;
	            velocity += (outputForce/boxMass)*Time.deltaTime/5;		

				//Es va modificant una mica pq el pes de la corda t'ajuda
				outputForce = tension [numTension - 1] + ( (load.position.y-target.position.y) * P_Rope_Metre);
			}
		}
		if (systemType == SystemType.movablePulley){
			
			//Movem la caixa i les politges mobils aplicant la outputForce que hem calculat
			if (load.position.y <= maxY /*&& target.transform.position.y > 0*/ && Input.GetKey(KeyCode.W)){
				target.position += new Vector3(0, velocity * Time.deltaTime / 5, 0); 
				load.position += new Vector3(0, velocity * Time.deltaTime / 5, 0)/MA;
				velocity += (outputForce/boxMass)*Time.deltaTime/5;		

				//Es va modificant una mica pq el pes de la corda t'ajuda
				outputForce = tension [numTension - 1] + ( (load.position.y-target.position.y) * P_Rope_Metre);
			}
		}

	}

	void OnGUI () {

		//DADES DE RESULTATS
		GUI.contentColor = Color.yellow;
		GUI.Box(new Rect(5, 10, 600, 100), "");

		GUI.Label(new Rect(10, 10, 800, 20), "Força minima per aixecar la càrrega : " + outputForce);
		GUI.Label(new Rect(10, 30, 800, 20), "LIMITS");
		if(systemType == SystemType.fixedPulley)
		GUI.Label(new Rect(10, 45, 800, 20), "Força que està suportant la politja : " + pulleyForce[0] + ", el limit d'aquest tipus de politja és " + pulleyLimit + "kN");
		for (int i = 0; i < numTension; i++){
			if (tension[i] > ropeLimit*1000)
				GUI.Label(new Rect(10, 60 + 15*i, 800, 20),"La tensio " + i + " es massa alta, la corda es trenca!!");
			else
				GUI.Label(new Rect(10, 60 + 15*i, 800, 20), "La tensio " + i + " es de " + tension[i] + ", el limit d'una corda d'aquestes característiques es de " + ropeLimit + "kN");			
		}

		//Distancia estirada
		float pulledDist = Mathf.Round((2.17f - target.position.y)*100f)/100f;
		
		GUI.Label(new Rect(10, 200, 300, 20), "Has estirat " + pulledDist + " de " + inputDistance + "m");

		//INFORMACIO GENERAL
		GUI.Box(new Rect(5, Screen.height-10, 300, -170), "");
		GUI.Label(new Rect(10, Screen.height-180, 300, 20), "DADES GENERALS");
		GUI.Label(new Rect(10, Screen.height-160, 300, 20), "Aquest sistema es : " + systemType);
		GUI.Label(new Rect(10, Screen.height-140, 300, 20), "- La massa que volem aixecar es de " + boxMass + "kg");
		GUI.Label(new Rect(10, Screen.height-120, 300, 20), "- El diametre de la corda es de " + ropeDiametre + " polzades");
		GUI.Label(new Rect(10, Screen.height-100, 300, 20), "- La corda pesa " + P_Rope_Metre + "N/m");
		GUI.Label(new Rect(10, Screen.height-80, 300, 20), "- El limit de pes d'aquesta corda es de " + ropeLimit +"kN");			
		GUI.Label(new Rect(10, Screen.height-60, 300, 20), "- La politja es de tipus " + pulleyType + " i pesa " + pulleyMass +"kg");			
		GUI.Label(new Rect(10, Screen.height-40, 300, 20), "- El limit de pes d'aquesta politja es de " + pulleyLimit +"kN");
	}

	void setPulleyValue(){
		switch (sheaveDiametre) {
		case SheaveDiametre.eight:
			if (pulleyType == PulleyType.withHook) {
				pulleyMass = 34;
				pulleyLimit = 196.1f;//kN
				ropeDiametre = 1;
				ropeLimit = 447.38f;//kN
			} else if (pulleyType == PulleyType.withShackle) {
				pulleyMass = 39.44f;
				pulleyLimit= 196133;
				ropeDiametre = 1;
				ropeLimit = 447.38f;//kN
			} else if (pulleyType == PulleyType.tailBoard) {
				pulleyMass = 19.04f;
				pulleyLimit= 196133;
				ropeDiametre = 1;
				ropeLimit = 447.38f;//kN			
			}
			break;
		}
	}
}
