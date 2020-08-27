using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
*
*/
namespace AudioVisualizer
{

public class BeatListener : MonoBehaviour 
{

    //____________Public Variables

    public UnityEvent OnBeatRecognized;

    //____________Delegates/Actions

    //____________Protected Variables

    //____________Private Variables

    /*________________Monobehaviour Methods________________*/

    private void OnEnable()
    {
            AudioEventListener.OnBeatRecognized += HandleBeat;
    }

    private void OnDisable()
    {
        AudioEventListener.OnBeatRecognized -= HandleBeat;
    }

    /*________________Public Methods________________*/

    /*________________Protected Methods________________*/

    /*________________Private Methods________________*/

    void HandleBeat(Beat beat)
    {
       OnBeatRecognized.Invoke();
    }



}

}
