using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ourEngine;

public class RopeManager : MonoBehaviour {

    public Transform rope;
    private Transform[] realParticles;
    ourParticle[] particles;

    public float ke;
    public float kd;
    public float segmentLongitude;
    public float maxSeparation;

    // Use this for initialization
    void Start()
    {
        //Omplim l'array amb els joints de la corda per no haver-ho de fer manualment
        realParticles = new Transform[rope.childCount];        
        for (int i = 0; i < rope.childCount; i++)
        {
            realParticles[i] = rope.GetChild(i);            
        }

        //iniciem les nostra simulacio a la posicio de la corda real
        particles = new ourParticle[realParticles.Length];
        particles[0] = new ourParticle(realParticles[0].position, 1, true); //la primera esta enganxada
        for (int i = 1; i < realParticles.Length; i++) {
            particles[i] = new ourParticle(realParticles[i].position, 1, false);
        }       

    }

    // Update is called once per frame
    void Update()
    {
        CalculateSpringForces();
        UpdateSimuation();
        DistanceCorrection();
        SetRealPositions();        
    }

    void CalculateSpringForces()
    {
        for (int i = 0; i < realParticles.Length; i++)
        {
            if (i < realParticles.Length - 1) //Si te particula a la dreta calculem força dreta
            {
                ourVector3 springVector = particles[i].position - particles[i + 1].position;
                float r = springVector.GetMagnitude();

                particles[i].rightForce = -1 * (ke * (r - segmentLongitude) + kd * ourVector3.Dot((particles[i].velocity - particles[i + 1].velocity), (springVector / r))) * springVector / r;

            }
            else particles[i].rightForce = new ourVector3();

            if (i > 0) //aqui no cal else pq al ser el agarre no entrarem al solver igualment
                particles[i].leftForce = -1 * particles[i - 1].rightForce;
            
        }
    }

    void UpdateSimuation ()
    {
        //Simular posicions
        for (int i = 0; i < realParticles.Length; i++)
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

            //Simulem moviment
            particles[i].Update(Time.deltaTime);

            //Detectar colisions
            
        }
    }

    void SetRealPositions()
    {
        for (int i = 0; i < realParticles.Length; i++)
        {
            //setejem els objectes
            realParticles[i].position = particles[i].position;

            //Debug
            if (i < realParticles.Length - 1)
                Debug.DrawLine(realParticles[i].position, realParticles[i + 1].position, Color.red);
        }
    }

    void DistanceCorrection()
    {
        for (int i = 0; i < realParticles.Length; i++)
        {
            if (i > 0 && ( (particles[i].position - particles[i - 1].position).GetMagnitude() > (segmentLongitude + segmentLongitude * maxSeparation) ) )
            {
                if (i-1 == 0)
                    particles[i].position -= (particles[i].position - particles[i - 1].position).GetNormalized() * ((particles[i].position - particles[i - 1].position).GetMagnitude() - (segmentLongitude));
                else
                {
                    particles[i].position -= (particles[i].position - particles[i - 1].position).GetNormalized() * (((particles[i].position - particles[i - 1].position).GetMagnitude() - (segmentLongitude)) / 2);
                    particles[i - 1].position += (particles[i].position - particles[i - 1].position).GetNormalized() * (((particles[i].position - particles[i - 1].position).GetMagnitude() - (segmentLongitude)) / 2);
                }
            }
            
            if (i > 0 && i < realParticles.Length-1 && ((particles[i].position - particles[i + 1].position).GetMagnitude() > (segmentLongitude + segmentLongitude * maxSeparation)))
            {               
                particles[i].position -= (particles[i].position - particles[i + 1].position).GetNormalized() * (((particles[i].position - particles[i + 1].position).GetMagnitude() - (segmentLongitude)) / 2);
                particles[i + 1].position += (particles[i].position - particles[i + 1].position).GetNormalized() * (((particles[i].position - particles[i + 1].position).GetMagnitude() - (segmentLongitude)) / 2);
                
            }
        }
    }
}
