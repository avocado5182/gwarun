using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMusic : MonoBehaviour {
    public AudioMixerGroup audioMixer;
    public List<AudioClip> audioClips;
    AudioSource audioSrc;
    int currSong = 0;
    bool isPlaying = false;

    void Awake() {
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc == null) audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.outputAudioMixerGroup = audioMixer;
        audioSrc.playOnAwake = false;
        audioSrc.loop = false;
        PlaySong(currSong);
    }

    void PlaySong(int index) {
        audioSrc.clip = audioClips[index];
        audioSrc.Play();
        isPlaying = true;
    }

    // Update is called once per frame
    void Update() {
        if (!isPlaying || audioSrc.isPlaying) return; // we done
        isPlaying = false;
        currSong++;
        currSong %= audioClips.Count;
        Debug.Log(currSong);
        PlaySong(currSong);
    }
}
