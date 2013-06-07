using UnityEngine;
using System.Collections;

public class KnifeItem : EquipmentItem {

    public KnifeItem(int q) : base("Knife", "Knifes", 8, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to attack with a Knife.";
        descriptText = "Your bread and butter weapon.";
        icon = (Texture2D)UnityEngine.Resources.Load("Knife_icon");
        statPower = 10f;
	}

    public KnifeItem(KnifeItem other)
        : base(other) {
    }


    public override void useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        inventory.EquipItem(this);
    }
}
