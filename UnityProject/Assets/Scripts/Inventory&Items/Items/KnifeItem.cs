using UnityEngine;
using System.Collections;

public class KnifeItem : EquipmentItem {

    public KnifeItem(int q) : base("Knife", "Knifes", 8, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to attack with a Knife.";
        icon = (Texture2D)UnityEngine.Resources.Load("Knife_icon");
	}

    public KnifeItem(KnifeItem other)
        : base(other) {
    }


    public override void useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        inventory.EquipItem(this);
    }
}
