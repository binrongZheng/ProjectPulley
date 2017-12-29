using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ourEngine;

public class RopeManager : MonoBehaviour {

    public Transform[] realParticles;

    ourParticle[] particles;

    // Use this for initialization
    void Start()
    {
        //iniciem les nostra simulacio a la posicio de la corda real
        particles = new ourParticle[realParticles.Length];
        for (int i = 0; i < realParticles.Length; i++) {
            particles[i] = new ourParticle(realParticles[i].position, 1);
        }       

    }

    // Update is called once per frame
    void Update()
    {
        //Simular posicions
        for (int i = 0; i < realParticles.Length; i++)
        {
            //actualitzem la simulacio
            particles[i].Update(Time.deltaTime);
       
            //setejem els objectes
            realParticles[i].position = particles[i].GetPos();
        } 
    }
}
