using UnityEngine;
using System.Collections;


public class TreeManager : ItemManager {
	
	// Use this for initialization
	new void Start () {
        base.Start();
        displayName = "Tree";

        lootTables = new Item[1][];
        lootTables[0] = new Item[3];

        lootTables[0][0] = new WoodItem(2);
        lootTables[0][1] = new WoodItem(3);
        lootTables[0][2] = new WoodItem(4);


        lootFrequencyTables = new int[1][];
        lootFrequencyTables[0] = new int[3];

        lootFrequencyTables[0][0] = 20;
        lootFrequencyTables[0][1] = 60;
        lootFrequencyTables[0][2] = 20;

        numberOfItemsDropped = new int[1];
        numberOfItemsDropped[0] = 1;
	}


    public override string collectMessage() {
        return "Hold [E] to chop down the Tree";
    }


}
