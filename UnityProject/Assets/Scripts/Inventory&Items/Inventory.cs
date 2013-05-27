using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
	
/*
 * This class represents a players inventory. The inventory itself is a dictionary of items, where the keys are 
 * the names of an item (i.e. an inventory can contain only unique names), and the values are Item objects themselves.
 * The inventoryNamesArray is an array of strings which represents the order of the inventory. These strings can 
 * be used to index into the dictionary to access the actual Item object.
 */
public class Inventory : MonoBehaviour{
	
	private Dictionary<string,Item> inventory = new Dictionary<string,Item>(); //an underlying dictionary from name->Item
	private string[] inventoryNamesArray = new string[20]; //an array representing the order of the inventory.
	
	public int maxSize = 20;

    private EquipmentItem equippedEquipable;
    private EquipmentItem equippedSecondaryEquipable;
    private EquipmentItem equippedJacket;
    private EquipmentItem equippedBoots;

    public Dictionary<Item, Item[]> craftingDictionary;

    public GUIText InventoryTextGUI;
    public AudioClip collectSound;

    EquippedItem equipItemScript;


    void Start() {
        initializeCraftingDictionary();
        equipItemScript = GameObject.Find("EquippedItem").GetComponent<EquippedItem>();
        equippedEquipable = new AxeItem(1);
	
    }

    void initializeCraftingDictionary() {
        craftingDictionary = new Dictionary<Item, Item[]>();

        craftingDictionary.Add(new SidePlankItem(1), new Item[] { new WoodItem(1) });
        craftingDictionary.Add(new RoofPlankItem(1), new Item[] { new WoodItem(1) });
        craftingDictionary.Add(new FireItem(1), new Item[] { new SharpStoneItem(2), new WoodItem(3)});
        craftingDictionary.Add(new SmallCookedMeatItem(1), new Item[] { new SharpStoneItem(1) });
        craftingDictionary.Add(new LargeCookedMeatItem(1), new Item[] { new LargeUncookedMeatItem(1) });
        craftingDictionary.Add(new LargeHideItem(1), new Item[] { new SmallHideItem(3) });
        craftingDictionary.Add(new StringItem(2), new Item[] { new SmallHideItem(1) });
        craftingDictionary.Add(new AxeItem(1), new Item[] { new SharpStoneItem(2), new WoodItem(3) });
        craftingDictionary.Add(new SpearItem(1), new Item[] { new SharpStoneItem(1), new WoodItem(5) });
        craftingDictionary.Add(new BowItem(1), new Item[] { new WoodItem(4), new StringItem(4)});
        craftingDictionary.Add(new ArrowItem(1), new Item[] { new WoodItem(1) });
    }
	
	void Update () {

	}
	
	
	//add an item. Item types, quantity, etc. expected on the item
	public bool addItem(Item i){
        if (i == null) {
            return false;
        }


        if (i.quantity == 0) {
            return false;
        }

       

		//if you already have this item in your inventory, utilize stacking
        if (inventory.ContainsKey(i.name)) {
            Debug.Log(i.name + " in addItem q: " + i.quantity.ToString());
            Item oldItemInstance = inventory[i.name];
            oldItemInstance.quantity += i.quantity;
 
        } else if(i is EquipmentItem && equippedEquipable != null &&  i.name.Equals(equippedEquipable.name)){
            equippedEquipable.quantity += i.quantity;
        } else if(i is EquipmentItem && equippedSecondaryEquipable != null && i.name.Equals(equippedSecondaryEquipable.name)){
            equippedEquipable.quantity += i.quantity;
        }else {

            

            //if the inventory is full, return false
            if (inventory.Count > maxSize) {
                return false;
            }


            //otherwise add the item to the dictionary
            inventory.Add(i.name, (Item)Activator.CreateInstance(i.GetType(), i));

            //find a location in the inventory array for it and add it to that as well
            for (int j = 0; j < inventoryNamesArray.Length; j++) {
                if (inventoryNamesArray[j] == null) {
                    inventory[i.name].inventoryLocation = j;
                    inventoryNamesArray[j] = i.name;
                    break;
                }
            }
        }

        InventoryTextGUI.gameObject.GetComponent<InventoryTextManager>().enqueueMessage("You collected " + i.quantity + " " + ((i.quantity > 1) ? i.pluralName : i.name));
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
		return true;
	}
	
	//remove an item from the inventory by name
	public bool removeItem(string name, int quantityToRemove){	
		
		//get the item out of the dictionary
		Item oldItemInstance = inventory[name];
		
		//if the item did not exist, return false
		if(oldItemInstance == null){
			return false;
		}
		
		//remove the quantity specified
		oldItemInstance.quantity -= quantityToRemove;
		
		//if you have no more of that item remaining, remove it from the hashset and name array
		if (oldItemInstance.quantity <= 0){
            inventory.Remove(oldItemInstance.name);
			inventoryNamesArray[oldItemInstance.inventoryLocation] = null;
		}
		
		return true;
			
	}

    //remove an item from the list by providing the item
    public bool removeItem(Item i) {
        return removeItem(i.name, i.quantity);
    }
	
