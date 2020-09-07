using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject enemyPrefab;
    //public GameObject powerUp;

    public int enemyCount;
    public int wave = 1;
    private bool enemyWaveInProgress;


    //Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyWave());
    }

    IEnumerator SpawnEnemyWave()
    {
        if (enemyWaveInProgress == false)
        {
            enemyWaveInProgress = true;
            wave = wave + 1;
            var enemiesToSpawn = Math.Min(wave, 5);
            yield return new WaitForSeconds(2);
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            }
            enemyWaveInProgress = false;
        }
    }


    private UnityEngine.Vector3 GenerateSpawnPosition()
    {

        float spawnPosX = UnityEngine.Random.Range(transform.position.x - 8, transform.position.x + 8);
        float spawnPosZ = transform.position.z;
        UnityEngine.Vector3 randomPos = new UnityEngine.Vector3(spawnPosX, transform.position.y, spawnPosZ);
        return randomPos;
    }

    // Update is called once per frame
    void Update()
    {


        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            StartCoroutine(SpawnEnemyWave());
        }


    }

}
