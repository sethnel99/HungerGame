using UnityEngine;
using System.Collections;

public class FireItem : Item {

    public FireItem(int q)
        : base("Fire", "Fire", 4, q) {
        addItemType(Item.tool);
        icon = (Texture2D)UnityEngine.Resources.Load("fire_icon");
	}

    public FireItem(FireItem other)
        : base(other) {
    }

    public override void useItem() {

    }
}
