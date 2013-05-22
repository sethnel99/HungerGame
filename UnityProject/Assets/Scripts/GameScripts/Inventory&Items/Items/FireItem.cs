using UnityEngine;
using System.Collections;

public class FireItem : Item {

    public FireItem(int q)
        : base("Fire", "Fire", 4, q) {
        addItemType(Item.tool);
        icon = (Texture2D)Resources.Load("fire_icon");
        usable = true;
        useText = "Create a fire in front of your location";
	}

    public FireItem(FireItem other)
        : base(other) {
    }

    public override void useItem() {
        GameObject.Instantiate(Resources.Load("Fire"));
    }
}
