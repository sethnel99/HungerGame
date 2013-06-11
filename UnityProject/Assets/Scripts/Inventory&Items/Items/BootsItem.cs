using UnityEngine;
using System.Collections;

public class BootsItem : EquipmentItem {

    public BootsItem(int q, int p)
        : base("Boots (+" + p.ToString() + ")", "Boots (+" + p.ToString() + ")", 6, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.boots;
        useText = "(Equip) Increases your movement speed by " + p.ToString() + ".";
        descriptText = "A pair of patchy boots.";
        icon = (Texture2D)UnityEngine.Resources.Load("boots_icon");
        statPower = p;
	}

    public BootsItem(BootsItem other)
        : base(other) {

    }


    public override bool useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        return inventory.EquipItem(this);
    }
}
