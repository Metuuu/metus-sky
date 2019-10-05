using UnityEngine;
using System.Collections;

public class SpaceshipAudio : MonoBehaviour {

    // - Init -
    public AudioSource audioSource;
    public AudioClip jetStartEngine;
    public AudioClip jetStopEngine;
    public AudioClip engineOverheatExplosion;
    public AudioClip thruster;

    bool usePitch;
    float lowPitch;
    float highPitch;
    float speedToRevs;

    Transform transform;
    Vector3 myVelocity;
    Rigidbody rb;
    

    // - Start -
    void Start() {
        transform = gameObject.transform;
        rb = GetComponent<Rigidbody>();
        audioSource.loop = true;
    }


    // - Fixed Update -
    private void FixedUpdate() {

        if (usePitch) {
            myVelocity = rb.velocity;
            float forwardSpeed = transform.InverseTransformDirection(rb.velocity * 50).z;
            float engineRevs = Mathf.Abs(forwardSpeed) * speedToRevs;
            audioSource.pitch = Mathf.Clamp(engineRevs, lowPitch, highPitch);
        }
        
        
    }



    // - Gear changed -
    public void GearChanged(AlusController.Gear.Audio audio) {
        if (audio.hasAudio) {
            audioSource.clip = audio.EngineSound;
            audioSource.pitch = 1;
            usePitch = audio.usePitch;
            lowPitch = audio.LowPitch;
            highPitch = audio.HighPitch;
            speedToRevs = audio.SpeedToRevs;
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
        
    }



    // - Sound play functions -

    // engine start
    public void PlayEngineStartSound() {
        Audio.PlaySoundEffect(jetStartEngine, transform.position, 1, 1, transform);
        audioSource.Play();
    }

    // engine stop
    public void PlayEngineStopSound() {
        audioSource.Stop();
        Audio.PlaySoundEffect(jetStopEngine, transform.position, 1, 1, transform);
    }


}