using UnityEngine;
using System.Collections;

public class SmallCookedMeatItem : Item {

    public SmallCookedMeatItem(int q)
        : base("Small Piece of Cooked Meat", "Small Pieces of Cooked Meat", 4, q) {
        addItemType(Item.food);
        icon = (Texture2D)UnityEngine.Resources.Load("small_cooked_meat_icon");
        

    }

    public SmallCookedMeatItem(SmallCookedMeatItem other)
        : base(other) {
    }

    public override void useItem() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVitals>().HealHunger(25.0f);
    }
}
