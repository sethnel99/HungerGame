using UnityEngine;
using System.Collections;


public class WaterDrinkManager : ItemManager {
	
	// Use this for initialization
	override public void Start (){
        base.Start();
        displayName = "Fresh Water";

        numberOfItemsDropped = 0;
	}
	
	override public string collectMessage() {
        return "Fresh Water: Hold [E] to drink";
    }
	
	    // Update is called once per frame
    override public void Update() {
       
		if (inZone && Input.GetButton ("Interact")) {
			buttonDownTime += Time.deltaTime;
            if (!interactionTimer.GetComponent<InteractionTimer>().setInteractionTimerLevel(buttonDownTime / gatherTime, gameObject)) {
                buttonDownTime = 0f;
            }
			
			if (buttonDownTime > gatherTime) {
				GameObject player  = GameObject.FindWithTag("Player");

//                Item[] drops = chooseLoot();
//
//                for (int i = 0; i < drops.Length; i++) {
//                   
//                    player.GetComponent<Inventory>().addItem(drops[i]);
//                }
//
//                interactionManager.removePotentialInteractor(gameObject);
				player.GetComponent<PlayerVitals>().HealThirst(30f);
				buttonDownTime = 0f;
                interactionTimer.GetComponent<InteractionTimer>().setInteractionTimerLevel(0, gameObject);
				//Destroy (gameObject);
			}	
		}
		else {
			buttonDownTime = 0.0f;
            interactionTimer.GetComponent<InteractionTimer>().setInteractionTimerLevel(0, gameObject);
		}
	}
	
}
