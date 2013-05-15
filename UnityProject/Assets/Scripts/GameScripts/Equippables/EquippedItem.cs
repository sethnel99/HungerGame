using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquippedItem : MonoBehaviour {
	
	public enum Equippable{
		Axe,
		Plank,
		RoofPiece1,
		MAX
	};
	
	public GameObject[] equippableItems;
	public GameObject equippedItem;
	
	public Vector3[] equippableTransform;
	public Vector3[] equippableRotate;
	
	public bool hasFirstStar=false;
	
	// Use this for initialization
	void Start () {
		equippableTransform = new Vector3[(int)Equippable.MAX];
		equippableRotate = new Vector3[(int)Equippable.MAX];
		
		equippableTransform[(int)Equippable.Axe] = new Vector3(.4f,-1f, 1f);
		equippableRotate[(int)Equippable.Axe] = new Vector3(12,90,0);
		
		equippableTransform[(int)Equippable.Plank] = new Vector3(0f,-.18f,2.8f);
		equippableRotate[(int)Equippable.Plank] = new Vector3(0,0,0);
		
		equippableTransform[(int)Equippable.RoofPiece1] = new Vector3(0f,-.18f,2.8f);
		equippableRotate[(int)Equippable.RoofPiece1] = new Vector3(0,0,0);
		
		//EquipItem((int)Equippable.Plank);
		EquipItem((int)Equippable.Axe);
	}

	
	
	// Update is called once per frame
	void Update () {
		if(hasFirstStar)
		{
			if(Input.GetButtonDown("1"))
			{
				EquipItem((int)EquippedItem.Equippable.Plank);
			}
			else if(Input.GetButtonDown("2"))
			{
				EquipItem((int)EquippedItem.Equippable.RoofPiece1);
			}
		}
	}
	
	public void EquipItem (int equippable){
		if(equippedItem != null)
		{
			GameObject.Destroy(equippedItem);
		}
		equippedItem = Instantiate(equippableItems[equippable], Camera.main.transform.position, Camera.main.transform.rotation) as GameObject;
		
		Reset(equippable);
		
//		equippedItem.transform.parent = gameObject.transform;
//		
//		equippedItem.transform.rotation = transform.rotation;
//		if(equippableRotate[equippable] != null)
//		{
//			equippedItem.transform.Rotate(equippableRotate[equippable]);
//		}
		
		//equippedItem.transform.localScale = Vector3.one;	
		//equippedItem.transform.position = new Vector3(0, -.8f, 0);
	}
	
	public void Reset(int equippable){
		if(equippedItem != null)
		{
			
			equippedItem.transform.parent = Camera.main.transform;
			equippedItem.transform.Translate(equippableTransform[equippable]);
			//equippedItem.transform.rotation = transform.rotation;
			//equippedItem.transform.rotation = transform.rotation;
			equippedItem.transform.Rotate(equippableRotate[equippable]);
			
		}
	}
}
