using UnityEngine;
using System.Collections;

public class LargeCookedMeatItem : Item {

    public LargeCookedMeatItem(int q)
        : base("Large Piece of Cooked Meat", "Large Pieces of Cooked Meat", 8, q) {
        addItemType(Item.food);
        usable = true;
        useText = "Eat to regain a large amount of hunger points.";
        descriptText = "A large rack of ribs.";
        icon = (Texture2D)UnityEngine.Resources.Load("large_cooked_meat_icon");

        

    }

    public LargeCookedMeatItem(LargeCookedMeatItem other)
        : base(other) {
    }

    public override bool useItem() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVitals>().HealHunger(40.0f);
        return true;
    }
}
