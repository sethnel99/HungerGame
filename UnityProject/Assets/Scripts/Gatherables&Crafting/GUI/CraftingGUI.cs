using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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

     public static string meatMessage = "You must be near a fire to cook meat";
	
	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;
        

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

		selectedLabelTexture = (Texture2D)UnityEngine.Resources.Load("SelectedLabel");
        arrowTexture = (Texture2D)UnityEngine.Resources.Load("Arrow_2");
        craftProgressBarTexture = (Texture2D)UnityEngine.Resources.Load("CraftProgressBar");

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

    GUIStyle chooseLeftLabelStyle(Item key) {
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

    GUIStyle chooseQuantityLabelStyle(Item ingredient) {
        Color cannotCraftColor = new Color(255.0f / 255, 40.0f / 255, 51.0f / 255, 1.0f);

        GUIStyle cannotCraftLabelStyle = new GUIStyle(GUI.skin.label);
        cannotCraftLabelStyle.normal.textColor = cannotCraftColor;
        cannotCraftLabelStyle.fontSize = 20;

        GUIStyle canCraftLabelStyle = new GUIStyle(GUI.skin.label);
        canCraftLabelStyle.fontSize = 20;

        //for quantity of result
        if (ingredient == null) {
            return canCraftLabelStyle;
        }

        return inventory.contains(ingredient.name, ingredient.quantity) ? canCraftLabelStyle : cannotCraftLabelStyle;

    }

    GUIStyle chooseMeatWarningLabelStyle() {
         Color cannotCraftColor = new Color(255.0f / 255, 40.0f / 255, 51.0f / 255, 1.0f);

        GUIStyle cannotCraftLabelStyle = new GUIStyle(GUI.skin.label);
        cannotCraftLabelStyle.normal.textColor = cannotCraftColor;

        GUIStyle canCraftLabelStyle = new GUIStyle(GUI.skin.label);

        return GameObject.FindWithTag("Player").GetComponent<PlayerVitals>().IsNearFire() ? canCraftLabelStyle : cannotCraftLabelStyle;
    }
	
	void OnGUI(){
		GUI.skin = craftingSkin;
		
		//set up custom styles
        
		
		
		//Crafting GUI
		GUI.BeginGroup(craftingRectNormalized);
		
		//Left scroll
		scrollPosition = GUI.BeginScrollView (new Rect (0,0,craftingRect.width/4,craftingRect.height),scrollPosition, new Rect (0, 0,craftingRect.width/4-1, inventory.craftingDictionary.Keys.Count * 22));
        
		int listYLoc = 20;
        foreach (KeyValuePair<Item, Item[]> entry in inventory.craftingDictionary)
		{

            int yHeight = entry.Key.name.Length > 16 ? 36 : 22;

            if (GUI.Button(new Rect(10, listYLoc, craftingRect.width / 4 - 20, yHeight), entry.Key.name, chooseLeftLabelStyle(entry.Key))) {
				selectedItem = entry.Key;
			}


            listYLoc += yHeight;
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
               GUI.Label(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon + 69, craftDetailRectNormalized.height / 3 + 58, 30, 30), ingredientsList[i].quantity.ToString(), chooseQuantityLabelStyle(ingredientsList[i]));
        }

        //draw the arrow
        GUI.DrawTexture(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon, craftDetailRectNormalized.height / 3, 80, 80), arrowTexture);
        i++;
        //draw the crafted result
        GUI.DrawTexture(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon, craftDetailRectNormalized.height / 3, 80, 80), selectedItem.icon);
        GUI.Label(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon + 69, craftDetailRectNormalized.height / 3 + 58, 30, 30), selectedItem.quantity.ToString(), chooseQuantityLabelStyle(null));

        //special case: the selected craft item is a cooked meat, add a label telling the player they need to be near fire
        if (selectedItem is SmallCookedMeatItem || selectedItem is LargeCookedMeatItem) {
            Rect labelRect = GUILayoutUtility.GetRect(new GUIContent(meatMessage), "label");
            GUI.Label(new Rect(craftDetailRectNormalized.width / 2 - labelRect.width / 2, craftDetailRectNormalized.height - 100, labelRect.width, labelRect.height), meatMessage, chooseMeatWarningLabelStyle());
        }


        //draw the craft button
        if (!inventory.canCraft(selectedItem)) {
            GUI.enabled = false;
        }

        if (GUI.Button(new Rect(craftDetailRectNormalized.width - 127, craftDetailRectNormalized.height - 37, 120, 30), "Craft!")) {
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
