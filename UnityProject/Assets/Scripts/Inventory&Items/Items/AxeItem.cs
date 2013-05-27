using UnityEngine;
using System.Collections;

public class AxeItem : EquipmentItem {

    public AxeItem(int q) : base("Axe", "Axes", 5, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to attack with an axe.";
        icon = (Texture2D)Resources.Load("axe_icon");
	}

    public AxeItem(AxeItem other)
        : base(other) {
    }


    public override void useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        inventory.EquipItem(this);
    }
}
