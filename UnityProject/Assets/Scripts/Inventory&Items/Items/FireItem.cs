using UnityEngine;
using System.Collections;

public class FireItem : Item {

    public FireItem(int q)
        : base("Fire", "Fire", 4, q) {
        addItemType(Item.tool);
        icon = (Texture2D)UnityEngine.Resources.Load("fire_icon");
        usable = true;
        useText = "Create a fire in front of your location";
        descriptText = "A fire, waiting to be deployed.";

	}

    public FireItem(FireItem other)
        : base(other) {
    }

    public override bool useItem() {
       // Debug.Log("making a fire");
        GameObject fire = UnityEngine.Resources.Load("Fire") as GameObject;

        //find player location
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        fire.transform.position = playerPosition + 3 * GameObject.FindGameObjectWithTag("Player").transform.forward + new Vector3(0f, -1.2f, 0f);
        GameObject.Instantiate(fire);

        return true;
       
    }
}
