using UnityEngine;
using System.Collections;

public class IGUI : MonoBehaviour {

    Inventory inventory;

    public GUISkin inventorySkin;
    public Rect inventoryRect;

    Rect inventoryRectNormalized;

    public Texture2D inventoryOverlay;
	
	public Texture2D background;

    // Use this for initialization
    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        inventoryRectNormalized = normalizeRect(inventoryRect);
		
		background = new Texture2D(1, 1, TextureFormat.RGB24, false);
		background.SetPixel(0, 0, Color.gray);
		background.Apply();
    }

    // Update is called once per frame
    void Update() {

    }

    //60x60 icons, 20 in betweeen, 25 on sides
    void OnGUI() {
        GUI.skin = inventorySkin;

        GUI.BeginGroup(inventoryRectNormalized);


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
					Rect box = new Rect(mouseX + 10, mouseY + 10, 180, 150);
					
					GUI.BeginGroup(box);
					{
						GUI.DrawTexture(new Rect(0, 0, box.width, box.height), background, ScaleMode.StretchToFill);
						GUI.Label (new Rect(5,5, 180, 20), "Name: " + item.name);
						GUI.Label (new Rect(5,25, 180, 20), "Quantity: " + item.quantity.ToString ());
						GUI.Label (new Rect(5,45, 180, 20), "Weight: " + (item.quantity * item.weight).ToString () + " (" + item.weight.ToString() + ")");
						GUI.Label (new Rect(5,65, 180, 20), "Usable: " + item.usable.ToString());
						GUI.Label (new Rect(5,85, 180, 60), "Description: " + item.useText);
					}
					GUI.EndGroup();
				}
            }
			
        }
		
        GUI.EndGroup();
    }

    Rect normalizeRect(Rect screenRect) {
        return new Rect(screenRect.x * Screen.width - (screenRect.width * 0.5f), screenRect.y * Screen.height - (screenRect.height * 0.5f), screenRect.width, screenRect.height);
    }

  
}
