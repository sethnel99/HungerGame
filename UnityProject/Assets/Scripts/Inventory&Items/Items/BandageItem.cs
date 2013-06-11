using UnityEngine;
using System.Collections;

public class BandageItem : Item {

    public BandageItem(int q)
        : base("Bandage", "Bandages", 2, q) {
        addItemType(Item.tool);
        icon = (Texture2D)UnityEngine.Resources.Load("bandage_icon");
        usable = true;
        useText = "Restores a broken limb to life.";
        descriptText = "A bandage, used for repairing broken limbs.";

	}

    public BandageItem(BandageItem other)
        : base(other) {
    }

    public override bool useItem() {
        bool bandageSuccess = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVitals>().bandage();
        if (!bandageSuccess) {
            GameObject.Find("TextHintGUI").SendMessage("ShowHint", "You have no broken limbs!");
        }

        return bandageSuccess;
    }
}
