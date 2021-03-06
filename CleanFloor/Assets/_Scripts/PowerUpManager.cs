using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] powerUpPrefabs = new GameObject[0];
    Queue<GameObject> shuffledPowerUpPrefabs;
    Queue<Vector3> shuffledPossiblePositions = new Queue<Vector3>();
    public int powerUpSpawnLoop = 20;
    public int maxPossiblePowerUpColloctPerLevel = 1;
    public int powerUpActiveInSceneLifeTime = 5;
    private int powerUpCollectCount = 0;
    private int spawnTime = 1;

    private void Start()
    {
        shuffledPowerUpPrefabs = new Queue<GameObject>(RandomNumberGenerator.ShuffleArray(powerUpPrefabs));
        spawnTime = RandomNumberGenerator.NextRandomInt(1, powerUpSpawnLoop);


    }
    private void OnEnable()
    {
        Swipe.OnLevelStarted += StartSpawnLoop;
        PowerUp.OnPowerUpCollected += PowerUpCollected;
    }
    private void OnDisable()
    {
        PowerUp.OnPowerUpCollected -= PowerUpCollected;
        Swipe.OnLevelStarted -= StartSpawnLoop;
    }
    public void SetPossibleSpwanPositions(Queue<Vector3> possiblePositions)
    {
        shuffledPossiblePositions = possiblePositions;


    }


    public void StartSpawnLoop()
    {
        StartCoroutine(SpawnCounDown(powerUpSpawnLoop));
    }
    private IEnumerator SpawnCounDown(int loopTime)
    {

        yield return new WaitForSeconds(spawnTime);
        var pos = GetRandomPos();
        //spawn
        var poerUpGo = GameObject.Instantiate(GetRandomPowerUP(), new Vector3(pos.x, 2, pos.z), Quaternion.identity);
        poerUpGo.GetComponent<PowerUp>().Instantieted(powerUpActiveInSceneLifeTime);

        yield return new WaitForSeconds(loopTime - spawnTime);

        if (powerUpCollectCount < maxPossiblePowerUpColloctPerLevel)
        {
            ResetLoop();
        }

    }

    public GameObject GetRandomPowerUP()
    {
        var randomPowerUp = shuffledPowerUpPrefabs.Dequeue();
        shuffledPowerUpPrefabs.Enqueue(randomPowerUp);
        return randomPowerUp;
    }

    public Vector3 GetRandomPos()
    {
        var randomPos = shuffledPossiblePositions.Dequeue();
        shuffledPossiblePositions.Enqueue(randomPos);
        return randomPos;
    }

    private void ResetLoop()
    {

        StopAllCoroutines();
        StartCoroutine(SpawnCounDown(powerUpSpawnLoop));
    }


    public void PowerUpCollected(PoweUpType powerUpType)
    {
        powerUpCollectCount++;
    }

}

