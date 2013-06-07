using UnityEngine;
using System.Collections;

public class SmallHideItem : Item {

    public SmallHideItem(int q)
        : base("Small Hide", "Small Hides", 2, q) {
        addItemType(Item.material);
        icon = (Texture2D)UnityEngine.Resources.Load("small_hide_icon");
        descriptText = "The hide of a small animal.";

    }

    public SmallHideItem(SmallHideItem other)
        : base(other) {
    }

    public override void useItem() {

    }
}
