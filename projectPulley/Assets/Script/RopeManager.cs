using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ourEngine;

public class RopeManager : MonoBehaviour {

    public Transform[] realParticles;
    ourParticle[] particles;

    public float ke;
    public float kd;
    public float segmentLongitude;

    // Use this for initialization
    void Start()
    {
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
        //Simular posicions
        for (int i = 0; i < realParticles.Length; i++)
        {
            //actualitzem la simulacio
            if (i > 0 && i < realParticles.Length-1)
            {
                particles[i].CalculateStringForces(particles[i - 1], particles[i + 1], ke, kd, segmentLongitude);                
            }
            else if (i == 0)
                particles[i].CalculateStringForces(null, particles[i + 1], ke, kd, segmentLongitude);
            else if (i == realParticles.Length-1)
                particles[i].CalculateStringForces(particles[i - 1], null, ke, kd, segmentLongitude);

            particles[i].Update(Time.deltaTime);

            //setejem els objectes
            realParticles[i].position = particles[i].GetPos();

            //Debug
            if (i < realParticles.Length-1)
                Debug.DrawLine(realParticles[i].position, realParticles[i + 1].position, Color.red);
        } 
    }
}
