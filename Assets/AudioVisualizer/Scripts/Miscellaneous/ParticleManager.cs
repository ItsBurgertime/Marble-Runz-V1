using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*
*/
public class ParticleManager : MonoBehaviour 
{

    //____________Public Variables

    //____________Delegates/Actions

    //____________Protected Variables

    //____________Private Variables

    ParticleSystem[] particles;

    /*________________Monobehaviour Methods________________*/

    private void Start()
    {
        particles = this.GetComponentsInChildren<ParticleSystem>();
    }

    /*________________Public Methods________________*/

    public void Emit(int n)
    {
        foreach(ParticleSystem ps in particles)
        {
            ps.Emit(n);
        }
    }

    /*________________Protected Methods________________*/

    /*________________Private Methods________________*/



}
