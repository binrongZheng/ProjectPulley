using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlScene : MonoBehaviour {


	public Dropdown system_Type; 
	public Dropdown pulley_Type; 
	public Dropdown shave_diametre;
	public Dropdown rope_Diametre;
	public Slider input_dist_Slide;
	public Text input_dist_Text;
	public Image input_dist_Image;
	public Button button;
	public InputField box_Mass;
	public InputField static_Coef;
	public InputField alpha_Angle;



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
		if(box_Mass.text!=""&&static_Coef.text!=""&&alpha_Angle.text!=""){
			if(Manager.inputDistance!=0){
				if(system_Type.value==0)
					Application.LoadLevel("finalScene");
				if(system_Type.value==1)
					Application.LoadLevel("movablePulley");
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

		input_dist_Text.text = input_dist_Slide.value.ToString("F1")+" m";

		Manager.inputDistance = (float)System.Math.Round(input_dist_Slide.value,2);
		if(input_dist_Slide.value==0)	input_dist_Image.color=Color.red;
		else {
			input_dist_Image.color=Color.green;
		}


		if(box_Mass.text!=""){
			Manager.boxMass = float.Parse( box_Mass.text);
		}
		if(static_Coef.text!=""){
			Manager.staticCoef = float.Parse( static_Coef.text);
		}
		if(alpha_Angle.text!=""){
			Manager.alpha = float.Parse(alpha_Angle.text);
		}

	}
}
