using UnityEngine;
using System.Collections;

public class RandomVineNode : MonoBehaviour {

	public GameObject[] Vines;
	
	private System.Random rnd;
	private int vineType;
    RandomVineManager parent;
    GameObject vine;
	
	// Use this for initialization
	void Start () {
		parent = (RandomVineManager)(transform.parent.GetComponent("RandomVineManager"));
		
	}

    void SpawnVine() {
        rnd = parent.rand;
        vineType = rnd.Next(0, Vines.Length - 1);
        transform.position -= new Vector3(0f, .5f, 0f);
        transform.Rotate(new Vector3(0, rnd.Next(0, 360), 0));
        vine = Instantiate(Vines[vineType], transform.position, transform.rotation) as GameObject;
        vine.transform.parent = transform;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "PlayerTreeEnabler") {
            SpawnVine();
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "PlayerTreeEnabler") {
            if (vine != null) {
                Destroy(vine);
            }
        }
    }
}
