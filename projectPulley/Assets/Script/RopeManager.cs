using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ourEngine;

public class RopeManager : MonoBehaviour {

    public Transform rope;
	public GameObject joint;
    private Transform[] realParticles;
    ourParticle[] particles;

    public float ke;
    public float kd;
    public float segmentLongitude;
    public float maxSeparation;

    public Transform pulley;
    
	public int RopeLength;
	private int numParticles;

    // Use this for initialization
    void Start()
    {
        //Omplim l'array amb els joints de la corda per no haver-ho de fer manualment
        /*realParticles = new Transform[rope.childCount];        
        for (int i = 0; i < rope.childCount; i++)
        {
            realParticles[i] = rope.GetChild(i);            
        }*/
		
		//numParticles = (int)(RopeLength/segmentLongitude);
		numParticles = 40;
		for (int i = 0; i < numParticles; i++){
			GameObject newJoint = Instantiate(joint, new Vector3(rope.position.x, rope.position.y, rope.position.z+i*segmentLongitude), Quaternion.identity);
			newJoint.transform.parent = rope;
		}

        //iniciem les nostra simulacio a la posicio de la corda real
		particles = new ourParticle[rope.childCount];        
        for (int i = 0; i < numParticles; i++) {
			particles[i] = new ourParticle(rope.GetChild(i).position, 1, false);
			particles[i].tooSeparated = true;
        }       
		particles[0].tooSeparated = false;

    }

    // Update is called once per frame
    void Update()
    {
       	CalculateSpringForces();
		UpdateSimuation ();
		DistanceCorrection ();
		SetRealPositions ();

		//int a = 8;
		//int b = 11;
		//print(particles[a].rightForce.x + "," + particles[a].rightForce.y + "," + particles[a].rightForce.z + " |||| " + particles[b].leftForce.x + "," + particles[b].leftForce.y + "," + particles[b].leftForce.z);

    }

    void CalculateSpringForces()
    {
        for (int i = 0; i < numParticles; i++)
        {
			//LA FORÇA PER LA DRETA (a la ultima no li setegem mai per tant es 0)
            if (i < numParticles - 1)
            {
				
                Vector3 springVector = particles[i].position - particles[i + 1].position;
				float r = springVector.magnitude;
				r = Mathf.Round (r *10f)/10f;

                particles[i].rightForce = -1 * (ke * (r - segmentLongitude) + kd * ourVector3.Dot((particles[i].velocity - particles[i + 1].velocity), (springVector / r))) * springVector / r;
				//print(" num " + i + ": " + particles[i].rightForce.z);

				//print(" num " + i + ": " + r);
            }

			//LA FORÇA PER L'ESQUERRA (a la primera no li setegem mai per tant es 0)
			if (i > 0){
				if (i == numParticles -1){ //a la ultima la tenim que calcular b pq no li hem calculat una right force a partir de la cual treure la left force
					ourVector3 springVector = particles[i].position - particles[i-1].position;
					float r = springVector.GetMagnitude();
					r = Mathf.Round (r *10f)/10f;

					particles[i].leftForce = -1 * (ke * (r - segmentLongitude) + kd * ourVector3.Dot((particles[i].velocity - particles[i-1].velocity), (springVector / r))) * springVector / r;
				}
				else  particles[i].leftForce = -1 * particles[i - 1].rightForce;
			}
			
            
        }
    }

    void UpdateSimuation ()
    {
        //Simular posicions
		for (int i = 0; i < numParticles; i++)
        {
            /*
            //actualitzem la simulacio
            if (i > 0 && i < realParticles.Length-1)
            {
                particles[i].CalculateStringForces(particles[i - 1], particles[i + 1], ke, kd, segmentLongitude);                
            }
            else if (i == 0)
                particles[i].CalculateStringForces(null, particles[i + 1], ke, kd, segmentLongitude);
            else if (i == realParticles.Length-1)
                particles[i].CalculateStringForces(particles[i - 1], null, ke, kd, segmentLongitude);
            */

            //Detectar colisions
            particles[i].PulleyCollision(pulley.position, 1.005f, Time.deltaTime);

            //Simulem moviment
            particles[i].Update(Time.deltaTime);
        }
    }

    void SetRealPositions()
    {
		for (int i = 0; i < numParticles; i++)
        {
            //setejem els objectes
			rope.GetChild(i).position = particles[i].position;

            //Debug
			if (i < numParticles - 1)
				Debug.DrawLine(rope.GetChild(i).position, rope.GetChild(i+1).position, Color.red);
        }
    }

    void DistanceCorrection()
    {
		for (int i = 0; i < numParticles; i++)
        {			

			/*if (i < numParticles-1 && (particles[i].position - particles[i + 1].position).magnitude > (segmentLongitude + segmentLongitude * maxSeparation)  ) //si estan massa separats
			{
				//if (particles[i+1].isFixed)
					//particles[i].position -= (particles[i].position - particles[i + 1].position).normalized * ((particles[i].position - particles[i + 1].position).magnitude - (segmentLongitude));
				//else
				//{
					Vector3 correction1 = (particles[i].position - particles[i + 1].position).normalized * (((particles[i].position - particles[i + 1].position).magnitude - (segmentLongitude)) / 2);
					particles[i].position -= correction1;
					Vector3 correction2 = (particles[i].position - particles[i + 1].position).normalized * (((particles[i].position - particles[i + 1].position).magnitude - (segmentLongitude)) / 2);
					particles[i + 1].position += correction2;
				//}
			}*/

			if (i>0 && (particles[i].position - particles[i - 1].position).magnitude > (segmentLongitude + segmentLongitude * maxSeparation) && !particles[i].tooSeparated ) //si estan massa separats
			{
				//if (particles[i-1].isFixed){
				//particles[i].position -= (particles[i].position - particles[i - 1].position).normalized * ((particles[i].position - particles[i - 1].position).magnitude - (segmentLongitude));
				//}
				//else
				//{
					

					Vector3 correction1 = (particles[i].position - particles[i - 1].position).normalized * (((particles[i].position - particles[i - 1].position).magnitude - (segmentLongitude))/2);
					particles[i].position -= correction1;
					Vector3 correction2 = (particles[i].position - particles[i - 1].position).normalized * (((particles[i].position - particles[i - 1].position).magnitude - (segmentLongitude))/2);
					particles[i - 1].position += correction2;
					//print (correction1.x + "," + correction1.y + "," + correction1.z + " ||| " + correction2.x + ","+ correction2.y + "," +correction2.z);
				//}
			}

        }
    }
}
