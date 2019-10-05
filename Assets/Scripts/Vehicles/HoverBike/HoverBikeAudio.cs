using UnityEngine;
using System.Collections;

public class HoverBikeAudio : MonoBehaviour {

    // - Init -
        
    Vector3 myVelocity;
    Rigidbody carRigidbody;

    // Sounds
    public AudioSource audioSource;
    public AudioClip jetStartEngine;
    public AudioClip jetStopEngine;
    public AudioClip jetEngineLoop;

    public float LowPitch = .1f;
    public float HighPitch = 5f;
    public float SpeedToRevs = .01f;

    public AudioClip glassBreak;



    // - Start -
    void Start() {
        carRigidbody = GetComponent<Rigidbody>();
        audioSource.clip = jetEngineLoop;
        audioSource.loop = true;
    }


    // - Fixed Update -
    private void FixedUpdate() {
        myVelocity = carRigidbody.velocity;
        float forwardSpeed = transform.InverseTransformDirection(carRigidbody.velocity * 50).z;
        float engineRevs = Mathf.Abs(forwardSpeed) * SpeedToRevs;
        audioSource.pitch = Mathf.Clamp(engineRevs, LowPitch, HighPitch);
    }



    // - Sound play functions -

    // Play one shot
    public void PlayGlassBreakSound() {
        audioSource.PlayOneShot(glassBreak,0.7f);
    }


    // engine start
    public void PlayEngineStartSound() {
        audioSource.PlayOneShot(jetStartEngine);
        audioSource.Play();
    }

    // engine stop
    public void PlayEngineStopSound() {
        audioSource.Stop();
        audioSource.PlayOneShot(jetStopEngine);
    }


}