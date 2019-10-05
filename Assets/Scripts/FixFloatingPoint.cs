using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;


public class FixFloatingPoint : MonoBehaviour {

    public Transform centerObj;
    public int maxDistance = 2000;



    Queue<MoveObjectsThreadInfo> moveObjecstThreadInfoQueue = new Queue<MoveObjectsThreadInfo>();


    // Update
    void Update () {

        /*if (moveObjecstThreadInfoQueue.Count > 0) {
            for (int i = 0; i < moveObjecstThreadInfoQueue.Count; ++i) {
                MoveObjectsThreadInfo threadInfo = moveObjecstThreadInfoQueue.Dequeue();
            }
        }*/

        //Debug.Log(Vector3.Distance(Vector3.zero, centerObj.transform.position));

        while (Vector3.Distance(Vector3.zero, centerObj.position) > maxDistance) {

            // Move Objects
            MoveObjects(centerObj.position);

            // Move particles
            //MoveParticles();


        }


    }



    // Move objects
    void MoveObjects(Vector3 pos) {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {
            if (obj.activeInHierarchy) {
                if (obj.transform.parent == null) {
                    obj.transform.position -= pos;
                }
            }
        }
    }

    // Move particles
    void MoveParticles() {
        ParticleSystem[] particlesystems = FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
        foreach (ParticleSystem pSys in particlesystems) {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[pSys.particleCount + 1];
            int numParticlesAlive = pSys.GetParticles(particles);
            for (int i = 0; i < numParticlesAlive; ++i) {
                particles[i].position -= centerObj.position;
            }
            pSys.SetParticles(particles, numParticlesAlive);
        }
    }




    // Map thread info
    struct MoveObjectsThreadInfo { }


}
