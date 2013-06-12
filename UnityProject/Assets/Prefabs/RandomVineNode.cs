using UnityEngine;
using System.Collections;

public class RandomVineNode : MonoBehaviour {

	public GameObject[] Vines;
	
	private System.Random rnd;
	private int vine;
	
	// Use this for initialization
	void Start () {
		RandomVineManager parent = (RandomVineManager)(transform.parent.GetComponent("RandomVineManager"));
		rnd = parent.rand;
		vine = rnd.Next(0, Vines.Length-1);
		transform.position -= new Vector3(0f, .5f, 0f);
		transform.Rotate(new Vector3(0, rnd.Next(0, 360), 0));
		Instantiate(Vines[vine], transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
