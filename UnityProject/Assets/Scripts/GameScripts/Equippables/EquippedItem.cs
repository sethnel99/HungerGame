using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquippedItem : MonoBehaviour {
	
	public enum Equippable{
		Axe,
		Plank,
		MAX
	};
	
	public GameObject[] equippableItems;
	public GameObject equippedItem;
	
	private Vector3[] equippableTransform;
	private Vector3[] equippableRotate;
	
	// Use this for initialization
	void Start () {
		equippableTransform = new Vector3[(int)Equippable.MAX];
		equippableRotate = new Vector3[(int)Equippable.MAX];
		
		equippableTransform[(int)Equippable.Axe] = new Vector3(.5f,-1f,.2f);
		equippableRotate[(int)Equippable.Axe] = new Vector3(15,90,0);
		
		equippableTransform[(int)Equippable.Plank] = new Vector3(-.32f,-.3f,1.8f);
		//equippableRotate[(int)Equippable.Plank] = new Vector3(0,0,0);
		
		//EquipItem((int)Equippable.Plank);
		EquipItem((int)Equippable.Axe);
	}

	
	
	// Update is called once per frame
	void Update () {
	}
	
	public void EquipItem (int equippable){
		if(equippedItem != null)
		{
			GameObject.Destroy(equippedItem);
		}
		equippedItem = Instantiate(equippableItems[equippable], transform.position+equippableTransform[equippable], Quaternion.identity) as GameObject;

		equippedItem.transform.parent = gameObject.transform;
		
		equippedItem.transform.rotation = transform.rotation;
		if(equippableRotate[equippable] != null)
		{
			equippedItem.transform.Rotate(equippableRotate[equippable]);
		}
		
		//equippedItem.transform.localScale = Vector3.one;	
		//equippedItem.transform.position = new Vector3(0, -.8f, 0);
	}
}
