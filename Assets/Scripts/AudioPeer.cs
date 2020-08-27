using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;

    public float[] _samples = new float[512];
    // Start is called before the first frame update
 

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
    }
    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }
}