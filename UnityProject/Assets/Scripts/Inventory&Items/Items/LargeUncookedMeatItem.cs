using UnityEngine;
using System.Collections;

public class LargeUncookedMeatItem : Item {

    public LargeUncookedMeatItem(int q)
        : base("Large Piece of Uncooked Meat", "Large Pieces of Uncooked Meat", 8, q) {
        addItemType(Item.material);
        icon = (Texture2D)UnityEngine.Resources.Load("large_meat_icon");
        descriptText = "A large, bloody hunk of meat.";
        

    }

    public LargeUncookedMeatItem(LargeUncookedMeatItem other)
        : base(other) {
    }

    public override bool useItem() {
        return false;
    }
}
