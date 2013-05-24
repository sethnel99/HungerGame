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
                enableControls(craftingGUI);
                craftingGUI.enabled = false;
            } else {
                disableControls(craftingGUI);
                craftingGUI.enabled = true;
            }
            debounceTimer += Time.deltaTime;
        } else if (Input.GetButton("InventoryGUI")) {
            if (inventoryGUI.enabled) {
                enableControls(inventoryGUI);
                inventoryGUI.enabled = false;
            } else {
                disableControls(inventoryGUI);
                inventoryGUI.enabled = true;
            }
            debounceTimer += Time.deltaTime;
        } else if (Input.GetButton("esc")) {
            if (inventoryGUI.enabled) {
                enableControls(inventoryGUI);
                inventoryGUI.enabled = false;
            } else if (craftingGUI.enabled) {
                enableControls(craftingGUI);
                craftingGUI.enabled = false;
            }
        }
	}

    public void disableControls(MonoBehaviour disabler) {
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


        if (disabler is CraftingGUI){
            inventoryGUI.enabled = false;
        }else{
            craftingGUI.enabled = false;
        }
    }

    public void enableControls(MonoBehaviour enabler) {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = true;
        GameObject.Find("EquippedItem").GetComponent<EquippedItem>().enabled = true;

        //Debug.Log("enable controls");

        if (disabledItem != null) {
            disabledItem.active = true;

            //if you click on the button with an axe equipped, it tries to start its animation and gets stuck. Reset it.
            if (disabledItem.tag.Equals("Axe")) {
                disabledItem.SendMessage("resetIsInAction");
            }

            disabledItem = null;
        }

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
