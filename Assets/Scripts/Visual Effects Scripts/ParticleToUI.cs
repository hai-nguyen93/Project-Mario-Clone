using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleToUI : MonoBehaviour
{
    ParticleSystem.Particle[] particles;
    ParticleSystem ps;
    Vector2[] originalPos;
    bool first = true;

    public RectTransform destinationUI;
    private Vector2 destination;

    private void Start()
    {
        Initialze();       
    }

    private void LateUpdate()
    {
        int numParticlesAlive = ps.GetParticles(particles);
        destination = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(destinationUI.transform.position));
        Debug.Log(destination);

        // get original particles' position
        if (first)
        {
            for (int i = 0; i < numParticlesAlive; ++i)
            {
                originalPos[i] = particles[i].position;
            }
            first = false;
        }

        for (int i = 0; i < numParticlesAlive; ++i)
        {
            particles[i].position = Vector2.Lerp(originalPos[i], destination, (particles[i].startLifetime - particles[i].remainingLifetime) / particles[i].startLifetime);
        }
        ps.SetParticles(particles, numParticlesAlive);
        
    }

    private void Initialze()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();

        if (particles == null || particles.Length < ps.main.maxParticles)
        {
            particles = new ParticleSystem.Particle[ps.main.maxParticles];
            originalPos = new Vector2[ps.main.maxParticles];
        }
    }   
}
