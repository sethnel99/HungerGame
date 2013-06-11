using UnityEngine;
using System.Collections;

public class SmallUncookedMeatItem : Item {

    public SmallUncookedMeatItem(int q)
        : base("Small Piece of Uncooked Meat", "Small Pieces of Uncooked Meat", 4, q) {
        addItemType(Item.material);
        icon = (Texture2D)UnityEngine.Resources.Load("small_meat_icon");
        descriptText = "A small, bloody hunk of meat.";

    }

    public SmallUncookedMeatItem(SmallUncookedMeatItem other)
        : base(other) {
    }

    public override bool useItem() {
        return false;
    }
}
