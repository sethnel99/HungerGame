using UnityEngine;
using System.Collections;

public class FireItem : Item {

    public FireItem(int q)
        : base("Fire", "Fire", 4, q) {
        addItemType(Item.tool);
        icon = (Texture2D)UnityEngine.Resources.Load("fire_icon");
        usable = true;
        useText = "Create a fire in front of your location";
	}

    public FireItem(FireItem other)
        : base(other) {
    }

    public override void useItem() {
        Debug.Log("making a fire");
        GameObject fire = UnityEngine.Resources.Load("Fire") as GameObject;

        //find player location
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        fire.transform.position = new Vector3(playerPosition.x + 3, playerPosition.y, playerPosition.z);
        GameObject.Instantiate(fire);
       
    }
}
