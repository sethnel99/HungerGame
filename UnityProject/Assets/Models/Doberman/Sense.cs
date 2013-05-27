using UnityEngine;
using System.Collections;

public class Sense : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider col) {
		
		if(col.gameObject.tag == "Player"){
			transform.parent.gameObject.SendMessage("SenseSomething", true);
		}
		
	}
	void OnTriggerExit(Collider col){
		if(col.gameObject.tag == "Player"){
			transform.parent.gameObject.SendMessage("SenseSomething", false);
		}
	}
}
