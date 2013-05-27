using UnityEngine;
using System.Collections;

public class BerryItem : Item {

    public BerryItem(int q) : base("Berry", "Berries", 1, q) {
        addItemType(Item.material);
        addItemType(Item.food);
        usable = true;
        useText = "Eat to regain some health";
        icon = (Texture2D)UnityEngine.Resources.Load("berries_icon");
	}

    public BerryItem(BerryItem other)
        : base(other) {
    }


    public override void useItem() {

    }
}
