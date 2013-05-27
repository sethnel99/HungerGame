using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour
{
	// The rows and dimensions of the box (only works for 4 and 6 right now, will change later)
	private int rows = 4;
	private int columns = 6;
	
	//Array which stores all of the inventoryinformation
	private GameObject[] InventoryIcons = new GameObject[24];
	private GameObject[] InventoryCount = new GameObject[24];
	
	private GUITexture InventoryOverlay;
	private Inventory InventoryInfo;
	Rect InventoryDimensions;
	Texture2D TxtArray;
	Font InventoryFont;
	
	// Use this for initialization
	void Start ()
	{
		InventoryOverlay = GameObject.Find("InventoryGUI").guiTexture;
		InventoryInfo = (Inventory) GameObject.Find("First Person Controller").GetComponent("Inventory");
				
		TxtArray = (Texture2D) Resources.Load("Icons/SlotUnusable");
		InventoryFont = (Font) Resources.Load ("OptimusPrinceps");
		
		PopulateInventory();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!InventoryOverlay.enabled)
		{
			for (int i = 0; i < 24; i++)
			{
				InventoryIcons[i].guiTexture.enabled = false;
				InventoryCount[i].guiText.enabled = false;
			}
		}
		else
		{
			for (int i = 0; i < 24; i++)
			{
				InventoryIcons[i].guiTexture.enabled = true;
				InventoryCount[i].guiText.enabled = true;
			}
			
			if (Input.GetMouseButtonDown(1))
			{
				int closest = FindClosestIcon(Input.mousePosition);
				
				Item currentItem = InventoryInfo.getItemAtLocation(closest);
				
				//INSERT CODE HERE FOR USING SOMETHING IN THE INVENTORY
				//Currently set to right click
			}
		}
	}
	
	//Populate the boxes initially with a GUITexture and a GUIText
	public void PopulateInventory()
	{
		Vector3 center = new Vector3(Screen.width/2, Screen.height/2, 0);
		Vector3 pixelInset = new Vector3(InventoryOverlay.pixelInset.x, InventoryOverlay.pixelInset.y, 0);
			
		InventoryDimensions =  new Rect (center.x + pixelInset.x, center.y + pixelInset.y, InventoryOverlay.pixelInset.width, InventoryOverlay.pixelInset.height);
		
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				int currentIndex = i * columns + j;
				
				float xCenter = 25 + InventoryDimensions.x + 80*j + 1;
				float yCenter = 80 * (rows-1) + 25 + InventoryDimensions.y - 80*i + 1;
				
				GameObject inventoryIcon = new GameObject();
				inventoryIcon.AddComponent("GUITexture");
				inventoryIcon.transform.localScale = Vector3.zero;
				inventoryIcon.transform.position = new Vector3(xCenter/Screen.width, yCenter/Screen.height, 0);
				inventoryIcon.guiTexture.pixelInset = new Rect(0,0,58,58);
				
				GameObject inventoryAmount = new GameObject();
				inventoryAmount.AddComponent("GUIText");
				inventoryAmount.guiText.font = InventoryFont;
				inventoryAmount.transform.localScale = Vector3.zero;
				inventoryAmount.transform.position = new Vector3(xCenter/Screen.width, yCenter/Screen.height, 1);
				inventoryAmount.guiText.pixelOffset = new Vector2(48, 14);
								
				
				if (currentIndex >= InventoryInfo.maxSize)
				{			
					inventoryIcon.guiTexture.texture = TxtArray;
				}
				else
				{
					Item currentItem = InventoryInfo.getItemAtLocation(currentIndex);
						
					if (currentItem != null)
					{
						Texture2D icon = (Texture2D) Resources.Load("Icons/" + currentItem.name);
							
						inventoryIcon.guiTexture.texture = icon;
						
						inventoryAmount.guiText.text = currentItem.quantity.ToString ();
					}
	
					
				}
				
				InventoryIcons[currentIndex] = inventoryIcon;
				InventoryCount[currentIndex] = inventoryAmount;
			}
		}		
	}
	
	//Updating an inventory slot with a new item
	public void UpdateIcon(Item item, int location)
	{
		Texture2D icon = (Texture2D) Resources.Load ("Icons/" + item.name);
		
		InventoryIcons[location].guiTexture.texture = icon;
		InventoryCount[location].guiText.text = item.quantity.ToString ();
	}
	
	//Updating an inventory slot with a new amount
	public void UpdateAmount(int num, int location)
	{
		InventoryCount[location].guiText.text = num.ToString();
	}
	
	//Helper function to find closest box on mouseclick
	public int FindClosestIcon(Vector3 mouseLocation)
	{
		float x = mouseLocation.x/Screen.width;
		float y = mouseLocation.y/Screen.height;
		
		int smallest = 0;
		float distance = float.MaxValue;
		
		for (int i = 0; i < InventoryIcons.Length; i++)
		{
			float currentDistance = Vector3.Distance(InventoryIcons[i].transform.position, new Vector3(x,y,0));
			if (currentDistance < distance)
			{
				smallest = i;
				distance = currentDistance;
			}
		}
		
		return smallest;
	}
}