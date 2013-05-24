using UnityEngine;
using System.Collections;

public class IGUI : MonoBehaviour {

    Inventory inventory;

    public GUISkin inventorySkin;
    public Rect inventoryRect;

    Rect inventoryRectNormalized;

    public Texture2D inventoryOverlay;

    // Use this for initialization
    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        inventoryRectNormalized = normalizeRect(inventoryRect);
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

        GUI.EndGroup();
    }

    Rect normalizeRect(Rect screenRect) {
        return new Rect(screenRect.x * Screen.width - (screenRect.width * 0.5f), screenRect.y * Screen.height - (screenRect.height * 0.5f), screenRect.width, screenRect.height);
    }

  
}
