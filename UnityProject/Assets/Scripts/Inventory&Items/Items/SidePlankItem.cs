using UnityEngine;
using System.Collections;

public class SidePlankItem : EquipmentItem {

    public SidePlankItem(int q) : base("Side Plank", "Side Planks", 2, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to place a side panel for building structures.";
        descriptText = "A vertical plank used to construct a wall.";
        icon = (Texture2D)UnityEngine.Resources.Load("side_plank_icon");
	}

    public SidePlankItem(SidePlankItem other)
        : base(other) {
    }


    public override void useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        inventory.EquipItem(this);
    }
}
