using UnityEngine;
using System.Collections;

public class IGUI : MonoBehaviour {

    Inventory inventory;

    public GUISkin inventorySkin;
    public Rect inventoryRect;

    Rect inventoryRectNormalized;
    Rect inventoryRectNormalizedWithTooltipPadding;

    public Texture2D inventoryOverlay;
	
	public Texture2D tooltipBackground;

    // Use this for initialization
    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        inventoryRectNormalized = normalizeRect(inventoryRect);
        inventoryRectNormalizedWithTooltipPadding = new Rect(inventoryRectNormalized.x, inventoryRectNormalized.y, inventoryRectNormalized.width + 300, inventoryRectNormalized.height + 100);

        tooltipBackground = new Texture2D(1, 1, TextureFormat.RGB24, false);
        tooltipBackground.SetPixel(0, 0, Color.gray);
        tooltipBackground.Apply();
    }

    // Update is called once per frame
    void Update() {

    }

    //60x60 icons, 20 in betweeen, 25 on sides
    void OnGUI() {
        GUI.skin = inventorySkin;

        GUI.BeginGroup(inventoryRectNormalizedWithTooltipPadding);


        //draw background
        GUI.DrawTexture(new Rect(0,0,inventoryRectNormalized.width, inventoryRectNormalized.height), inventoryOverlay);

        for (int i = 0; i < inventory.maxSize; i++) {
            Item item = inventory.getItemAtLocation(i);
            if (item != null) {
                int row = i % 6;
                int column = i / 4;

                if (GUI.Button(new Rect(25 + row * 80, 25 + column * 80, 60, 60), item.icon)) {
                    if (item.usable) {
                        inventory.useItem(item);
                    } else {
                         GameObject.Find("GUIButtons").GetComponent<OpenCloseGUIs>().disableControls(null);
                         GameObject.Find("GUIButtons").GetComponent<OpenCloseGUIs>().setCraftingEnabled(true);
                    }
                }
				
                GUI.Label(new Rect(25 + row * 80 + 51, 25 + column * 80 + 43, 20, 20), item.quantity.ToString());


            }
			
        }
		
		for (int i = 0; i < inventory.maxSize; i++) {
            Item item = inventory.getItemAtLocation(i);
            if (item != null) {
                int row = i % 6;
                int column = i / 4;
				
				float mouseX = (Input.mousePosition.x - inventoryRectNormalized.x);
				float mouseY = (inventoryRectNormalized.y + inventoryRectNormalized.height - Input.mousePosition.y);
				
				
				if (mouseX >= (25 + row * 80) && mouseX <= (85 + row * 80) && mouseY >= (25 + column * 80) && mouseY <= (85 + column * 80))
				{
                    IGUI.drawTooltip(item, mouseX, mouseY, tooltipBackground);
				}
            }
			
        }
		
        GUI.EndGroup();
    }

    Rect normalizeRect(Rect screenRect) {
        return new Rect(screenRect.x * Screen.width - (screenRect.width * 0.5f), screenRect.y * Screen.height - (screenRect.height * 0.5f), screenRect.width, screenRect.height);
    }

    public static void drawTooltip(Item item, float mouseX, float mouseY, Texture2D background) {

        int useTextLines = (item.useText != null) ? (item.useText.Length + 5) / 26 + 1 : 0;
        int descriptTextLines = (item.descriptText != null) ? (item.descriptText.Length + 13) / 26 + 1 : 0;
        Rect box = new Rect(mouseX + 10, mouseY + 10, 210, 75 + 20 * (useTextLines + descriptTextLines));


        GUI.BeginGroup(box);
        {
            int curY = 0;
            GUI.DrawTexture(new Rect(0, curY, box.width, box.height), background, ScaleMode.StretchToFill); curY += 5;
            GUI.Label(new Rect(5, curY, 200, 20), "Name: " + item.name); curY += 20;
            GUI.Label(new Rect(5, curY, 200, 20), "Quantity: " + item.quantity.ToString()); curY += 20;
            GUI.Label(new Rect(5, curY, 200, 20), "Weight: " + (item.quantity * item.weight).ToString() + " (" + item.weight.ToString() + ")"); curY += 20;
            if (useTextLines > 0) {
                GUI.Label(new Rect(5, curY, 200, 20 * useTextLines), "Use: " + item.useText); curY += 20 * useTextLines;
            }
            if (descriptTextLines > 0) {
                GUI.Label(new Rect(5, curY, 200, 20 * descriptTextLines), "Description: " + item.descriptText); curY += 20 * descriptTextLines;
            }
        }
        GUI.EndGroup();


    }

  
}
