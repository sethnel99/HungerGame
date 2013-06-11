using UnityEngine;
using System.Collections;

public class RoofPlankItem : EquipmentItem {

    public RoofPlankItem(int q)
        : base("Roof Plank", "Roof Planks", 2, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to place a roof panel for building structures.";
        descriptText = "A horizontal plank used to construct a roof.";
        icon = (Texture2D)UnityEngine.Resources.Load("roof_plank_icon");
	}

    public RoofPlankItem(RoofPlankItem other)
        : base(other) {
    }


    public override bool useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        return inventory.EquipItem(this);
    }
}
