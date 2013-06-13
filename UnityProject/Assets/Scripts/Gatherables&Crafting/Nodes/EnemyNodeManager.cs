using UnityEngine;
using System.Collections;
using System;

public class EnemyNodeManager : MonoBehaviour {
    public enum NodeType{
        Dog, Lizard, Random
    }

    public NodeType nodeType = NodeType.Dog;
     NodeType nodeToCreate = NodeType.Dog;

    static string[] nodeTypes = {"Dog", "Lizard"};
    
    public bool isSpawned = true;
    private GameObject enemy;

    public float respawnTime;
    public float respawnTimer = 0.0f;

    InteractionManager interactionManager;
    skydomeScript2 timeScript;

	// Use this for initialization
	void Start () {
        respawnTime = UnityEngine.Random.Range(45, 60);
        interactionManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionManager>();      
        timeScript = GameObject.Find("Skybox Controller").GetComponent<skydomeScript2>();

        isSpawned = true;
        nodeToCreate = nodeType;
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
            interactionManager.removePotentialInteractor(gameObject);
            GameObject.Destroy(enemy);
            return true;
        }

        return false;
    }

    public bool spawnNode() {
         bool isNightTime = (timeScript.TIME < 7.2 || timeScript.TIME >= 18.2);

         if (nodeType == NodeType.Random) {
             int roll = UnityEngine.Random.Range(1, 100);
             nodeToCreate = (NodeType)Enum.GetValues(typeof(NodeType)).GetValue((roll < 10 || roll < 20 && isNightTime) ? 1 : 0);
         } else {
             nodeToCreate = nodeType;
         }

            isSpawned = true;
            enemy = Instantiate(UnityEngine.Resources.Load(nodeTypes[(int)nodeToCreate])) as GameObject;

            if (isNightTime) {
                enemy.SendMessage("setNightime");
            }
			
            enemy.transform.parent = transform;
            enemy.transform.localPosition = new Vector3(0,1,0);
            enemy.transform.localRotation = Quaternion.Euler(enemy.transform.localRotation.eulerAngles.x, UnityEngine.Random.Range(0, 360), enemy.transform.localRotation.eulerAngles.z);
            return true;
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "PlayerEnabler") {
            if (isSpawned) {
                spawnNode();
            }
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "PlayerEnabler") {
            if (enemy != null) {
                Destroy(enemy);
            }
        }
    }
}
