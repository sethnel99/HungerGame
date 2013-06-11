using UnityEngine;
using System.Collections;

public class FireManager : MonoBehaviour {

    public float fireTimeout = 120.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        fireTimeout -= Time.deltaTime;
        if (fireTimeout <= 0) {
            Destroy(gameObject);
        }
	}


    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.SendMessage("setIsNearFire", true);
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.SendMessage("setIsNearFire", false);
        }

    }

}
