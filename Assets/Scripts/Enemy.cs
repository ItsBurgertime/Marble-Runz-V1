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
    public float extraGravity;
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
        // Ensures the extra gravity is only added when we're moving down
        if (eRig.velocity.y < 0)
        {
            UnityEngine.Vector3 l_newVelocity = eRig.velocity;
            l_newVelocity.y -= extraGravity;    // add the extra gravity every frame we're moving down
            eRig.velocity = l_newVelocity;
        }
    }
}
