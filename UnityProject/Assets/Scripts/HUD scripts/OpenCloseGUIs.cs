using UnityEngine;
using System.Collections;

public class OpenCloseGUIs : MonoBehaviour {
    public CraftingGUI craftingGUI;
    public IGUI inventoryGUI;

    float debounceTime = .25f;
    float debounceTimer = 0f;

    GameObject disabledItem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (debounceTimer > debounceTime) {
            debounceTimer = 0f;
        } else if (debounceTimer > 0f) {
            debounceTimer += Time.deltaTime;
            return;
        }else if (Input.GetButton("CraftingGUI")) {
            if (craftingGUI.enabled) {
                enableControls("CraftingGUI");
                craftingGUI.enabled = false;
            } else {
                disableControls("CraftingGUI");
                craftingGUI.enabled = true;
            }
            debounceTimer += Time.deltaTime;
        } else if (Input.GetButton("InventoryGUI")) {
            if (inventoryGUI.enabled) {
                enableControls("InventoryGUI");
                inventoryGUI.enabled = false;
            } else {
                disableControls("InventoryGUI");
                inventoryGUI.enabled = true;
            }
            debounceTimer += Time.deltaTime;

        }
	}

    public void disableControls(string disabler) {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;

        //Debug.Log("disable controls");
        GameObject curItem = GameObject.FindGameObjectWithTag("Axe");
        if (curItem == null) {
            curItem = GameObject.FindGameObjectWithTag("BuildingGhost");
        }

        if (curItem != null && disabledItem == null) {
            curItem.active = false;
            disabledItem = curItem;
        }


        if (disabler.Equals("CraftingGUI")){
            inventoryGUI.enabled = false;
        }else{
            craftingGUI.enabled = false;
        }
    }

    public void enableControls(string enabler) {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = true;
        GameObject.Find("EquippedItem").GetComponent<EquippedItem>().enabled = true;

        //Debug.Log("enable controls");

        disabledItem.active = true;
        disabledItem = null;

    }

    public void disableNewItem() {

        
        GameObject curItem = GameObject.FindGameObjectWithTag("Axe");
        if (curItem == null) {
            curItem = GameObject.FindGameObjectWithTag("BuildingGhost");
        }

        if (curItem != null) {
            curItem.active = false;
            disabledItem = curItem;
        }
    }
	
}
