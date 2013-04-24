using UnityEngine;
using System.Collections;

public class Item  {
	public static uint weapon = 0; //mask for first bit
	public static uint food = 2; //mask for second bit
	public static uint material = 4; //mask for third bit
	public static uint tool = 8; //mask for fourth bit
	public static uint clothing = 16; //mask for fifth bit
		
	public int weight; //weight of the item
	public string name; //name of the item. Unique per item
	private uint itemType; //a bit-string representing the types assocaited with the item
	public int quantity = 0; //the quantity of the item possessed
	
	public int inventoryLocation; //this item's location in the inventory
	
	//Constructor - requires a name and weight for the item
	public Item(string n, int w){
		name = n;
		weight = w;
	}
	
	//add an item type to the item
	public void addItemType(uint type){
		itemType = itemType | type;	
	}
	
	//remove an item type from the item
	public void removeItemType(uint type){
		itemType = itemType & ~type;	
	}
	
	//returns whether an item is a specific item type
	public bool isItemType(uint type){
		return (itemType & type) != 0;
	}

	

}
