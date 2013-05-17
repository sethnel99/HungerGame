using UnityEngine;
using System.Collections;


public class TreeManager : ItemManager {
	
	// Use this for initialization
	new void Start () {
        base.Start();
        displayName = "Tree";

        lootTable = new Item[3];

        lootTable[0] = new WoodItem(2);
        lootTable[1] = new WoodItem(3);
        lootTable[2] = new WoodItem(4);


        lootFrequencyTable = new int[3];
        lootFrequencyTable[0] = 20;
        lootFrequencyTable[1] = 60;
        lootFrequencyTable[2] = 20;

        numberOfItemsDropped = 1;
	}


    public override string collectMessage() {
        return "Hold [E] to chop down the Tree";
    }


}
