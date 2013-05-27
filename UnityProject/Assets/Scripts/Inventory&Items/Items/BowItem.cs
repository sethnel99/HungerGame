using UnityEngine;
using System.Collections;

public class BowItem : EquipmentItem {

    public BowItem(int q) : base("Bow", "Bows", 3, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to attack with a bow (requires arrows).";
        icon = (Texture2D)Resources.Load("bow_icon");
	}

    public BowItem(BowItem other)
        : base(other) {
    }


    public override void useItem() {

    }
}
