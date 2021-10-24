using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public GameObject backgroundManager;
    public GameObject startingObstacleContainer;
    [Header("Squirrel cash. These settings only affect squirrel cash gain")]
    public int minCash;
    public int maxCash;

    [Header("Spawn settings")]
    [Tooltip("How many instances of each object is allowed to exist simultaneously")]
    public int[] maxInstancesPerObject;
    [Tooltip("Spawn interval per object")]
    public float[] spawnInterval;
    [Tooltip("How many seconds plus or minus is allowed for the spread interval")]
    public float[] spawnIntervalSpread;
    [Tooltip("Obstacle prefabs. Keep all of the above arrays the same size as this one")]
    public GameObject[] obstaclePrefabs;
    public GameObject player;
    public GameObject bestDistanceIndicator;
    public bool spawning = false;
    bool spawnBestDistanceIndicatorOnce = false;
    Vector2 prevSpawnPos;

    float minSpawnInterval = 0.5f;
    Coroutine[] coroutines;
    GameObject[] obstacleTrees;

    [HideInInspector]
    public static ObstacleSpawner obstacleSpawner;

    // Start is called before the first frame update
    void Start()
    {
        SpawnStartingObstacles();
    }

    float minXSpawn = 5;
    float maxXspawn = 25;
    void SpawnStartingObstacles()
    {
        int obstacleAmount = Random.Range(4, 8);
        for (int i = 0; i < obstacleAmount; i++)
        {
            int randomAnimal = Random.Range(0, obstaclePrefabs.Length);
            if (randomAnimal == 0 || randomAnimal == 1 || randomAnimal == 2 || randomAnimal == 4 || randomAnimal == 7 || randomAnimal == 8 || randomAnimal == 9 || randomAnimal == 12 || randomAnimal == 13)
            {
                i--;
            }
            else
            {
                float xPos = Random.Range(minXSpawn, maxXspawn);
                float height = transform.position.y + Random.Range(obstaclePrefabs[randomAnimal].transform.GetChild(0).GetComponent<Obstacle>().minHeight, obstaclePrefabs[randomAnimal].transform.GetChild(0).GetComponent<Obstacle>().maxHeight);
                Vector2 spawnPos = new Vector2(xPos, height);
                GameObject instantiated = Instantiate(obstaclePrefabs[randomAnimal], spawnPos, Quaternion.identity, startingObstacleContainer.transform);
                instantiated.transform.GetChild(0).GetComponent<Obstacle>().backgroundManager = backgroundManager;
                instantiated.transform.GetChild(0).GetComponent<Obstacle>().player = player;
                instantiated.transform.GetChild(0).GetComponent<Obstacle>().manager = backgroundManager.GetComponent<BackgroundManager>();
            }
        }
    }

    private void Awake()
    {
        obstacleSpawner = this;
        Debug.Log("WHAT");
        coroutines = new Coroutine[obstaclePrefabs.Length];
        prevSpawnPos = new Vector2(0, 0);
        obstacleTrees = new GameObject[obstaclePrefabs.Length];
        for (int i = 0; i < obstacleTrees.Length; i++)
        {
            obstacleTrees[i] = new GameObject();
            obstacleTrees[i].transform.parent = transform;
            obstacleTrees[i].name = "" + i;
        }
        //StartSpawningAll();
    }
    //obstacleSpawner.GetComponent<ObstacleSpawner>().StartSpawningAll();
    public void StartSpawningAll()
    {
        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            float randTime = Random.Range(spawnInterval[i] - spawnIntervalSpread[i], spawnInterval[i] + spawnIntervalSpread[i]);
            coroutines[i] = StartCoroutine(ExecuteAfterTime(randTime, i));

        }
        //spawning = true;
        //MyElement = Elements[Random.Range(0,Elements.Length)];
    }

    public void StopSpawningAll()
    {
        StopAllCoroutines();
        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            coroutines[i] = null;
        }
        //spawning = false;
    }

    // Update is called once per frame
    /*void Update()
    {
        //if ( !spawnBestDistanceIndicatorOnce && backgroundManager.GetComponent<GameManager>().distance + 32 > 100)
        if (!spawnBestDistanceIndicatorOnce && GameManager.getDistance() + 32 > 100)
        {
            Debug.Log("BEST DISTANCE TEST");
            spawnBestDistanceIndicatorOnce = true;
            SpawnBestDistanceIndicator();
        }
        //Debug.Log(coroutines[0]);
        //Debug.Log(backgroundManager.GetComponent<GameManager>().distance);
    }*/
    public bool isPaused;
    IEnumerator ExecuteAfterTime(float time, int index)
    {
        while (isPaused)
        {
            yield return null;
        }
        //Debug.Log(time + " " + index);
        yield return new WaitForSeconds(time);
        SpawnObstacle(index);

    }

    public void PauseSpawner()
    {
        isPaused = true;
    }

    public void ResumeSpawner()
    {
        isPaused = false;
    }

    public void SpawnBestDistanceIndicator()
    {
        GameObject instantiated = Instantiate(bestDistanceIndicator, new Vector2(49, 0), Quaternion.identity);
        instantiated.transform.GetChild(0).GetComponent<Obstacle>().backgroundManager = backgroundManager;
        instantiated.transform.GetChild(0).GetComponent<Obstacle>().manager = backgroundManager.GetComponent<BackgroundManager>();
    }

    float obstacleOverlapMargin = 0.3f;
    public void SpawnObstacle(int index)
    {
        if (coroutines[index] != null)
            coroutines[index] = null;                                               //multiplier, mindeDuctible, maxDeductible
        float speedTime = Mathf.Clamp(-backgroundManager.GetComponent<BackgroundManager>().xSpeed / 50, 0, 5);
        //Debug.Log(speedTime);
        float randTime = Mathf.Clamp((Random.Range(spawnInterval[index] - spawnIntervalSpread[index], spawnInterval[index] + spawnIntervalSpread[index])) - speedTime, minSpawnInterval, Mathf.Infinity);
        //randTime = Mathf.Clamp((Random.Range(spawnInterval[index] - spawnIntervalSpread[index], spawnInterval[index] + spawnIntervalSpread[index])) - speedTime, 0.1f, Mathf.Infinity);
        if (obstacleTrees[index].transform.childCount < maxInstancesPerObject[index])
        {
            float height = Random.Range(obstaclePrefabs[index].transform.GetChild(0).GetComponent<Obstacle>().minHeight, obstaclePrefabs[index].transform.GetChild(0).GetComponent<Obstacle>().maxHeight);
            Vector2 spawnPos = transform.position + new Vector3(0, height);
            spawnPos.x = player.transform.position.x + 30;
            //Debug.Log("SPAWN " + prevSpawnPos + " ABC " + backgroundManager.GetComponent<GameManager>().distance + " " + spawnPos.y);
            //if (prevSpawnPos.x < backgroundManager.GetComponent<GameManager>().distance - 0.5f || spawnPos.y < prevSpawnPos.y - 0.25f || spawnPos.y > prevSpawnPos.y + 0.25f)
            if (prevSpawnPos.x < GameManager.getDistance() - (obstacleOverlapMargin * 2) || spawnPos.y < prevSpawnPos.y - obstacleOverlapMargin || spawnPos.y > prevSpawnPos.y + obstacleOverlapMargin)
            {
                if (spawning)
                {
                    GameObject instantiated = Instantiate(obstaclePrefabs[index], spawnPos, Quaternion.identity, obstacleTrees[index].transform);
                    instantiated.transform.GetChild(0).GetComponent<Obstacle>().backgroundManager = backgroundManager;
                    instantiated.transform.GetChild(0).GetComponent<Obstacle>().player = player;
                    instantiated.transform.GetChild(0).GetComponent<Obstacle>().manager = backgroundManager.GetComponent<BackgroundManager>();
                    if (instantiated.transform.GetChild(0).GetComponent<Obstacle>().typeAnimal == (int)Obstacle.animalType.SquirrelFlying || instantiated.transform.GetChild(0).GetComponent<Obstacle>().typeAnimal == (int)Obstacle.animalType.SquirrelRunning)
                    {
                        instantiated.GetComponent<Obstacle>().minCash = minCash;
                        instantiated.GetComponent<Obstacle>().maxCash = maxCash;
                    }
                    //prevSpawnPos.x = backgroundManager.GetComponent<GameManager>().distance;
                    prevSpawnPos.x = GameManager.getDistance();
                    prevSpawnPos.y = spawnPos.y;
                }
            }
            else
            {
                randTime = 0.5f;
                //Debug.Log("OVERLAPPING OBSTACLES PREVENTED");
            }
        }
        else
        {
            randTime = 0.5f;
        }
        //Debug.Log("Index: " + index + " Time: " + randTime);
        coroutines[index] = StartCoroutine(ExecuteAfterTime(randTime, index));
        //spawning = true;
    }

    public void StopSpawningObstacle(int index)
    {
        StopCoroutine(coroutines[index]);
        coroutines[index] = null;
    }
}
