using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script written by Anna Qiu
 * GitHub: annaq1    Email: annaq1@uci.edu
 */

/* Procedural Prefab Generation (2D)
 * Customizable parameters:
 *     Maximum number of items to spawn at beginning
 *     Spawn distance from other items
 *     Central location to spawn
 *     Max range of spawn from central location
 *     Dictionary/table mapping to set probability of each item to spawn
 */
//Goals:
//generate random clusters of items
//priority/rareness idea
//spawn intermittently, with time to spawn



//Want to spawn a few "seeds" at first *
//Then, get locations of those seeds *
//and spawn a few items near those seeds; *
//Check that the items spawned do not overlap with another
//Then, use this function to get array of item near seed
//If there are too many near seed, choose another random object in array to be seed
//spawn objects around that random object then
//repeat this until spawn count is depleted
// maybe use recursion? if i can figure out how to use it

public class SpawnAttempt1 : MonoBehaviour {

    public GameObject[] items = new GameObject[3]; //array of items to spawn

    public GameObject spawner;//Gameobject that script is attatched to

    public int totalNumberToSpawn; //max number to spawn ///Change to a max num to spawn and a min num to spawn for more customizability
    public float itemSpacing; //spacing between items
    public float maxSpawnRange; //maximum range from spawner location

    private List<Vector2> initialPositionsList = new List<Vector2>();//used to store positions of initially spawned objects, which are seeds 

    public Collider2D[] colliders;
    
    // Returns random SpawnLocation based on spawnPos of spawner and max range from center of spawner
    Vector2 SpawnLocation(Vector2 spawnPos, float max)
    {
        float randX = Random.Range(-max, max) + spawnPos.x;
        float randY = Random.Range(-max, max) + spawnPos.y;
        return new Vector2(randX, randY);
    }

    //Returns random object from given array
    GameObject RandomArrayChoice(GameObject[] arr)
    {
        int index = Random.Range(0, arr.Length);
        return arr[index];
    }

    //Spawns a single GameObject at the spawnerPosition
    GameObject SpawnOneItem(Vector2 spawnerPosition, float maxSpawnRange)
    {
        Vector2 position = SpawnLocation(spawnerPosition, maxSpawnRange);
        bool canSpawnHere = PreventSpawnOverlap(position);
        Debug.Log(position);
        Debug.Log("YOU CAN " + canSpawnHere);


        //DOESNT WORK
        while (canSpawnHere == false)
        {
            position = SpawnLocation(spawnerPosition, maxSpawnRange);
            canSpawnHere = PreventSpawnOverlap(position);
            Debug.Log(canSpawnHere);

        }

        GameObject randItem = RandomArrayChoice(items);
        return Instantiate(randItem, position, Quaternion.identity);
    }

    //Spawns totalNumberToSpawn/divideBy, as initial seed from which clusters will form
    //first, spawn totalNumberToSpawn/divideBy
    //but if totalNumberToSpawn/divideBy is less than 1, only spawn one initial item
    void InitialSpawn()
    {
        Vector2 spawnerPosition = spawner.transform.position; // position of spawner object
        int divideBy = 4; // number to divide totalNumberToSpawn by (so I can change it easier later if I want)

        if (totalNumberToSpawn/divideBy < 1)
        {
            GameObject justSpawned = SpawnOneItem(spawnerPosition, maxSpawnRange);
            totalNumberToSpawn--;
            initialPositionsList.Add(justSpawned.transform.position);
        }
        else
        {
            for (int i = 0; i < totalNumberToSpawn/divideBy; ++i)
            {
                GameObject justSpawned = SpawnOneItem(spawnerPosition, maxSpawnRange);
                totalNumberToSpawn--;
                initialPositionsList.Add(justSpawned.transform.position);
            }
        }
    }

    //Spawn in clusters after the initial spawning of seeds
    void SpawnMore()
    {

        float maxRange = (float)2.0;

        while (totalNumberToSpawn != 0)
        {
            int index = Random.Range(0, initialPositionsList.Count); // gets random index from list
            Vector2 spawnerPosition = initialPositionsList[index]; //random spawn location from the seeds available
            GameObject justSpawned = SpawnOneItem(spawnerPosition, maxRange);
            totalNumberToSpawn--;
            Debug.Log(totalNumberToSpawn);

        }

    }

    //DOESNT WORK
    bool PreventSpawnOverlap(Vector2 spawnPos)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, 5);//1 is radius, find better solution
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector2 centerPoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x;
            float height = colliders[i].bounds.extents.y;
            float leftExtent = centerPoint.x - width;
            float rightExtent = centerPoint.x + width;
            float lowerExtent = centerPoint.y - height;
            float upperExtent = centerPoint.y + height;

            if (spawnPos.x >= leftExtent && spawnPos.x <= rightExtent)
            {
                if (spawnPos.y >= lowerExtent && spawnPos.y <= upperExtent)
                    return false;
            }
        }
        return true;
    }

    /*
    //Return true if object collides with another
    int HasCollisions(GameObject spawning)
    {
        
        Debug.Log(spawning.transform.position);
        Collider2D[] results = new Collider2D[100];
        int number = spawning.GetComponent<Collider2D>().GetContacts(results);
        Debug.Log("NUM" + number);
        return number;
        //Collider2D already = alreadySpawned.GetComponent<Collider2D>();
        //Collider2D spawn = spawning.GetComponent<Collider2D>();
        //return already.IsTouching(spawn);
    
    }
    */

    //write function to choose one of the 3 types of items to spawn based
    //on probability or random number

    // Use this for initialization
    void Start()
    {
        InitialSpawn();
        Debug.Log(totalNumberToSpawn);
        SpawnMore();
    }
}
