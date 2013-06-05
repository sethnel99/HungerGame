using UnityEngine;
using System.Collections;

//causes boots to have their intended effects
public class SpeedManager : MonoBehaviour {

    CharacterController controller;
    CharacterMotor motor;
    Inventory inventory;
    PlayerVitals vitals;

	// Use this for initialization
	void Start () {
        controller = gameObject.transform.root.GetComponent<CharacterController>();
        motor = gameObject.transform.root.GetComponent<CharacterMotor>();
        inventory = gameObject.transform.root.GetComponent<Inventory>();
        vitals = gameObject.transform.root.GetComponent<PlayerVitals>();
	}
	
	// Update is called once per frame
	void Update () {

        //check if the player's legs are injured
        int injuredLegs = 0;
        if (vitals.bodyPartHealth[(int)PlayerVitals.BodyPart.LeftLeg] <= 0) {
            injuredLegs++;
        }

        if (vitals.bodyPartHealth[(int)PlayerVitals.BodyPart.RightLeg] <= 0) {
            injuredLegs++;
        }

        //if both of the player's legs are injured, he can't walk
        if (injuredLegs == 2) {
            motor.movement.maxForwardSpeed = 1;
            motor.movement.maxSidewaysSpeed = 1;
            motor.movement.maxBackwardsSpeed = 1;
            return;
        }


        //check if the player is wearing boots
        BootsItem boots = inventory.getEquippedBoots() as BootsItem;
        if (boots != null) {
            motor.movement.maxForwardSpeed = 8 * (1 + boots.statPower/50);
            motor.movement.maxSidewaysSpeed = 8 * (1 + boots.statPower/50);
            motor.movement.maxBackwardsSpeed = 8 * (1 + boots.statPower/50);
            //controller.slopeLimit = 45 * 1 + boots.statPower / 50;
        } else {
            motor.movement.maxForwardSpeed = 8;
            motor.movement.maxSidewaysSpeed = 8;
            motor.movement.maxBackwardsSpeed = 8;
            //controller.slopeLimit = 45.0f;
        } 

        //if one of the player's legs is injured, he's hurt
        if (injuredLegs == 1) {
            motor.movement.maxForwardSpeed /= 2;
            motor.movement.maxSidewaysSpeed /= 2;
            motor.movement.maxBackwardsSpeed /= 2;
        } 
	}
}
