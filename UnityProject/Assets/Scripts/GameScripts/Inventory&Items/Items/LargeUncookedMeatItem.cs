using UnityEngine;
using System.Collections;

public class LargeUncookedMeatItem : Item {

    public LargeUncookedMeatItem(int q)
        : base("Large Piece of Uncooked Meat", "Large Pieces of Uncooked Meat", 8, q) {
        addItemType(Item.material);
        icon = (Texture2D)Resources.Load("large_meat_icon");
        

    }

    public LargeUncookedMeatItem(LargeUncookedMeatItem other)
        : base(other) {
    }

    public override void useItem() {

    }
}
