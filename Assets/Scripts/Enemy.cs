using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 3.0f;
    private Rigidbody eRig;
    private GameObject player;
    public float destroyDistance = 20;
    // Start is called before the first frame update
    void Start()
    {
        eRig = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        eRig.AddForce(lookDirection * speed);

        var distanceFromPlayer = (player.transform.position - transform.position).magnitude;
        if (distanceFromPlayer > destroyDistance)
        {
            //Checks for the distance between the player and the enemy marble. Also displays current enemy marble position.
            //Debug.Log($"distance from player: {distanceFromPlayer}, destroy distance {destroyDistance}");
            //Debug.Log($"current enemy position {transform.position}");
            Destroy(gameObject);
        }
    }
}
