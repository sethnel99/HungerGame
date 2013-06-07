using UnityEngine;
using System.Collections;

public class LargeHideItem : Item {

    public LargeHideItem(int q)
        : base("Large Hide", "Large Hides", 7, q) {
        addItemType(Item.material);
        icon = (Texture2D)UnityEngine.Resources.Load("large_hide_icon");
        descriptText = "The hide of a very large animal.";
        

    }

    public LargeHideItem(LargeHideItem other)
        : base(other) {
    }

    public override void useItem() {

    }
}
