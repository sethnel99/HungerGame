using UnityEngine;
using System.Collections;

//causes boots to have their intended effects
public class BootsEffects : MonoBehaviour {

    CharacterController controller;
    CharacterMotor motor;
    Inventory inventory;

	// Use this for initialization
	void Start () {
        controller = gameObject.transform.root.GetComponent<CharacterController>();
        motor = gameObject.transform.root.GetComponent<CharacterMotor>();
        inventory = gameObject.transform.root.GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
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
	}
}
