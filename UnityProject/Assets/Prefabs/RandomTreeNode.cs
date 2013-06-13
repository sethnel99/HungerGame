using UnityEngine;
using System.Collections;

public class RandomTreeNode : MonoBehaviour {
	
	
	public GameObject[] Trees;
	
	private System.Random rnd;
	private int treeType;
    RandomTreeManager parent;
    GameObject tree;
	
	// Use this for initialization
	void Start () {
		parent = (RandomTreeManager)(transform.parent.GetComponent("RandomTreeManager"));
	}

    void SpawnTree() {
        rnd = parent.rand;
        treeType = rnd.Next(0, Trees.Length - 1);
        transform.position -= new Vector3(0f, 2.2f, 0f);
        transform.Rotate(new Vector3(0, rnd.Next(0, 360), 0));
        tree = (GameObject)Instantiate(Trees[treeType], transform.position, transform.rotation);
        tree.transform.parent = transform;
        tree.transform.localScale = new Vector3(2, 2, 2);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "PlayerTreeEnabler") {
                SpawnTree();
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "PlayerTreeEnabler") {
            if (tree != null) {
                Destroy(tree);
            }
        }
    }
}
