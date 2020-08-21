using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Camera;
    public float playerDestroyDistance = 20;

    private void Update()
    {
        float playerDistanceFromCamera = Vector3.Distance(Player.transform.position , Camera.transform.position);
        if(playerDistanceFromCamera > playerDestroyDistance)
        {
            //Destroy(Player);
            //Debug.Log($"distance from track {playerDistanceFromCamera}");
        }

    }

}
