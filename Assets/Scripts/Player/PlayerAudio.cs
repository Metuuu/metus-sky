using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {

    #region [ - Init - ]

    // Enums
    public enum GROUND_TYPE { DIRT } // TODO: tää sitten laitetaan ihan muualle joskus

    // Classes
    AudioSource audioS;
    HeadBobber headBobber;

    // Audio Clips
    [System.Serializable]
    public class StepSounds { public AudioClip[] dirt; }
    public StepSounds stepSounds;
    public AudioClip crippleSound;

    #endregion


    // - Start -
    void Start() {
        audioS = GetComponent<AudioSource>();
        Character.Cameras.GetComponent<HeadBobber>().Stepped += OnSepped; // on stepped listener for stepping sounds
    }



    #region [ - Play Sounds - ]

    public void CrippleSound() {
        audioS.PlayOneShot(crippleSound);
    }
    
    private void StepSound(GROUND_TYPE groundType) { // TODO: movement type että juokseeko vaiko kävelee vai sneakkaa vai mitä
        switch (groundType) {
            case GROUND_TYPE.DIRT:
                audioS.PlayOneShot(stepSounds.dirt[Random.Range(0, stepSounds.dirt.Length)]);
                break;
            default:
                break;
        }
    }

    #endregion

    
    
    private void OnSepped() { // object source, System.EventArgs e   TODO: tähän pitäis laittaa event argseihin ground type
        StepSound(GROUND_TYPE.DIRT);
    }


}
