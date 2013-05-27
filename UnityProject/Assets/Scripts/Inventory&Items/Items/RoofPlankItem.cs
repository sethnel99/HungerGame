using UnityEngine;
using System.Collections;

public class RoofPlankItem : EquipmentItem {

    public RoofPlankItem(int q)
        : base("Roof Plank", "Roof Planks", 2, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to place a roof panel for building structures.";
        icon = (Texture2D)Resources.Load("roof_plank_icon");
	}

    public RoofPlankItem(RoofPlankItem other)
        : base(other) {
    }


    public override void useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        inventory.EquipItem(this);
    }
}
