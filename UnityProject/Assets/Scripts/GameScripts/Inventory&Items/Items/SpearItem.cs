using UnityEngine;
using System.Collections;

public class SpearItem : EquipmentItem {

    public SpearItem(int q) : base("Spear", "Spears", 8, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to attack with a spear.";
        icon = (Texture2D)Resources.Load("spear_icon");
	}

    public SpearItem(SpearItem other)
        : base(other) {
    }


    public override void useItem() {

    }
}