	//checks if the given item name is contained in the array
	public bool contains(string itemName, int quantity = 0){
        Item item;
        if (!inventory.TryGetValue(itemName, out item)) {
            item = null;
        }

        return item != null && inventory[itemName].quantity >= quantity;
	}
	
	//returns the item at a specific location in the inventory
	public Item getItemAtLocation(int location){
        string n = inventoryNamesArray[location];
        if (n == null) {
            return null;
        }

		return inventory[n];
	}
	
	//moves an item to a specific location in the inventory. Swaps if that location is alrady occupied.
	public bool moveItem(string itemName, int newLocation){
		//check to make sure the new location is in bounds
		if(newLocation >= this.maxSize){
			return false;
		}
		
		//check to make sure this itemName is in the inventory to begin with
		Item targetItem = inventory[itemName];
		if(targetItem == null){
			return false;
		}
		
		//save old values
		string oldItemInNewLocation = inventoryNamesArray[newLocation];
		int oldLocationOfItem = targetItem.inventoryLocation;
		//update target item
		inventoryNamesArray[newLocation] = itemName;
		targetItem.inventoryLocation = newLocation;
		
		//update old item in that slot, if it exists
		if(oldItemInNewLocation != null){
			inventoryNamesArray[oldLocationOfItem] = oldItemInNewLocation;
			inventory[oldItemInNewLocation].inventoryLocation = oldLocationOfItem;
		}
		
		return true;

	}

    public bool useItem(Item i) {
        i.useItem();

        //equipped items don't get decremented when they are "used" (equipped)
        if (!(i is EquipmentItem)){
            return removeItem(i.name, 1);
        }

        return true;
    }


    /*Crafting Code*/

    public bool canCraft(Item targetCraft) {
        Item[] craftIngredients = craftingDictionary[targetCraft];

        //special case: meat must be cooked near a fire. Is the player near a fire?
        if (targetCraft is SmallCookedMeatItem || targetCraft is LargeCookedMeatItem){
            if (!GameObject.FindWithTag("Player").GetComponent<PlayerVitals>().IsNearFire()) {
                return false;
            }
        }

        for (int i = 0; i < craftIngredients.Length; i++) {
            if (!this.contains(craftIngredients[i].name, craftIngredients[i].quantity)) {
                //Debug.Log("cannot craft " + craftIngredients[i].name);
                
                return false;
            }
        }
        return true;
    }

    public bool craftItem(Item targetCraft) {
        if (!canCraft(targetCraft)) {
            return false;
        }

        Item[] ingredientsList = craftingDictionary[targetCraft];

        for (int i = 0; i < ingredientsList.Length; i++) {

            //Try to remove each item
            if (!removeItem(ingredientsList[i])) {
                //if for whatever reason that fails (it shouldn't), add the already removed items back and return false
                for (int j = 0; j < i; j++) {
                    addItem(ingredientsList[j]);
                }
                return false;
            }
        }

        //if you've made it this far, removing the ingredient items was successful. Add the crafted item and return true
        addItem(targetCraft);
        return true;
    }

    /*Equipment code*/

    public void EquipItem(EquipmentItem i) {

        //cut this item out of the inventory
        inventory.Remove(i.name);
        inventoryNamesArray[i.inventoryLocation] = null;

        EquipmentItem curEquipped;
        switch (i.equipmentType) {
            case EquipmentItem.EquipmentType.equipable:
                curEquipped = equippedEquipable;
                equippedEquipable = i;
                //Debug.Log(curEquipped.name + " " + curEquipped.quantity.ToString());
                addItem(curEquipped);


                if (i is AxeItem) {
                    equipItemScript.EquipItem((int)EquippedItem.Equippable.Axe);
                } else if (i is SidePlankItem) {
                    equipItemScript.EquipItem((int)EquippedItem.Equippable.Plank);
                } else if (i is RoofPlankItem) {
                    equipItemScript.EquipItem((int)EquippedItem.Equippable.RoofPiece1);
                }

                GameObject.Find("GUIButtons").GetComponent<OpenCloseGUIs>().disableNewItem();

                break;
            case EquipmentItem.EquipmentType.secondary_equipable:
                curEquipped = equippedSecondaryEquipable;
                equippedSecondaryEquipable = i;
                addItem(curEquipped);
                
                break;

            case EquipmentItem.EquipmentType.jacket:
                curEquipped = equippedJacket;
                equippedJacket = i;
                addItem(curEquipped);
                
                break;

            case EquipmentItem.EquipmentType.boots:
                curEquipped = equippedBoots;
                equippedBoots = i;
                addItem(curEquipped);
                
                break;


        }

    }

    public void decrementEquipable() {
        equippedEquipable.quantity--;
    }

    public void decrementSecondaryEquipable() {
        equippedSecondaryEquipable.quantity--;
    }

    public EquipmentItem getEquippedEquipable(){
        return equippedEquipable;
    }

    public EquipmentItem getEquippedSecondaryEquipable() {
        return equippedSecondaryEquipable;
    }

    public EquipmentItem getEquippedJacket() {
        return equippedJacket;
    }

    public EquipmentItem getEquippedBoots() {
        return equippedBoots;
    }





	
}
