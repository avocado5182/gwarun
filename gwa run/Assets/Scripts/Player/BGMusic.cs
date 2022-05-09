using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMusic : MonoBehaviour {
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup soundMixer;
    public List<AudioClip> audioClips;
    AudioSource audioSrc;
    int currSong = 0;
    bool isPlaying = false;

    void Awake() {
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc == null) audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.outputAudioMixerGroup = musicMixer;
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

    public void ChangeMixerVolume(AudioMixerGroup mixer, float val) { // float 0-1
        float v = Mathf.Log10(val); // decibel scale is logarithmic so this might feel more natural?
        mixer.audioMixer.SetFloat("Volume", Mathf.Lerp(-80f, 0f, v + 1));
    }

    public void OnMusicSliderValueChanged(float val) {
        ChangeMixerVolume(musicMixer, val);
    }
    
    public void OnSoundSliderValueChanged(float val) {
        ChangeMixerVolume(soundMixer, val);
    }
}
