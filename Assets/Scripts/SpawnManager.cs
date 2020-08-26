using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject enemyPrefab;
    //public GameObject powerUp;

    public int enemyCount;
    public int wave = 1;


    //Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(wave);
        //Instantiate(powerUp, GenerateSpawnPosition(), powerUp.transform.rotation);

    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn;  i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
    private UnityEngine.Vector3 GenerateSpawnPosition()
    {

        float spawnPosX = Random.Range(transform.position.x-4, transform.position.x + 4);
        float spawnPosZ = transform.position.z  ;//Random.Range(-spawnRange, spawnRange);
        UnityEngine.Vector3 randomPos = new UnityEngine.Vector3(spawnPosX, transform.position.y , spawnPosZ);
        return randomPos;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            wave++;
           SpawnEnemyWave(wave);
        }


    }

}
