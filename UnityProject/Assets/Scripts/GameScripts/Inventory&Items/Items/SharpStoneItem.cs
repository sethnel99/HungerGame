using UnityEngine;
using System.Collections;

public class SharpStoneItem : Item {

    public SharpStoneItem(int q)
        : base("Sharp Stone", "Sharp Stones", 2, q) {
        addItemType(Item.material);
        icon = (Texture2D)Resources.Load("stone_icon");
        Debug.Log(icon);

    }

    public SharpStoneItem(SharpStoneItem other)
        : base(other) {
    }

    public override void useItem() {

    }
}
