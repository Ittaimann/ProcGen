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

public class SpawnAttempt1 : MonoBehaviour {

    public GameObject[] items = new GameObject[3]; //array of items to spawn

    public GameObject spawner;//Gameobject that script is attatched to

    public int totalNumberToSpawn; //max number to spawn
    public float itemSpacing; //spacing between items
    public float maxSpawnRange; //maximum range from spawner location

    
    // Returns random SpawnLocation based on spawnPos of spawner and maxSpawnRange from center of spawner
    Vector2 SpawnLocations(Vector2 spawnPos)
    {
        float max = (float)maxSpawnRange;
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
    
    //write function to choose one of the 3 types of items to spawn based
    //on probability or random number

    // Use this for initialization
    void Start()
    {
        Vector2 spawnerPosition = spawner.transform.position; // position of spawner object
        Debug.Log(spawnerPosition);
        for (int i = 0; i < totalNumberToSpawn; ++i)
        {
            Vector2 position = SpawnLocations(spawnerPosition);
            Instantiate(RandomArrayChoice(items), position, Quaternion.identity);

        }
    }

	
	/*// Update is called once per frame
     * Don't need update because this spawner only
     * designed to spawn items at beginning
	void Update () {
		
	}*/
}
