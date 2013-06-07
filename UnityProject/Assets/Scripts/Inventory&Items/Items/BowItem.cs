using UnityEngine;
using System.Collections;

public class BowItem : EquipmentItem {

    public BowItem(int q) : base("Bow", "Bows", 3, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to attack with a bow (requires arrows).";
        descriptText = "A hunting bow.";
        icon = (Texture2D)UnityEngine.Resources.Load("bow_icon");
        statPower = 8f;
	}

    public BowItem(BowItem other)
        : base(other) {
    }


    public override void useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        inventory.EquipItem(this);
    }
}
