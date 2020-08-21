using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 30;
    //private PlayerController playerControllerScript;

    private float speedWas;
    public void Pause()
    {
        //save speed value for later
        speedWas = speed;
        speed = 0;
    }
    public void UnPause()
    {
        //recall saved speed value from earlier
        speed = speedWas;
    }


    //private void Start()
    //{
    //    playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    //}

    private void Update()
    {

        Vector3 moveBack = new Vector3(0, 0, -1);

        transform.Translate(moveBack * Time.deltaTime * speed);


        //if(transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
