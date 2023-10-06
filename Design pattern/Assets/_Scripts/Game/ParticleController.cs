using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    ParticleSystem PS;
    public bool play; 
    void Start()
    {
        PS = gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (play)
        {
            PS.Emit(PS.main.maxParticles);
            ParticleSystem.Particle[] particleArray = new ParticleSystem.Particle[PS.main.maxParticles];
            int particles = PS.GetParticles(particleArray);
            for (int i = 0; i < particles; i++)
            {
                Vector3 vel = particleArray[i].velocity;
                vel.z = 0;
                particleArray[i].velocity = vel;
            }
            PS.SetParticles(particleArray, particles);
            play = false; 
        }
    }
}
