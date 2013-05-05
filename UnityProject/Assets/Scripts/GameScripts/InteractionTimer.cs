using UnityEngine;
using System.Collections;

public class InteractionTimer : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//set the timer to a specific level 0.0f-1.0f full.
	public void setInteractionTimerLevel(float level){
		//special case: level 0 = turn off the display
		if(level == 0 && transform.root.camera.enabled){
			//Debug.Log ("disabling camera");
			transform.root.camera.enabled = false;
			return;
		}else if (level == 0){
			return;
		}
		
		//turn on the display if it is off
		if(transform.root.camera.enabled == false){
			//Debug.Log ("enabling camera");
			transform.root.camera.enabled = true;
		}
		renderer.material.SetFloat ("_Cutoff",Mathf.InverseLerp(1.0f,0.0f,level));
	}
}
