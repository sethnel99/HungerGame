using UnityEngine;
using System.Collections;
using System;

public class GatherableNodeManager : MonoBehaviour {
    public enum NodeType{
        Tree, Sharp_Stone, BerryBush, Random
    }

    public NodeType nodeType = NodeType.Random;

    static string[] nodeTypes = {"Tree", "Sharp_Stone", "BerryBush" };
    
    public bool isSpawned = false;
    private GameObject gatherable;
   


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool removeNode() {
        if (isSpawned) {
            isSpawned = false;
            GameObject.Destroy(gatherable);
            return true;
        }

        return false;
    }

    public bool spawnNode() {
        NodeType nodeToCreate;

        if (nodeType == NodeType.Random) {
            nodeToCreate = (NodeType)Enum.GetValues(typeof(NodeType)).GetValue(UnityEngine.Random.Range(0, 3));
        } else {
            nodeToCreate = nodeType;
        }

       // Debug.Log((int)nodeToCreate);

        if (!isSpawned) {
            isSpawned = true;
            gatherable = Instantiate(Resources.Load(nodeTypes[(int)nodeToCreate])) as GameObject;
            gatherable.transform.parent = transform;
            gatherable.transform.localPosition = Vector3.zero;
            float xz_scale = UnityEngine.Random.Range(.5f, .9f);
            gatherable.transform.localScale = Vector3.Scale(gatherable.transform.localScale, new Vector3(xz_scale, UnityEngine.Random.Range(.5f, .9f), xz_scale));
            gatherable.transform.localRotation = Quaternion.Euler(gatherable.transform.localRotation.eulerAngles.x, UnityEngine.Random.Range(0, 360), gatherable.transform.localRotation.eulerAngles.z);
            gatherable.GetComponent<ItemManager>().interactionTimer = GameObject.FindGameObjectWithTag("InteractionTimer");

            return true;
        }

        return false;
    }
}
