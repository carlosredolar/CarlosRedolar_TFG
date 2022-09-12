using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform spawnCentre;
    public int amountOfEnemies;
    public float spawnRange;
    private bool spawnPointSet = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountOfEnemies - 1; i++)
        {
            float randomZ = Random.Range(0f, spawnRange);
            float randomX = Random.Range(-spawnRange, spawnRange);

            Vector3 spawnPoint = new Vector3(spawnCentre.position.x + randomX, spawnCentre.position.y, spawnCentre.position.z + randomZ);

            Rigidbody rb = Instantiate(enemy, spawnPoint, Quaternion.identity).GetComponent<Rigidbody>();
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
