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
    
	private int numParticles;

	public Transform ropeStart;
	public Transform ropeEnd;

	LineRenderer lineRenderer;

    // Use this for initialization
    void Start()
    {
        //Omplim l'array amb els joints de la corda per no haver-ho de fer manualment
		numParticles = 20;
		for (int i = 0; i < numParticles; i++){
			GameObject newJoint = Instantiate(joint, new Vector3(rope.position.x+i*segmentLongitude, rope.position.y, rope.position.z), Quaternion.identity);
			newJoint.transform.parent = rope;
			//LineRenderer lineRenderer = newJoint.AddComponent<LineRenderer>();
			//lineRenderer.SetWidth(0, 3);
		}

        //iniciem les nostra simulacio a la posicio de la corda real
		particles = new ourParticle[rope.childCount];        
        for (int i = 1; i < numParticles-1; i++) {
			particles[i] = new ourParticle(rope.GetChild(i).position, 1, false);
        }
		particles[0] = new ourParticle(ropeStart.position, 1, true);
		particles[numParticles-1] = new ourParticle(ropeEnd.position, 1, true);

		//Agafem el component per pintar linies
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.positionCount = numParticles;

    }

    // Update is called once per frame
    void Update()
    {
		
		particles[0].position = ropeStart.position;
		particles[numParticles-1].position = ropeEnd.position;
		
       	CalculateSpringForces();
		UpdateSimuation ();
		DistanceCorrection();
		SetRealPositions ();


    }

	void OnPostRender() {
		for (int i = 0; i < numParticles; i++)
		{
			lineRenderer.SetPosition (i, particles [i].position);
			/*Vector3 pos = particles[i].position;
			Vector3 nextPos = particles[i+1].position;

			GL.Begin(GL.LINES);
			GL.Color(new Color(1f, 1f, 1f, 1f));
			GL.Vertex3(pos.x, pos.y, pos.z);
			GL.Vertex3(nextPos.x, nextPos.y, nextPos.z);
			GL.End();*/
		}

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
				r = Mathf.Round (r *100f)/100f;

                particles[i].rightForce = -1 * (ke * (r - segmentLongitude) + kd * Vector3.Dot((particles[i].velocity - particles[i + 1].velocity), (springVector / r))) * springVector / r;
				//print(" num " + i + ": " + particles[i].rightForce.z);

				//print(" num " + i + ": " + r);
            }

			//LA FORÇA PER L'ESQUERRA (a la primera no li setegem mai per tant es 0)
			if (i > 0){
				if (i == numParticles -1){ //a la ultima la tenim que calcular b pq no li hem calculat una right force a partir de la cual treure la left force
					Vector3 springVector = particles[i].position - particles[i-1].position;
					float r = springVector.magnitude;
					r = Mathf.Round (r *100f)/100f;

					particles[i].leftForce = -1 * (ke * (r - segmentLongitude) + kd * Vector3.Dot((particles[i].velocity - particles[i-1].velocity), (springVector / r))) * springVector / r;
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
            
            //Detectar colisions
			particles[i].PulleyCollision(pulley.position, 0.55f, Time.deltaTime);
		
				

            //Simulem moviment
            particles[i].Update(Time.deltaTime);
        }
    }

    void SetRealPositions()
    {
		for (int i = 0; i < numParticles; i++)
        {
            //setejem els objectes
			//print (i + " : " + particles[i].position);
			if (!float.IsNaN(particles[i].position.x) && !float.IsNaN(particles[i].position.y) && !float.IsNaN(particles[i].position.y))
				rope.GetChild(i).position = particles[i].position;
			else{
				print("SOME POSITION VALUES ARE NAN!!!");
			}

            //Debug
			if (i < numParticles - 1)
				Debug.DrawLine(rope.GetChild(i).position, rope.GetChild(i+1).position, Color.red);
        }
    }

    void DistanceCorrection()
    {
		
		for (int i = 0; i < numParticles; i++)
        {			
			
			if (i < numParticles-1 && !particles[i].isFixed){

				Vector3 distVec = (particles[i].position - particles[i + 1].position);
				distVec.x =  Mathf.Round (distVec.x * 100f)/100f;

				if (distVec.magnitude > (segmentLongitude + segmentLongitude * maxSeparation) ) //si estan massa separats
				{
					if (particles[i+1].isFixed)
						particles[i].position -= (particles[i].position - particles[i + 1].position).normalized * ((particles[i].position - particles[i + 1].position).magnitude - (segmentLongitude));
					else
					{
						Vector3 correction1 = distVec.normalized * ((distVec.magnitude - (segmentLongitude)) / 2);
						particles[i].position -= correction1;
						Vector3 correction2 = distVec.normalized * ((distVec.magnitude - (segmentLongitude)) / 2);
						particles[i + 1].position += correction2;
					}
				}
			}


			if (i > 0 && !particles[i].isFixed){

				Vector3 distVec = (particles[i].position - particles[i - 1].position);
				distVec.x =  Mathf.Round (distVec.x * 100f)/100f;

				if (distVec.magnitude > (segmentLongitude + segmentLongitude * maxSeparation) ) //si estan massa separats
				{
					if (particles[i-1].isFixed){
						particles[i].position -= (particles[i].position - particles[i - 1].position).normalized * ((particles[i].position - particles[i - 1].position).magnitude - (segmentLongitude));
					}
					else
					{
						Vector3 correction1 = distVec.normalized * ((distVec.magnitude - (segmentLongitude)) / 2);
						particles[i].position -= correction1;
						Vector3 correction2 = distVec.normalized * ((distVec.magnitude - (segmentLongitude)) / 2);
						particles[i - 1].position += correction2;
						//print (correction1.x + "," + correction1.y + "," + correction1.z + " ||| " + correction2.x + ","+ correction2.y + "," +correction2.z);
					}
				}
			}
        }
    }
}
