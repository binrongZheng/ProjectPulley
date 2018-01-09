using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlScene : MonoBehaviour {


	public Dropdown system_Type; 
	public Dropdown pulley_Type; 
	public Dropdown shave_diametre;
	public Dropdown rope_Diametre;
	public Dropdown static_Coef;
	public Dropdown alpha;
	public Slider input_dist_Slide;
	public Text input_dist_Text;
	public Slider alpha_Angle_Slide;
	//public Text alpha_Angle_Text;
	public Image input_dist_Image;
	public Button button;
	public InputField box_Mass;
	//public InputField static_Coef;
	//public InputField alpha_Angle;



	// Use this for initialization
	void Start () {
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	
	// Update is called once per frame
	void Update () {
		passInputInf();
	}
	void TaskOnClick()
	{
		if(box_Mass.text!=""/*&&static_Coef.text!=""&&alpha_Angle.text!=""*/){
			if(Manager.inputDistance!=0){
				if(system_Type.value==0)
					Application.LoadLevel("fixedPulley");
				if(system_Type.value==1)
					Application.LoadLevel("movablePulley");
				if(system_Type.value==2)
					Application.LoadLevel("twoPulley");
			}
		}
	}
	void passInputInf(){
		switch(pulley_Type.value){
		case 0:
			Manager.pulleyType=Manager.PulleyType.withHook;
			break;
		case 1:
			Manager.pulleyType=Manager.PulleyType.withShackle;
			break;
		case 2:
			Manager.pulleyType=Manager.PulleyType.tailBoard;
			break;
		}
		switch(shave_diametre.value){
		case 0:
			Manager.sheaveDiametre=Manager.SheaveDiametre.eight;
			break;
		case 1:
			Manager.sheaveDiametre=Manager.SheaveDiametre.ten;
			break;
		case 2:
			Manager.sheaveDiametre=Manager.SheaveDiametre.twelve;
			break;
		case 3:
			Manager.sheaveDiametre=Manager.SheaveDiametre.fourteen;
			break;
		case 4:
			Manager.sheaveDiametre=Manager.SheaveDiametre.eighteen;
			break;
		}
		switch(rope_Diametre.value){
		case 0:
			Manager.ropeDiametre=1;
			break;
		case 1:
			Manager.ropeDiametre=1/8;
			break;		
		}
		switch(static_Coef.value){
		case 0:
			Manager.staticCoef = 0.16f;
			break;
		case 1:
			Manager.staticCoef = 0.3f;
			break;
		case 2:
			Manager.staticCoef = 0.2f;
			break;	
		}
		switch (alpha.value){
			case 0 : Manager.alpha = 0; Manager.angleFactor = 2;break;
			case 1 : Manager.alpha = 10; Manager.angleFactor = 1.99f;break;
			case 2 : Manager.alpha = 20; Manager.angleFactor = 1.97f;break;
			case 3 : Manager.alpha = 30; Manager.angleFactor = 1.93f;break;
			case 4 : Manager.alpha = 40; Manager.angleFactor = 1.87f;break;
			case 5 : Manager.alpha = 50; Manager.angleFactor = 1.81f;break;
			case 6 : Manager.alpha = 60; Manager.angleFactor = 1.73f;break;
			case 7 : Manager.alpha = 70; Manager.angleFactor = 1.64f;break;
			case 8 : Manager.alpha = 80; Manager.angleFactor = 1.53f;break;
			case 9 : Manager.alpha = 90; Manager.angleFactor = 1.41f;break;
			case 10 : Manager.alpha = 100; Manager.angleFactor = 1.29f;break;
			case 11 : Manager.alpha = 110; Manager.angleFactor = 1.15f;break;
			case 12 : Manager.alpha = 120; Manager.angleFactor = 1;break;
			case 13 : Manager.alpha = 130; Manager.angleFactor = 0.84f;break;
			case 14 : Manager.alpha = 140; Manager.angleFactor = 0.68f;break;
			case 15 : Manager.alpha = 150; Manager.angleFactor = 0.52f;break;
			case 16 : Manager.alpha = 160; Manager.angleFactor = 0.35f;break;
			case 17 : Manager.alpha = 170; Manager.angleFactor = 0.17f;break;
			case 18 : Manager.alpha = 180; Manager.angleFactor = 0;break;
		}

		input_dist_Text.text = input_dist_Slide.value.ToString("F1")+" m";
		Manager.inputDistance = (float)System.Math.Round(input_dist_Slide.value,2);
		if(input_dist_Slide.value==0)	input_dist_Image.color=Color.red;
		else {
			input_dist_Image.color=Color.green;
		}

		//alpha_Angle_Text.text = alpha_Angle_Slide.value.ToString("F1")+" º";
		//Manager.alpha = (float)System.Math.Round(alpha_Angle_Slide.value,2);


		if(box_Mass.text!=""){
			Manager.boxMass = float.Parse( box_Mass.text);
		}
		/*if(static_Coef.text!=""){
			Manager.staticCoef = float.Parse( static_Coef.text);
		}*/
		/*if(alpha_Angle.text!=""){
			Manager.alpha = float.Parse(alpha_Angle.text);
		}*/

	}
}
