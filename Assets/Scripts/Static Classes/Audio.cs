using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Audio {

    public static void PlaySoundEffect(AudioClip clip, Vector3 position, float volume, float pitch, Transform parentToFollow = null) {
        GameObject obj = new GameObject();
        obj.transform.position = position;
        AudioSource audioS = obj.AddComponent<AudioSource>();
        audioS.pitch = pitch;
        audioS.PlayOneShot(clip, volume);
        GameObject.Destroy(obj, clip.length / pitch);
        if (parentToFollow) {
            obj.transform.parent = parentToFollow;
        }
    }



}
