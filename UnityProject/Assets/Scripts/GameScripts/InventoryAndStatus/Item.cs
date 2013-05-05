using UnityEngine;
using System.Collections;

public class Item  {
	public static readonly uint weapon = 1; //mask for first bit
	public static readonly uint food = 1<<1; //mask for second bit
	public static readonly uint material = 1<<2; //mask for third bit
	public static readonly uint tool = 1<<3; //mask for fourth bit
	public static readonly uint clothing = 1<<4; //mask for fifth bit
		
	public int weight; //weight of the item
	public string name; //name of the item. Unique per item
	public string pluralName; //the plural of the name for the item
	private uint itemType; //a bit-string representing the types assocaited with the item
	public int quantity = 0; //the quantity of the item possessed
	
	public int inventoryLocation; //this item's location in the inventory
	
	//Constructor - requires a name, plural name, weight, and quantity for the item
	public Item(string n, string pn, int w, int q){
		name = n;
		pluralName = pn;
		weight = w;
		quantity = q;
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
