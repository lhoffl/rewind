using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    
    [SerializeField]
    AudioClip _start, _loop, _end;
    AudioSource _source;

    private bool _loopMusic = true;

    void Start() {
        _source = GetComponent<AudioSource>();

        DontDestroyOnLoad(this);

        _source.volume = 0.8f;
        StartMusic();
    }

    void Update() {
        if(_loopMusic)
            LoopMusic();
    }

    public void StartMusic() {
        _source.PlayOneShot(_start);

        _source.clip = _loop;
        _source.PlayDelayed(_start.length);
    }

    public void EndMusic() {
        _loopMusic = false;
        _source.Stop();
        _source.PlayOneShot(_end);
    }

    public void LoopMusic() {
        if(_source.isPlaying)
            return;
        
        _source.Play();
    }

    public IEnumerator ReduceVolumeForSeconds(float amount, float seconds) {
        _source.volume = amount;
        yield return new WaitForSeconds(seconds);
        _source.volume = 1.0f;
    }
}
