using UnityEngine;
using System.Collections;

public abstract class Item  {
	public static readonly uint equipment = 1; //mask for first bit
	public static readonly uint food = 1<<1; //mask for second bit
	public static readonly uint material = 1<<2; //mask for third bit
    public static readonly uint tool = 1 << 3; //mask for the fourth bit

    public bool usable = false; //whether or not this item is usable. If it is, it ought to have a use text.
	public int weight; //weight of the item
	public string name; //name of the item. Unique per item
	public string pluralName; //the plural of the name for the item
	private uint itemType; //a bit-string representing the types assocaited with the item
	public int quantity = 0; //the quantity of the item possessed
    public string useText; //If the item is usable, the text that describes what it does on use
	
	public int inventoryLocation; //this item's location in the inventory

    public Texture2D icon;
	
	//Constructor - requires a name, plural name, weight, and quantity for the item
	public Item(string n, string pn, int w, int q){
		name = n;
		pluralName = pn;
		weight = w;
		quantity = q;
	}

    //Copy constructor
    public Item(Item other) {
        usable = other.usable;
        weight = other.weight;
        name = other.name;
        pluralName = other.pluralName;
        itemType = other.itemType;
        quantity = other.quantity;
        useText = other.useText;
        inventoryLocation = other.inventoryLocation;
        icon = other.icon;

    }
	
	//add an item type to the item
	protected void addItemType(uint type){
		itemType = itemType | type;	
	}
	
	//remove an item type from the item
    protected void removeItemType(uint type) {
		itemType = itemType & ~type;	
	}
	
	//returns whether an item is a specific item type
	public bool isItemType(uint type){
		return (itemType & type) != 0;
	}

    public abstract void useItem(); 


	

}
