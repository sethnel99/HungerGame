using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
	
	Portal otherPortal;
	public bool ported;
	
	void Update() {
		if(gameObject.tag == "PortalA"){	
			otherPortal = (Portal)GameObject.FindWithTag("PortalB").GetComponent("Portal");
		}
		
		if(gameObject.tag == "PortalB"){
			otherPortal =(Portal)GameObject.FindWithTag("PortalA").GetComponent("Portal");
		}

		
	}
	
	void OnTriggerEnter (Collider col) {
		if(!ported){
    		col.gameObject.transform.position = otherPortal.transform.position;
			otherPortal.ported = true;
		}		
	}
	
	
	void OnTriggerExit (Collider col) {
		ported = false;
	}
	
}	