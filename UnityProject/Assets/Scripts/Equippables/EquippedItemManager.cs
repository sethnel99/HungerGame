using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquippedItemManager : MonoBehaviour {
	
	public enum Equippable{
		Axe,
		Plank,
		RoofPiece1,
		Knife,
		Bow,
		Door,
		MAX
	};
	
	public GameObject[] equippableItems;
	public GameObject equippedItem;
	
	public Vector3[] equippableTransform;
	public Vector3[] equippableRotate;
	
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
		
		equippableTransform[(int)Equippable.Knife] = new Vector3(.5f,-.5f,1.3f);
		equippableRotate[(int)Equippable.Knife] = new Vector3(0,180,280);
		
		equippableTransform[(int)Equippable.Bow] = new Vector3(-.2f,-.05f,.55f);
		equippableRotate[(int)Equippable.Bow] = new Vector3(355,265,5);

		equippableTransform[(int)Equippable.Door] = new Vector3(-.85f,-1.8f,3.2f);
		equippableRotate[(int)Equippable.Door] = new Vector3(0,180,0);
		
	}

	
	
	// Update is called once per frame
	void Update () {
	}
	
	public void EquipItem (int equippable, float damage = 0){
		if(equippedItem != null)
		{
			GameObject.Destroy(equippedItem);
		}
   

		equippedItem = Instantiate(equippableItems[equippable], Camera.main.transform.position, Camera.main.transform.rotation) as GameObject;
        if (equippedItem.GetComponent("DamageDealer") != null) {
            (equippedItem.GetComponent("DamageDealer") as DamageDealer).damage = damage;
        }


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

    //unequips the current item
    public void UnequipItem() {
        if (equippedItem != null) {
            GameObject.Destroy(equippedItem);
        }

        return;
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
