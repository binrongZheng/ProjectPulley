using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ourEngine;

public class IK_Fabrik : MonoBehaviour {

	public Transform[] joints;
	public Transform target;

	private ourVector3[] copy;		
	private float[] distances;		
	private bool done;

	private float threshold_distance = 0.1f;
	private float maxIterations = 50;

	private float angulo;
	public Transform pivot;
	private ourVector3 pivotCopy;

	void Start () {
		pivotCopy = pivot.position;
		distances = new float[joints.Length - 1];
		copy = new ourVector3[joints.Length];
	}

	void Update () {

		//copiar els joints a copy i calcular distancies
		for (int i = 0; i < joints.Length;i++){
			copy [i] = joints[i].position;
			if (i < joints.Length - 1) {
				distances [i] = (joints [i + 1].position - joints [i].position).magnitude;
			}
		}
		//Comporvar si estem en linia
		CalculateAlignment ();
		if (angulo > 0.1f) {
			ourVector3 axis = new ourVector3 (0, 1, 0);

			Quaternion newQ = Quaternion.AngleAxis (angulo, axis);

			Quaternion newRot = newQ * (Quaternion)pivot.rotation;
			pivot.rotation = newRot;			
		} //si no ho estem
		else {
			//Comprovem si hem acabat
			done = ourVector3.Distance(target.position, joints[joints.Length-1].position) < threshold_distance;

			if (!done) {
				float targetRootDist = Vector3.Distance (copy [0], target.position);

				//Mirar si ems hem passat
				if (targetRootDist > distances.Sum ()) {
					for (int i = 0; i < copy.Length - 1; i++) {
						float dist = ((ourVector3)target.position - copy [i]).GetMagnitude();
						float ratio = distances [i] / dist;

						copy [i + 1] = (1 - ratio) * copy [i] + ratio * (ourVector3)target.position;
					}
				}
				else {

					int counter = 0;
					//El target esta a l'alcanç
					while (!done && counter < maxIterations){
						counter++;

						//STAGE 1: FORWARD REACHING
						copy[copy.Length-1] = target.position;
						for (int i = copy.Length - 1; i > 0; i--) {
							ourVector3 temp = (copy [i - 1] - copy [i]).GetNormalized();
							temp = temp * distances [i - 1];
							copy [i - 1] = (ourVector3)temp + copy [i];
						}

						//STAGE 2: BACKWARD REACHING
						copy[0] = joints[0].position;
						for (int i = 0; i < copy.Length - 2; i++) {
							ourVector3 temp = (copy [i + 1] - copy [i]).GetNormalized();
							temp = temp * distances [i];
							copy [i + 1] = (ourVector3)temp + copy [i];
						}

						//tornem a calcular el done
						done = ourVector3.Distance(target.position, joints[joints.Length-1].position) < threshold_distance;
					}
				}

				//Rotem els joints
				for (int i = 0; i <= joints.Length -2 && angulo < 0.1f; i++){
					joints [i].position = copy [i];

					ourVector3 a, b;
					a = joints [i + 1].position - joints [i].position;
					b = copy [i + 1] - copy [i];

					Vector3 axis = Vector3.Cross (a, b).normalized; //Es la unica part del codi que no funciona amb el nostre vector
					//ourVector3 axis2 = ourVector3.Cross(a,b);
					//axis2 = axis2.GetNormalized();

					float cosa = ourVector3.Dot(a,b) / (a.GetMagnitude() * b.GetMagnitude());
					float sina = ourVector3.Cross (a.GetNormalized(), b.GetNormalized()).GetMagnitude();

					float alpha = Mathf.Atan2 (sina, cosa);

					ourQuaternion q = new ourQuaternion (axis.x * Mathf.Sin (alpha / 2), axis.y * Mathf.Sin (alpha / 2), axis.z * Mathf.Sin (alpha / 2), Mathf.Cos (alpha / 2));
					joints [i].position = copy [i];
					joints [i].rotation = (Quaternion)q * joints [i].rotation;

					CalculateAlignment ();
				}

			}

		}
	}

	public void CalculateAlignment () {
		//comparar vectors
		ourVector3 v1 = (ourVector3)target.position - pivotCopy;
		ourVector3 n1 = ourVector3.Cross (v1.GetNormalized(), new ourVector3 (0, 1, 0)).GetNormalized();
		ourVector3 v2 = copy [1] - pivotCopy;
		ourVector3 n2 = ourVector3.Cross (v2.GetNormalized(), new ourVector3 (0, 1, 0)).GetNormalized();

		float myCos = ourVector3.Dot (n1, n2) / (n1.GetMagnitude() * n2.GetMagnitude()); //tmb es pot no dividir i fer v1.normlaized i v2.normalized
		float mySen = ourVector3.Cross (n1.GetNormalized(), n2.GetNormalized()).GetMagnitude();
		angulo = Mathf.Atan2 (mySen, myCos);	
	}
}
