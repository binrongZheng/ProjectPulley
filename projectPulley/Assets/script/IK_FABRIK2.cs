using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ourEngine;

public class IK_FABRIK2 : MonoBehaviour
{
    public Transform[] joints;
    public Transform target;

	private ourVector3[] copy;		
    private float[] distances;		
    private bool done;
		
	private float threshold_distance = 0.1f;

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
                // The target is unreachable -> Posar tots en linia
				for (int i = 0; i < copy.Length-1;i++){
					float distanceToTarget = ourVector3.Distance ((ourVector3)target.position, copy [i]);
					float percentatgeToTarget = distances [i] / distanceToTarget;
					copy [i + 1] = (1 - percentatgeToTarget) * copy [i] + percentatgeToTarget * (ourVector3)target.position; //l'anterior més el teu percentatge a la distancia del target
				}

            }
            else
            {
                // The target is reachable
				while (!done)
                {
					// STAGE 1: FORWARD REACHING
					copy[copy.Length-1] = target.position;
					for (int i = copy.Length - 1; i > 0; i--) {
						ourVector3 temp = (copy [i - 1] - copy [i]).Normalize(); //agafem vector de la recta
						temp *= distances [i - 1]; //multipliquem per distancia per obtenir el tamany de vector que toca
						copy [i - 1] = copy[i] + temp;
					}

                    // STAGE 2: BACKWARD REACHING
					copy[0] = joints[0].position;
					for (int i = 0; i < copy.Length - 2; i++) {
						ourVector3 temp = (copy [i + 1] - copy [i]).Normalize();
						temp *= distances [i];
						copy [i + 1] = copy [i] + temp;
					}

					done = (ourVector3.Distance (copy [copy.Length - 1], (ourVector3)target.position) < threshold_distance);
                }
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

				//rotem amb el quaterion fet per aquest angle i eix
				Quaternion newQ;
				newQ.w = Mathf.Cos (alpha / 2);
				newQ.x = axis.x * Mathf.Sin (alpha / 2);
				newQ.y = axis.y * Mathf.Sin (alpha / 2);
				newQ.z = axis.z * Mathf.Sin (alpha / 2);

				joints [i].position = copy [i];
				joints [i].rotation = newQ * joints [i].rotation;

            }          
        }
    }

	void CalculateDistances (ourVector3[] nodes) {
		for (int i = 0; i < nodes.Length-1; i++) { //pq no peta?????!!!!!
			distances [i] = ourVector3.Distance (nodes [i], nodes [i + 1]);
		}
	}
}
