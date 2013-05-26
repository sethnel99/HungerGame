using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnGatherableNodes : MonoBehaviour {

    GameObject[] gatherableNodes; //all of the GatherableNodes in the scene

    public float numSecondsBetweenSpawns = 60.0f; //how often modes should respawn
    float timeElapsedSinceLastSpawn = 0.0f; //elapsed time since the last spawn cycle

    public float targertProportionToSpawn = .75f; //proportion of total spawns we would like to be active at any time

	// Use this for initialization
	void Start () {
        gatherableNodes = GameObject.FindGameObjectsWithTag("GatherableNode");
        spawnNewNodes(gatherableNodes.Length / 2);
	    
    }
	
	// Update is called once per frame
	void Update () {
        timeElapsedSinceLastSpawn += Time.deltaTime;

        if (timeElapsedSinceLastSpawn > numSecondsBetweenSpawns) {
            timeElapsedSinceLastSpawn = 0;
           

            int numSpawnedNodes = gatherableNodes.Count(c => c.GetComponent<GatherableNodeManager>().isSpawned); //determine how many nodes currently have gatherables spawned
            spawnNewNodes((int)(gatherableNodes.Length * targertProportionToSpawn) - numSpawnedNodes); //spawn gatherables at enough nodes to meet our target proportion
        }
	}

    void spawnNewNodes(int numToSpawn) {
        //Debug.Log("Spawn new nodes");
        //base-case: We don't need to spawn any more nodes
        if (numToSpawn <= 0) {
            return;
        }

        //find all unspawned nodes
        GameObject[] unSpawnedNodes = gatherableNodes.Where(c => c.GetComponent<GatherableNodeManager>().isSpawned == false).ToArray<GameObject>();

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
                unSpawnedNodes[randomList[i]].GetComponent<GatherableNodeManager>().spawnNode();
            }
        }


    }
}
