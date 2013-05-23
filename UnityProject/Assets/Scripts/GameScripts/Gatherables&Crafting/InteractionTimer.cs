using UnityEngine;
using System.Collections;

public class InteractionTimer : MonoBehaviour {

    InteractionManager interactionManager;
	
	// Use this for initialization
	void Start () {
	    interactionManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionManager>();;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	
	//set the timer to a specific level 0.0f-1.0f full. Returns true if successful (aka if the requestor is the interactTarget)
	public bool setInteractionTimerLevel(float level, GameObject interactor){
        if (interactor != interactionManager.interactTarget) {
            return false;
        }

		//special case: level 0 = turn off the display
		if(level == 0 && transform.root.camera.enabled){
			//Debug.Log ("disabling camera");
			transform.root.camera.enabled = false;
			return true;
		}else if (level == 0){
			return true;
		}
		
		//turn on the display if it is off
		if(transform.root.camera.enabled == false){
			//Debug.Log ("enabling camera");
			transform.root.camera.enabled = true;
		}
		renderer.material.SetFloat ("_Cutoff",Mathf.InverseLerp(1.0f,0.0f,level));
        return true;
	}
}
