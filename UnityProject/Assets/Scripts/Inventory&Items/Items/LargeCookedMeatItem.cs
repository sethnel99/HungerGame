using UnityEngine;
using System.Collections;

public class LargeCookedMeatItem : Item {

    public LargeCookedMeatItem(int q)
        : base("Large Piece of Cooked Meat", "Large Pieces of Cooked Meat", 8, q) {
        addItemType(Item.food);
        icon = (Texture2D)UnityEngine.Resources.Load("large_cooked_meat_icon");
        

    }

    public LargeCookedMeatItem(LargeCookedMeatItem other)
        : base(other) {
    }

    public override void useItem() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVitals>().HealHunger(40.0f);
    }
}
