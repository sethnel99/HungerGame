using UnityEngine;
using System.Collections;

public class GUIButtons : MonoBehaviour {

    public GUISkin buttonSkin;

    public Texture2D craftingButton;
    public Texture2D inventoryButton;
    public Texture2D normalBackground;
    public Texture2D pressedBackground;

    CraftingGUI craftingDisplay;
    IGUI inventoryDisplay;

    OpenCloseGUIs openCloseGUIs;

	// Use this for initialization
	void Start () {
        craftingDisplay = GameObject.Find("CraftingGUI").GetComponent<CraftingGUI>();
        craftingDisplay.enabled = false;
        inventoryDisplay = GameObject.Find("InventoryGUI").GetComponent<IGUI>();
        inventoryDisplay.enabled = false;


        openCloseGUIs = GameObject.Find("GUIButtons").GetComponent<OpenCloseGUIs>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        GUI.skin = buttonSkin;

        GUI.BeginGroup(new Rect(Screen.width - 230, Screen.height - 110, 230, 110));

        if (GUI.Button(new Rect(0, 0, 100, 100), craftingButton)) {
            toggleGUIAndControls(craftingDisplay);
        }


        if (GUI.Button(new Rect(120, 0, 100, 100), inventoryButton)) {
            toggleGUIAndControls(inventoryDisplay);
        }



        GUI.EndGroup();

    }

    void toggleGUIAndControls(MonoBehaviour c) {
        Debug.Log(c);
        if (c.enabled) {
            openCloseGUIs.enableControls(c);
        } else {
            openCloseGUIs.disableControls(c);
        }
        c.enabled = !c.enabled;
    }


   
}
