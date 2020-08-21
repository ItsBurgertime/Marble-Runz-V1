using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{

    public float speed = 30;
    private PlayerController playerControllerScript;
    //private float leftBound = -5;
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


    private void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {



        transform.Translate(Vector3.forward * Time.deltaTime * speed);


        //if(transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
