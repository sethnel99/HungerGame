using UnityEngine;
using System.Collections;

public class InteractionTextManager : MonoBehaviour {
	
	float timer = 0.0f;
	float targetTime = 0.0f;
	int countWithin = 0; //to handle the case where there are overlapping items, we don't want to prematurely remove text
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(guiText.enabled && targetTime > 0){
			timer+= Time.deltaTime;
			
			if(timer>=targetTime){
				guiText.enabled = false;
				timer =0.0f;
			}
		}
	}
	
	public void showMessage(string message, int time=0){
		guiText.text = message;
		targetTime = time;
		countWithin++;
		
		if(!guiText.enabled){
			guiText.enabled=true;
		}
		
	}
	
	public void hideMessage(){
		countWithin--;
		if(guiText.enabled && countWithin == 0){
			guiText.enabled = false;
			targetTime = 0.0f;
		}
	}
}
