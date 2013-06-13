using UnityEngine;
using System.Collections;

public class DoorItem : EquipmentItem {

    public DoorItem(int q) : base("Door", "Doors", 1, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to place a door for building structures.";
        descriptText = "A wooden door.";
        icon = (Texture2D)UnityEngine.Resources.Load("door_icon");
	}

    public DoorItem(DoorItem other)
        : base(other) {
    }


    public override bool useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        return inventory.EquipItem(this);
    }
}
