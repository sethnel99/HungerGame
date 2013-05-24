using UnityEngine;
using System.Collections;

public class SmallUncookedMeatItem : Item {

    public SmallUncookedMeatItem(int q)
        : base("Small Piece of Uncooked Meat", "Small Pieces of Uncooked Meat", 4, q) {
        addItemType(Item.material);
        icon = (Texture2D)Resources.Load("small_meat_icon");
        

    }

    public SmallUncookedMeatItem(LargeHideItem other)
        : base(other) {
    }

    public override void useItem() {

    }
}
