using UnityEngine;
using System.Collections;

public class LargeCookedMeatItem : Item {

    public LargeCookedMeatItem(int q)
        : base("Large Piece of Cooked Meat", "Large Pieces of Cooked Meat", 8, q) {
        addItemType(Item.food);
        icon = (Texture2D)Resources.Load("large_cooked_meat_icon");
        

    }

    public LargeCookedMeatItem(LargeCookedMeatItem other)
        : base(other) {
    }

    public override void useItem() {

    }
}
