using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingGUI : MonoBehaviour {
	


    Inventory inventory;
	
	public GUISkin craftingSkin;
	public Rect craftingRect;
	Rect craftingRectNormalized;
	Rect craftDetailRectNormalized;
	
	Item selectedItem;
    Item currentlyCrafting;

    Texture2D selectedLabelTexture;
    Texture2D arrowTexture;
    Texture2D craftProgressBarTexture;

     float craftProgress = 0.0f;
     public float craftTime = 1.0f;
	
	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;
        

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        selectedLabelTexture = (Texture2D)Resources.Load("SelectedLabel");
        arrowTexture = (Texture2D)Resources.Load("Arrow_2");
        craftProgressBarTexture = (Texture2D)Resources.Load("CraftProgressBar");

        IEnumerator enumerator = inventory.craftingDictionary.Keys.GetEnumerator();
        enumerator.MoveNext();
        selectedItem = (Item)enumerator.Current;

        //Debug.Log(selectedItem);

		craftingRectNormalized = normalizeRect (craftingRect);
		craftDetailRectNormalized = new Rect(craftingRect.width/4,0,craftingRect.width*3/4,craftingRect.height);
	}

    
	// Update is called once per frame
	void Update () {
        if (craftProgress > craftTime) {
            inventory.craftItem(selectedItem);
            craftProgress = 0.0f;
        } else if(craftProgress > 0.0f){
            craftProgress += Time.deltaTime;
        }
	}
	
          


	Vector2 scrollPosition = Vector2.zero;

    GUIStyle chooseLabelStyle(Item key) {
        Color cannotCraftColor =  new Color(255.0f / 255, 40.0f / 255, 51.0f / 255, 1.0f);

        GUIStyle cannotCraftLabelStyle = new GUIStyle(GUI.skin.label);
        cannotCraftLabelStyle.normal.textColor =cannotCraftColor;

        GUIStyle canCraftLabelStyle = new GUIStyle(GUI.skin.label);


        GUIStyle selectedCannotCraftLabelStyle = new GUIStyle(GUI.skin.label);
        selectedCannotCraftLabelStyle.normal.background = selectedLabelTexture;
        selectedCannotCraftLabelStyle.normal.textColor = cannotCraftColor;

        GUIStyle selectedCanCraftLabelStyle = new GUIStyle(GUI.skin.label);
        selectedCanCraftLabelStyle.normal.background = selectedLabelTexture;

        bool craftable = inventory.canCraft(key);

        if (selectedItem == key && craftable) {
            return selectedCanCraftLabelStyle;
        } else if(selectedItem == key){
            return selectedCannotCraftLabelStyle;
        }else if(craftable){
            return canCraftLabelStyle;
        } else {
            return cannotCraftLabelStyle;
        }
    }
	
	void OnGUI(){
		GUI.skin = craftingSkin;
		
		//set up custom styles
        

        GUIStyle quantityLabelStyle = new GUIStyle(GUI.skin.label);
        quantityLabelStyle.fontSize = 20;


		
		
		//Crafting GUI
		GUI.BeginGroup(craftingRectNormalized);
		
		//Left scroll
		scrollPosition = GUI.BeginScrollView (new Rect (0,0,craftingRect.width/4,craftingRect.height),scrollPosition, new Rect (0, 0,craftingRect.width/4-1, inventory.craftingDictionary.Keys.Count * 22));
        
		int listYLoc = 20;
        foreach (KeyValuePair<Item, Item[]> entry in inventory.craftingDictionary)
		{

            if (GUI.Button(new Rect(10, listYLoc, craftingRect.width / 4 - 20, 22), entry.Key.name, chooseLabelStyle(entry.Key))) {
				selectedItem = entry.Key;
			}
			

			listYLoc += 22;
		}
        // End the scroll view that we began above.
        GUI.EndScrollView ();
        
        //right display
        GUI.BeginGroup (craftDetailRectNormalized);
        GUI.Box(new Rect(0,0,craftDetailRectNormalized.width,craftDetailRectNormalized.height),selectedItem.name);


        Item[] ingredientsList = inventory.craftingDictionary[selectedItem];
        int numIcons = ingredientsList.Length + 2;
        float paddingPixels = .1f * craftDetailRectNormalized.width / 2;
        float pixelsPerIcon = .9f * craftDetailRectNormalized.width / numIcons;
        float paddingWithinIconSection = (pixelsPerIcon - 80) / 2;

        //draw the ingredients
        int i = 0;
        for (; i < ingredientsList.Length; i++) {
               GUI.DrawTexture(new Rect(paddingPixels + paddingWithinIconSection + i*pixelsPerIcon, craftDetailRectNormalized.height/3, 80, 80), ingredientsList[i].icon);
               GUI.Label(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon + 69, craftDetailRectNormalized.height / 3 + 58, 30, 30), ingredientsList[i].quantity.ToString(), quantityLabelStyle);
        }

        //draw the arrow
        GUI.DrawTexture(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon, craftDetailRectNormalized.height / 3, 80, 80), arrowTexture);
        i++;
        //draw the crafted result
        GUI.DrawTexture(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon, craftDetailRectNormalized.height / 3, 80, 80), selectedItem.icon);
        GUI.Label(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon + 69, craftDetailRectNormalized.height / 3 + 58, 30, 30), selectedItem.quantity.ToString(), quantityLabelStyle);

        //draw the craft button
        if (!inventory.canCraft(selectedItem)) {
            GUI.enabled = false;
        }

        if (GUI.Button(new Rect(craftDetailRectNormalized.width - 127, craftDetailRectNormalized.height - 37, 120, 30), "Craft!")) {
            Debug.Log("attempted craft");
            craftProgress += Time.deltaTime;
            currentlyCrafting = selectedItem;
        }
        GUI.enabled = true;

        //draw the craft progress bar, if applicable
        GUI.DrawTexture(new Rect(craftDetailRectNormalized.width/2 - 120, craftDetailRectNormalized.height - 60, 200 * craftProgress/craftTime, 30), craftProgressBarTexture);
		
        GUI.EndGroup();
        


        GUI.EndGroup();
		
	}
	
	Rect normalizeRect(Rect screenRect){
		return new Rect(screenRect.x * Screen.width - (screenRect.width * 0.5f), screenRect.y * Screen.height - (screenRect.height * 0.5f), screenRect.width, screenRect.height);
	}
}
