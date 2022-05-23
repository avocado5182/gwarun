using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMusic : MonoBehaviour {
    public static BGMusic Instance;

    public Slider musicSlider;
    public AudioMixerGroup musicMixer;
    public Vector2 musicVolumeRange;
    [Space]
    public Slider soundSlider;
    public AudioMixerGroup soundMixer;
    public Vector2 soundVolumeRange;
    public List<AudioClip> audioClips;
    AudioSource audioSrc;
    int currSong = 0;
    bool isPlaying = false;

    bool playedSfx = false;

    void Awake() {
        // if (Instance != null) Instance = this;
        Instance = this;
        
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc == null) audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.outputAudioMixerGroup = musicMixer;
        audioSrc.playOnAwake = false;
        audioSrc.loop = false;
        PlaySong(currSong);
    }

    // void Start() { // mixer.SetFloat() doesn't work in awake
    //     UpdateVolumeSlider(musicSlider, musicMixer, 0f, musicVolumeRange);
    //     UpdateVolumeSlider(soundSlider, soundMixer, 0f, soundVolumeRange);
    // }

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
        // Debug.Log(currSong);
        PlaySong(currSong);
    }

    public void ChangeMixerVal(AudioMixerGroup mixer, float val, Vector2 range) { // float 0-1
        float dB = valTodB(val, range);
        mixer.audioMixer.SetFloat("Volume", dB);
    }

    public void UpdateSlider(Slider slider, float dB, Vector2 dBRange) {
        slider.value = dBToVal(dB, dBRange);
    }
    
    public float dBToVal(float dB, float min, float max) { // min/max being dB range
        float v = dB + (min * -1f);
        v /= Mathf.Abs(min) + Mathf.Abs(max);
        v -= 1f;
        return (v == -1f) ? 0f : Mathf.Pow(10f, v);
    }

    public float dBToVal(float dB, Vector2 range) {
        return dBToVal(dB, range.x, range.y);
    }

    public float valTodB(float val, float min, float max) { // min/max being dB range
        float v = (val == 0f) ? -1f : Mathf.Log10(val); // decibel scale is logarithmic so this might feel more natural?
        float dB = Mathf.Lerp(min, max, v + 1f);
        return dB;
    }

    public float valTodB(float val, Vector2 range) {
        return valTodB(val, range.x, range.y);
    }

    public void OnMusicSliderValueChanged(float val) {
        ChangeMixerVal(musicMixer, val, musicVolumeRange);
    }
    
    public void OnSoundSliderValueChanged(float val) {
        ChangeMixerVal(soundMixer, val, soundVolumeRange);
        if (playedSfx) {
            GameManager.Instance.sfxMixer.audioMixer.GetFloat("Volume", out float vol);
            
            // quiet it
            float amt = 100f;
            GameManager.Instance.sfxMixer.audioMixer.SetFloat("Volume", (vol + (soundVolumeRange.x * -1f)) / amt - (soundVolumeRange.x * -1f));
            GameManager.Instance.sfxMixer.audioMixer.GetFloat("Volume", out vol);
            // Debug.Log(vol);
            
            GameManager.Instance.audioSrc.time = 0f;
            GameManager.Instance.audioSrc.Play();
        
            // back to normal
            GameManager.Instance.sfxMixer.audioMixer.SetFloat("Volume", (vol + (soundVolumeRange.x * -1f)) * amt - (soundVolumeRange.x * -1f));
        }
        else {
            playedSfx = true;
        }
    }
}
