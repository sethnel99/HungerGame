using UnityEngine;

public class ArrowItem : EquipmentItem {

    public ArrowItem(int q) : base("Arrow", "Arrows", 3, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.secondary_equipable;
        useText = "(Equip) Allows you to fire an equipped bow.";
        descriptText = "A crudely made arrow.";
        icon = (Texture2D)UnityEngine.Resources.Load("arrow_icon");
	}

    public ArrowItem(ArrowItem other)
        : base(other) {
    }


    public override bool useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        return inventory.EquipItem(this);
    }
}
