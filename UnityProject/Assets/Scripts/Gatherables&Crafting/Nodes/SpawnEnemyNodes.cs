using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnEnemyNodes : MonoBehaviour {

    GameObject[] enemyNodes; //all of the GatherableNodes in the scene

    public float numSecondsBetweenSpawns = 60.0f; //how often modes should respawn
    float timeElapsedSinceLastSpawn = 0.0f; //elapsed time since the last spawn cycle

    public float targertProportionToSpawn = .6f; 

	// Use this for initialization
	void Start () {
        enemyNodes = GameObject.FindGameObjectsWithTag("EnemyNode");
        spawnNewNodes(enemyNodes.Length);
	    
    }
	
	// Update is called once per frame
	void Update () {
        timeElapsedSinceLastSpawn += Time.deltaTime;

        if (timeElapsedSinceLastSpawn > numSecondsBetweenSpawns) {
            timeElapsedSinceLastSpawn = 0;
           

            int numSpawnedNodes = enemyNodes.Count(c => c.GetComponent<EnemyNodeManager>().isSpawned); //determine how many nodes currently have enemies spawned
            spawnNewNodes((int)(enemyNodes.Length * targertProportionToSpawn) - numSpawnedNodes); //spawn enemies at enough nodes to meet our target proportion
        }
	}

    void spawnNewNodes(int numToSpawn) {
        //base-case: We don't need to spawn any more nodes
        if (numToSpawn <= 0) {
            return;
        }

        //find all unspawned nodes
        GameObject[] unSpawnedNodes = enemyNodes.Where(c => c.GetComponent<EnemyNodeManager>().isSpawned == false).ToArray<GameObject>();

        //cap number to spawn at the number of unspawned nodes (impossible to spawn more than number of nodes)
        if (numToSpawn > unSpawnedNodes.Length) {
            numToSpawn = unSpawnedNodes.Length;
        }

        //compose a list of numToSpawn random integers
        List<int> randomList = new List<int>();
        while (randomList.Count() < numToSpawn){
            int r = Random.Range(0, unSpawnedNodes.Length);
            if (!randomList.Contains(r)) {
                randomList.Add(r);
            }
        }

        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        //now tell the nodes at those locations in unSpawnedNodes to spawn
        for (int i = 0; i < randomList.Count(); i++) {
            //don't spawn a node if it is right next to the player
            if (Vector3.Distance(playerPosition, unSpawnedNodes[randomList[i]].transform.position) > 10) {
                unSpawnedNodes[randomList[i]].GetComponent<EnemyNodeManager>().spawnNode();
            }
        }


    }
}
