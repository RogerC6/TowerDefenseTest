using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvasionCellSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float minimumSpawnTime;

    [SerializeField]
    private float maximumSpawnTime;

    private float timeUntilSpawn;

    private int[] enemyTypeCounts;

    private int totalCellTypes = System.Enum.GetValues(typeof(CellType)).Length;
    void Awake()
    {
        enemyTypeCounts = new int[totalCellTypes];
        SetTimeUntilSpawn();
    }

    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;

        if(timeUntilSpawn <= 0)
        {
            SpawnEnemy();
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

    private void SpawnEnemy()
    {
        CellType randomCellType = GetRandomCellType();

        GameObject enemy = Instantiate(enemyPrefab);
        MovingInfection movingInfection = enemy.GetComponent<MovingInfection>();

        if (movingInfection != null)
        {
            movingInfection.cellType = randomCellType; 
        }

        enemyTypeCounts[(int)randomCellType]++;

        Debug.Log($"Spawned enemy of type: {randomCellType}");
    }

    private CellType GetRandomCellType()
    {
        CellType[] cellTypes = (CellType[])System.Enum.GetValues(typeof(CellType));
        int randomIndex = Random.Range(0, cellTypes.Length);
        CellType selectedType = cellTypes[randomIndex];
        Debug.Log($"Randomly selected CellType: {selectedType}");
        return selectedType;
    }
}
