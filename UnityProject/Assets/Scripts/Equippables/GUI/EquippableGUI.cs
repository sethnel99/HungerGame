using UnityEngine;
using System.Collections;

public class EquippableGUI : MonoBehaviour {

    Inventory inventory;

    public GUISkin inventorySkin;
    public Rect inventoryRect;

    Rect inventoryRectNormalized;

    public Texture2D inventoryOverlay;
	
	private PlayerVitals PlayerData;
	
	public GUIStyle IncreaseStat;
	public GUIStyle GoodStatus;
	public GUIStyle BadStatus;

    // Use this for initialization
    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        inventoryRectNormalized = normalizeRect(inventoryRect);
		
		PlayerData = (PlayerVitals) GameObject.Find("First Person Controller").GetComponent ("PlayerVitals");
		
		
		IncreaseStat.normal.textColor = new Color(0,40,0,1);
		GoodStatus.normal.textColor = Color.white;
		BadStatus.normal.textColor = Color.red;
		
		this.enabled = false;
    }

    //60x60 icons, 20 in betweeen, 25 on sides
    void OnGUI() {
        GUI.skin = inventorySkin;

        GUI.BeginGroup(inventoryRectNormalized);
		{
			GUI.DrawTexture(new Rect(0, 0, inventoryRectNormalized.width, inventoryRectNormalized.height), inventoryOverlay);
			
			Rect leftbox = new Rect(0, 0, 255, 350);
			
			Debug.Log (Input.mousePosition);
			
			GUI.BeginGroup(leftbox);
			{
				if (inventory.getEquippedEquipable() != null)
				{
					GUI.Button(new Rect(180, 146, 58, 58), inventory.getEquippedEquipable().icon);
				}
				
				//Secondary
				if (inventory.getEquippedSecondaryEquipable() != null)
				{
					GUI.Button(new Rect(166, 27, 58, 58), inventory.getEquippedSecondaryEquipable().icon);
				}
				
				//Jacket
				if (inventory.getEquippedJacket() != null)
				{
					GUI.Button(new Rect(15, 146, 58, 58), inventory.getEquippedJacket().icon);
				}
				
				//boot
				if (inventory.getEquippedBoots() != null)
				{
					GUI.Button(new Rect(166, 265, 58, 58), inventory.getEquippedBoots().icon);
				}
			}
			GUI.EndGroup ();
			
	        Rect rightbox = new Rect(255, 0, 255, 350);
						
			GUI.BeginGroup(rightbox);
			{
				GUI.Label (new Rect(5,5, 255, 20), "Hunger: " + PlayerData.CurrentHunger.ToString("##.") + "/" + PlayerData.GetMaxHunger(), GetTextColor (PlayerData.GetHealthRatio()));
				GUI.Label (new Rect(5,25, 255, 20), "Thirst: " + PlayerData.CurrentThirst.ToString("##.") + "/" + PlayerData.GetMaxThirst(), GetTextColor (PlayerData.GetThirstRatio()));
				GUI.Label (new Rect(5,45, 255, 20), "Temperature: " + PlayerData.CurrentTemp.ToString("##.#") + " F", GetTextColor (PlayerData.CurrentTemp/100f));
				GUI.Label (new Rect(5,65, 255, 20), "Attack: " + (inventory.getEquippedEquipable() == null ? 0 : inventory.getEquippedEquipable().statPower), GoodStatus);
				GUI.Label (new Rect(5,85, 255, 20), "Armor: " + (inventory.getEquippedSecondaryEquipable() == null ? 0 : inventory.getEquippedSecondaryEquipable().statPower), GoodStatus);
				GUI.Label (new Rect(5,105, 255, 20), "Warmth: " + (inventory.getEquippedJacket() == null ? 0 : inventory.getEquippedJacket().statPower), GoodStatus);
				GUI.Label (new Rect(5,125, 255, 20), "Speed: " + (inventory.getEquippedBoots() == null ? 0 : inventory.getEquippedBoots().statPower), GoodStatus);
				GUI.Label (new Rect(5,145, 255, 20), "Head: " + PlayerData.bodyPartHealth[0] + "/100", GetTextColor (PlayerData.bodyPartHealth[0]/100f));
				GUI.Label (new Rect(5,165, 255, 20), "Torso: " + PlayerData.bodyPartHealth[1] + "/500", GetTextColor (PlayerData.bodyPartHealth[1]/500f));
				GUI.Label (new Rect(5,185, 255, 20), "Left Arm: " + PlayerData.bodyPartHealth[2] + "/100", GetTextColor (PlayerData.bodyPartHealth[2]/100f));
				GUI.Label (new Rect(5,205, 255, 20), "Right Arm: " + PlayerData.bodyPartHealth[3] + "/100", GetTextColor (PlayerData.bodyPartHealth[3]/100f));
				GUI.Label (new Rect(5,225, 255, 20), "Left Leg: " + PlayerData.bodyPartHealth[4] + "/200" , GetTextColor (PlayerData.bodyPartHealth[4]/200f));
				GUI.Label (new Rect(5,245, 255, 20), "Right Leg: " + PlayerData.bodyPartHealth[5] + "/200", GetTextColor (PlayerData.bodyPartHealth[5]/200f));
				
			}
			GUI.EndGroup();
		}
        GUI.EndGroup ();
    }

    Rect normalizeRect(Rect screenRect) {
        return new Rect(screenRect.x * Screen.width - (screenRect.width * 0.5f), screenRect.y * Screen.height - (screenRect.height * 0.5f), screenRect.width, screenRect.height);
    }
	
	GUIStyle GetTextColor(float ratio)
	{
		if (ratio > 0.3)
			return GoodStatus;
		else
			return BadStatus;
	}
}
