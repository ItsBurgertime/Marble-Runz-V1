using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*
*/
namespace AudioVisualizer
{
    public class Waveform : MonoBehaviour
    {

        //____________Public Variables

        [Tooltip("Index into the AudioSampler audioSources or audioFiles list")]
        public int audioIndex = 0; // index into audioSampler audioSources or audioFIles list. Determines which audio source we want to sample
        [Tooltip("The frequency range you want to listen to")]
        public FrequencyRange frequencyRange = FrequencyRange.Decibal; // what frequency will we listen to? 
        [Tooltip("Height of the waveform")]
        public float waveformAmplitude = 5; // height of the waveform.

        [Tooltip("Sample from a recorded AudioFile?")]
        public bool useAudioFile = false; // if true, it will use the audioFile inside teh AudioSampler audioFiles array
        [Tooltip("Take the absolute value of samples?")]
        public bool abs = false; // use absolute value or not


        //____________Delegates/Actions

        //____________Protected Variables

        //____________Private Variables

        /*________________Monobehaviour Methods________________*/

        /*________________Public Methods________________*/


        /*________________Protected Methods________________*/

        /*________________Private Methods________________*/



    }
}
