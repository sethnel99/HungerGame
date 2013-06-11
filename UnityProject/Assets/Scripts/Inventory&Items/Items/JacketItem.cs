using UnityEngine;
using System.Collections;

public class JacketItem : EquipmentItem {

    public JacketItem(int q, int p)
        : base("Jacket (+" + p.ToString() + ")", "Jackets (+" + p.ToString() + ")", 8, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.jacket;
        useText = "(Equip) Increases your temperature by " + p.ToString() + ".";
        descriptText = "A warm winter jacket.";
        icon = (Texture2D)UnityEngine.Resources.Load("jacket_icon");
        statPower = p;
	}

    public JacketItem(JacketItem other)
        : base(other) {
    }


    public override bool useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        return inventory.EquipItem(this);
    }
}
