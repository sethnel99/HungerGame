using UnityEngine;
using System.Collections;

public class StringItem : Item {

    public StringItem(int q)
        : base("Piece of String", "Pieces of String", 1, q) {
        addItemType(Item.food);
        icon = (Texture2D)UnityEngine.Resources.Load("string_icon");
        descriptText = "A piece of string, used to construct weapons.";
        

    }

    public StringItem(StringItem other)
        : base(other) {
    }

    public override void useItem() {

    }
}
