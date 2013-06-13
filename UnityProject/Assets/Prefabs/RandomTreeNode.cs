using UnityEngine;
using System.Collections;

public class RandomTreeNode : MonoBehaviour {
	
	
	public GameObject[] Trees;
	
	private System.Random rnd;
	private int tree;
	
	// Use this for initialization
	void Start () {
		RandomTreeManager parent = (RandomTreeManager)(transform.parent.GetComponent("RandomTreeManager"));
		rnd = parent.rand;
		tree = rnd.Next(0, Trees.Length-1);
		transform.position -= new Vector3(0f, 2.2f, 0f);
		transform.Rotate(new Vector3(0, rnd.Next(0, 360), 0));
		GameObject newTree= (GameObject)Instantiate(Trees[tree], transform.position, transform.rotation);
        newTree.transform.parent = transform;
		newTree.transform.localScale = new Vector3(2, 2, 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
