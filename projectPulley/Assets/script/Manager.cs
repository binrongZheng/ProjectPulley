using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
	//public
	public enum SystemType{fixedPulley,movablePulley,twoPulleySystem}; 
	public enum PulleyType{withHook,withShackle,tailBoard}; 
	public enum SheaveDiametre{eight=8,ten=10,twelve=12,fourteen=14,eighteen=18}; 

	public SystemType systemType;
	static public PulleyType pulleyType;
	static public SheaveDiametre sheaveDiametre;
	static public float boxMass;
	static public float staticCoef;
	static public float alpha;
	static public float ropeDiametre;
	static public float angleFactor;

	[Range (0.0f,1.5f)]
	static public float inputDistance;

	//private
	float pulleyMass;
	float pulleyLimit;
	float eulerNum = 2.71828182f;
	float p_load;
	float drumFriction=222.4f;
	float beta;
	float P_Rope_Metre;	
	float overHaulingFactor;
	//float[] longituds;
	float[] tension=new float[3];
	int numTension;
	float outputForce;
	float MA;
	float[] pulleyForce; 
	int numPulley; //Aquestes nomes son les mobils
	float gravity=9.81f;
	float ropeLength = 10;
	float ropeLimit;
	float incialTargetPos;

	//Per moure la caixa
	private float velocity;
	private float maxY;
	public Transform load;
	public Transform target;

	private GUIStyle guiStyle = new GUIStyle();

	// Use this for initialization
	void Start () {
		switch (systemType) {
		case SystemType.fixedPulley:
			MA = 1;
			numPulley = 0;
			numTension = 2;
			//longituds = new float[]{ 2.5f, 2.5f };
			pulleyForce = new float[1];
			break;
		case SystemType.movablePulley:
			MA = 2;
			numPulley = 1;
			numTension = 2;
			//longituds = new float[]{ 2.5f, 2.5f };
			pulleyForce = new float[1];
			break;
		case SystemType.twoPulleySystem:
			MA = 2;
			numPulley = 1;
			numTension = 3;
			pulleyForce = new float[2];
			//longituds = new float[]{ 2.5f, 1.25f,1.25f };
			break;
		default:
			break;
		}

		beta=180-alpha;

		setPulleyValue();

		p_load = boxMass * gravity; 
		//Sumem la massa de les politges mobils
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
			
			tension [i] = tension [i - 1] * Mathf.Pow (eulerNum,staticCoef*beta*Mathf.Deg2Rad);
		}

		incialTargetPos = target.position.y;
		//CALCULEM FORCES FINALS I DE POLITJA
		if (systemType == SystemType.fixedPulley){

			//oF = T2 + (l1-l2)*P_rope/m
			outputForce = tension [numTension - 1] + ( (incialTargetPos - target.position.y)*2 * P_Rope_Metre);
			pulleyForce[0] = ((( (ropeLength/2 * P_Rope_Metre) + drumFriction) * overHaulingFactor)+tension[0])*angleFactor; //2 pq alpha es 0 i per tant el factor es aixi

		}
		if (systemType == SystemType.movablePulley){
			
			//oF = T2 + (l2)*P_rope/m
			outputForce = tension [numTension-1] + ( (target.position.y-2) * P_Rope_Metre); //2 es l'alçada a la que esta la politja en un principi
			pulleyForce[0] = ((( (ropeLength/2 * P_Rope_Metre) + drumFriction) * overHaulingFactor)+tension[0])*angleFactor; //2 pq alpha es 0 i per tant el factor es aixi
		}
		if (systemType == SystemType.twoPulleySystem){

			//oF = T2 + (l2)*P_rope/m
			outputForce = tension [numTension-1] + ( -((incialTargetPos - target.position.y) + (incialTargetPos - target.position.y)/2)  * P_Rope_Metre); //segon numero és l'alçada de la segona politja
			pulleyForce[0] = tension[0]*angleFactor;
			float ropeInPulley = (3.6f-(target.position.y-inputDistance)) + ((3.6f-load.position.y)+inputDistance/MA); //Tot el tros de corda que esta repenjat a aquesta politja
			pulleyForce[1] = ((( (ropeInPulley/2 * P_Rope_Metre) + drumFriction) * overHaulingFactor)+tension[0])*angleFactor; //2 pq alpha es 0 i per tant el factor es aixi

		}

		//calculem la posicio final de la caixa
		maxY = load.position.y + inputDistance/MA;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (systemType == SystemType.fixedPulley){
		//Movem la caixa i les politges mobils aplicant la outputForce que hem calculat
			if (load.position.y < maxY && Input.GetKey(KeyCode.S)){
				target.position -= new Vector3(0, velocity * Time.deltaTime / 5, 0); 
				load.position += new Vector3(0, velocity * Time.deltaTime / 5, 0)/MA;
	            velocity += (outputForce/boxMass)*Time.deltaTime/5;		

				//Es va modificant una mica pq el pes de la corda t'ajuda
				outputForce = tension [numTension - 1] + ( (target.position.y-load.position.y) * P_Rope_Metre);
			}
		}
		else if (systemType == SystemType.movablePulley){
			
			//Movem la caixa i les politges mobils aplicant la outputForce que hem calculat
			if (load.position.y < maxY && Input.GetKey(KeyCode.W)){
				target.position += new Vector3(0, velocity * Time.deltaTime / 5, 0); 
				load.position += new Vector3(0, velocity * Time.deltaTime / 5, 0)/MA;
				velocity += (outputForce/boxMass)*Time.deltaTime/5;		

				//Es va modificant una mica pq el pes de la corda t'ajuda
				outputForce = tension [numTension - 1] + ( (target.position.y-load.position.y) * P_Rope_Metre);
			}
		}
		else if (systemType == SystemType.twoPulleySystem){

			//Movem la caixa i les politges mobils aplicant la outputForce que hem calculat
			if (load.position.y < maxY && Input.GetKey(KeyCode.S)){
				target.position -= new Vector3(0, velocity * Time.deltaTime / 5, 0); 
				load.position += new Vector3(0, velocity * Time.deltaTime / 5, 0)/MA;
				velocity += (outputForce/boxMass)*Time.deltaTime/5;		

				//Es va modificant una mica pq el pes de la corda t'ajuda
				outputForce = tension [numTension - 1] + ( ((incialTargetPos - target.position.y) + (incialTargetPos - target.position.y)/2) * P_Rope_Metre);
			}
		}

		//Restart i go to menu
		if (Input.GetKeyDown (KeyCode.Escape))
			SceneManager.LoadScene("optionScene");
		if(Input.GetKeyDown(KeyCode.Space))
			Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
		


	}

	void OnGUI () {

		//DADES DE RESULTATS
		GUI.contentColor = Color.yellow;
			GUI.Box(new Rect(5, 10, 550, 140), "");

		GUI.Label(new Rect(10, 10, 800, 20), "Minimum force to lift the rope : " + outputForce);
		GUI.Label(new Rect(10, 30, 800, 20), "LIMITS");
		for (int i = 0; i < numTension; i++){
			if (tension[i] > ropeLimit*1000)
				GUI.Label(new Rect(10, 45 + 15*i, 800, 20),"Tension " + i + " is too high, the rope breaks!!");
			else
				GUI.Label(new Rect(10, 45 + 15*i, 800, 20), "The tension " + i + " is " + tension[i] + ", the rope limit is " + ropeLimit + "kN");			
		}

		if (systemType == SystemType.twoPulleySystem) {
			GUI.Label (new Rect (10, 100, 800, 100), "Force that the first pulley is holding : " + pulleyForce [0] + ", the limit of this type of pulley is " + pulleyLimit + "kN");
			GUI.Label (new Rect (10, 120, 800, 100), "Force that the second pulley is holding : " + pulleyForce [1] + ", the limit of this type of pulley is " + pulleyLimit + "kN");
		}
		else
			GUI.Label(new Rect(10, 90, 800, 100), "Force that the pulley is holding : " + pulleyForce[0] + ", the limit of this type of pulley is " + pulleyLimit + "kN");

		//Distancia estirada
		float pulledDist = Mathf.Round(Mathf.Abs(incialTargetPos - target.position.y)*100f)/100f;

		GUI.contentColor = Color.black;

		guiStyle.fontSize = 20;
		GUI.Label(new Rect(10, Screen.height/2, 300, 20), "You pulled " + pulledDist + " of " + inputDistance + "m", guiStyle);
		guiStyle.fontSize = 18;
		GUI.Label(new Rect(Screen.width - 250, Screen.height-20, 300, 20), "Press 'esc' to go to menu", guiStyle);
		GUI.Label(new Rect(Screen.width - 250, Screen.height-40, 300, 20), "Press 'space' to restart scene", guiStyle);
		GUI.contentColor = Color.yellow;

		//INFORMACIO GENERAL
		GUI.Box(new Rect(5, Screen.height-10, 300, -170), "");
		GUI.Label(new Rect(10, Screen.height-160, 300, 20), "GENERAL DATA");
		GUI.Label(new Rect(10, Screen.height-140, 300, 20), "This system is : " + systemType);
		GUI.Label(new Rect(10, Screen.height-120, 300, 20), "- We are lifting a mass of " + boxMass + "kg");
		GUI.Label(new Rect(10, Screen.height-100, 300, 20), "- The rope diametre is " + ropeDiametre + " inches");
		GUI.Label(new Rect(10, Screen.height-80, 300, 20), "- The rope weights " + P_Rope_Metre + "N/m");					
		GUI.Label(new Rect(10, Screen.height-60, 300, 20), "- This pulley is '" + pulleyType + "' and weights " + pulleyMass +"kg");
		GUI.Label(new Rect(10, Screen.height-40, 300, 20), "- The pulley has a diametre of '" + sheaveDiametre + "' inches");

	}

	void setPulleyValue(){
		switch (sheaveDiametre) {
		case SheaveDiametre.eight:
			if (pulleyType == PulleyType.withHook) {
				pulleyMass = 34;								
			} else if (pulleyType == PulleyType.withShackle) {				
				pulleyMass = 39.44f;							
			} else if (pulleyType == PulleyType.tailBoard) {				
				pulleyMass = 19.05f;									
			}
			pulleyLimit = 196.1f;//kN	
			break;		
		case SheaveDiametre.ten:
			if (pulleyType == PulleyType.withHook) {				
				pulleyMass = 40.37f;								
			} else if (pulleyType == PulleyType.withShackle) {				
				pulleyMass = 45.81f;						
			} else if (pulleyType == PulleyType.tailBoard) {				
				pulleyMass = 24.95f;								
			}
			pulleyLimit = 196.1f;//kN	
		break;
		case SheaveDiametre.twelve:
			if (pulleyType == PulleyType.withHook) {				
				pulleyMass = 46.72f;								
			} else if (pulleyType == PulleyType.withShackle) {				
				pulleyMass = 52.16f;						
			} else if (pulleyType == PulleyType.tailBoard) {				
				pulleyMass = 31.75f;								
			}
			pulleyLimit = 196.1f;//kN	
			break;
		case SheaveDiametre.fourteen:
			if (pulleyType == PulleyType.withHook) {				
				pulleyMass = 55.79f;								
			} else if (pulleyType == PulleyType.withShackle) {				
				pulleyMass = 61.24f;						
			} else if (pulleyType == PulleyType.tailBoard) {				
				pulleyMass = 40.82f;								
			}
			pulleyLimit = 196.1f;//kN	
			break;
		case SheaveDiametre.eighteen:
			if (pulleyType == PulleyType.withHook) {				
				pulleyMass = 108.86f;								
			} else if (pulleyType == PulleyType.withShackle) {				
				pulleyMass = 117.93f;						
			} else if (pulleyType == PulleyType.tailBoard) {				
				pulleyMass = 74.84f;								
			}
			pulleyLimit = 245.2f;//kN
			break;
		}

		//Aixo nomes depen de la corda
		if (ropeDiametre == 1){
			ropeLimit = 440.32f;//kN
			P_Rope_Metre = 2.5f; //N/m
		}
		if (ropeDiametre == 1/8){
			ropeLimit = 554.08f;//kN -- AQUEST CANVIA
			P_Rope_Metre = 3.17f; //N/m --AQUEST CANVIA
		}

	}
}
