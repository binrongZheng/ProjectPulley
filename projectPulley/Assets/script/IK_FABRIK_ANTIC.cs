using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ourEngine;

public class IK_FABRIK_ANTIC : MonoBehaviour
{
    public Transform[] joints;
    public Transform target;

	private ourVector3[] copy;		
    private float[] distances;		
    private bool done;
		
	private float threshold_distance = 0.1f;
	private float maxIterations = 50;
	private float currInterations = 0;

    void Start()
    {
        distances = new float[joints.Length - 1];			
		copy = new ourVector3[joints.Length];			
    }

    void Update()
    {
        // Copy the joints positions to work with
		for (int i = 0; i< joints.Length;i++){
			copy [i] = joints [i].position;
		}
		//calcular distancies
		CalculateDistances(copy);

		//Si encara no ens hem acostat prou al target començem tot el procés
		done = (ourVector3.Distance (copy [copy.Length - 1], (ourVector3)target.position) < threshold_distance);
        if (!done)
        {
			float targetRootDist = ourVector3.Distance(copy[0], target.position);


            // Update joint positions
            if (targetRootDist > distances.Sum())
            {
				print ("Target unreachable");
                // The target is unreachable -> Posar tots en linia
				for (int i = 0; i < copy.Length - 1; i++) {
					float distanceToTarget = ourVector3.Distance ((ourVector3)target.position, copy [i]);
					float percentatgeToTarget = distances [i] / distanceToTarget;
					copy [i + 1] = (1 - percentatgeToTarget) * copy [i] + percentatgeToTarget * (ourVector3)target.position; //l'anterior més el teu percentatge a la distancia del target


					//CREEM EL PLA

					//vector des de l'anterior a la nova pos
					/*Vector3 joint2Copy = new Vector3 ();

					//Fem els dos vectors
					Vector3 planeV1 = new Vector3 ();
					planeV1 = (joints [i - 1].position - joints [i].position);
					joint2Copy = ((Vector3)copy [i] - joints [i - 1].position);							

					Vector3 planeV2 = new Vector3 ();
					if (i < 3) {
						planeV2 = (joints [i + 1].position - joints [i].position);
					} else {
						planeV2 = new Vector3 (0, -1, 0);
					}

					//Treiem la normal
					Vector3 planeN = Vector3.Cross (planeV1, planeV2).normalized;

					//Comprovem si esta en el pla
					bool inPlane = Mathf.Abs (Vector3.Dot (planeN, joint2Copy)) < 0.001;

					//Si no esta al pla
					if (!inPlane) {

						//projectem aquest vector a la normal del pla
						float dProd = Vector3.Dot (planeN, joint2Copy);
						Vector3 vertComponent = planeN * dProd;

						//posem copy[i] al pla verticalment no seguint el cercle
						Vector3 targetPos = (Vector3)copy [i] - vertComponent;

						//Trobem vector direccio cap a la projeccio del copy en el pla
						Vector3 joint2Target = (targetPos - joints [i - 1].position).normalized;

						//Posem el copy a la nova pos en el pla
						copy [i] = joints [i - 1].position + joint2Target * joint2Copy.magnitude;
					}*/
				}
			
            }
            else
            {
                // The target is reachable
				while (!done && currInterations < maxIterations)
                {
					// STAGE 1: FORWARD REACHING
					copy[copy.Length-1] = target.position;
					for (int i = copy.Length - 1; i > 0; i--) {
						ourVector3 temp = (copy [i - 1] - copy [i]).Normalize(); //agafem vector de la recta
						temp *= distances [i - 1]; //multipliquem per distancia per obtenir el tamany de vector que toca
						copy [i - 1] = copy[i] + temp;					

					}

                    // STAGE 2: BACKWARD REACHING
					copy[1] = joints[1].position;
					for (int i = 1; i < copy.Length - 2; i++) {
						ourVector3 temp = (copy [i + 1] - copy [i]).Normalize();
						temp *= distances [i];
						copy [i + 1] = copy [i] + temp;


					}

					//STAGE 3: AXIS CORRECTION
					/*for (int i = 1; i < 5 ; i++){
						//CREEM EL PLA

						//vector des de l'anterior a la nova pos
						Vector3 joint2Copy = new Vector3();

						//Fem els dos vectors
						Vector3 planeV1 = new Vector3();
						planeV1 = (joints [i - 1].position - joints [i].position);
						joint2Copy = ((Vector3)copy [i] - joints [i - 1].position);							

						Vector3 planeV2 = new Vector3();
						if (i < 3) {
							planeV2 = (joints [i + 1].position - joints [i].position);
						} else {
							planeV2 = new Vector3 (0, -1, 0);
						}

						//Treiem la normal
						Vector3 planeN = Vector3.Cross (planeV1, planeV2).normalized;

						//Comprovem si esta en el pla
						bool inPlane = Mathf.Abs (Vector3.Dot (planeN, joint2Copy)) < 0.001;

						//Si no esta al pla
						if (!inPlane) {

							//projectem aquest vector a la normal del pla
							float dProd = Vector3.Dot (planeN, joint2Copy);
							Vector3 vertComponent = planeN * dProd;

							//posem copy[i] al pla verticalment no seguint el cercle
							Vector3 targetPos = (Vector3)copy[i] - vertComponent;

							//Trobem vector direccio cap a la projeccio del copy en el pla
							Vector3 joint2Target = (targetPos - joints[i-1].position).normalized;

							//Posem el copy a la nova pos en el pla
							copy [i] = joints [i - 1].position + joint2Target * joint2Copy.magnitude;

						}

					}*/

					done = (ourVector3.Distance (copy [copy.Length - 1], (ourVector3)target.position) < threshold_distance);
					currInterations++;
                }

				currInterations = 0;
            }

            // Update original joint rotations
            for (int i = 0; i <= joints.Length - 2; i++)
            {
				//trobar els dos vectors
				ourVector3 v1 = joints [i+1].position - joints [i].position;
				ourVector3 v2 = copy[i+1] - copy[i];

				//fem cross per treure eix			
				ourVector3 axis = ourVector3.Cross(v1, v2).Normalize();

				//dot product ens donara l'angle a traves del cosinus
				float myCos = ourVector3.Dot(v1,v2)/(v1.GetMagnitude()*v2.GetMagnitude()); //tmb es pot no dividir i fer v1.normlaized i v2.normalized
				float mySen = ourVector3.Cross(v1.Normalize(), v2.Normalize()).GetMagnitude();
				float alpha = Mathf.Atan2 (mySen, myCos);

				//creem el quaternion amb aquesta nova rotacio
				ourQuaternion newQ = new ourQuaternion(
					axis.x * Mathf.Sin (alpha / 2),
					axis.y * Mathf.Sin (alpha / 2),
					axis.z * Mathf.Sin (alpha / 2),
					Mathf.Cos (alpha / 2)
				);

				//apliquem rotacio
				joints [i].position = copy [i];
				ourQuaternion newRot = newQ * (ourQuaternion)joints [i].rotation;
				joints [i].rotation = newRot;

            }          
        }
    }

	void CalculateDistances (ourVector3[] nodes) {
		for (int i = 0; i < nodes.Length-1; i++) { 
			distances [i] = ourVector3.Distance (nodes [i], nodes [i + 1]);
		}
	}
}
