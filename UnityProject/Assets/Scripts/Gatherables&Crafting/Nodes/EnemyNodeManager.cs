using UnityEngine;
using System.Collections;
using System;

public class EnemyNodeManager : MonoBehaviour {
    public enum NodeType{
        Dog, Random
    }

    public NodeType nodeType = NodeType.Dog;

    static string[] nodeTypes = {"Dog"};
    
    public bool isSpawned = true;
    private GameObject enemy;

    public float respawnTime;
    public float respawnTimer = 0.0f;
   


	// Use this for initialization
	void Start () {
        spawnNode();
        respawnTime = UnityEngine.Random.Range(45, 60);
	}
	
	// Update is called once per frame
	void Update () {
        if (respawnTimer > 0) {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer <= 0) {
                Debug.Log("respawn a dog!");
                if (isSpawned)
                    removeNode();
                spawnNode();
                respawnTime = UnityEngine.Random.Range(45, 60);
            }
        }
	}

    public void startRespawnTimer() {
        respawnTimer = respawnTime;
    }

    public bool removeNode() {
        if (isSpawned) {
            isSpawned = false;
            GameObject.Destroy(enemy);
            return true;
        }

        return false;
    }

    public bool spawnNode() {
        /* NodeType nodeToCreate;

         if (nodeType == NodeType.Random) {
             nodeToCreate = (NodeType)Enum.GetValues(typeof(NodeType)).GetValue(UnityEngine.Random.Range(0, 1));
         } else {
             nodeToCreate = nodeType;
         }*/

        if (!isSpawned) {
            isSpawned = true;
            enemy = Instantiate(UnityEngine.Resources.Load(nodeTypes[(int)nodeType])) as GameObject;
			
            enemy.transform.parent = transform;
            enemy.transform.localPosition = Vector3.zero;
            enemy.transform.localRotation = Quaternion.Euler(enemy.transform.localRotation.eulerAngles.x, UnityEngine.Random.Range(0, 360), enemy.transform.localRotation.eulerAngles.z);
            return true;
        }

        return false;
    }
}
