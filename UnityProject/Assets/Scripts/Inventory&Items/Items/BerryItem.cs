using UnityEngine;
using System.Collections;

public class BerryItem : Item {

    public BerryItem(int q) : base("Berry", "Berries", 1, q) {
        addItemType(Item.material);
        addItemType(Item.food);
        usable = true;
        useText = "Eat to regain a small amount of hunger points.";
        descriptText = "A berry you plucked from a bush.";
        
        icon = (Texture2D)UnityEngine.Resources.Load("berries_icon");
	}

    public BerryItem(BerryItem other)
        : base(other) {
    }


    public override bool useItem() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVitals>().HealHunger(4.0f);
        return true;
    }
}
