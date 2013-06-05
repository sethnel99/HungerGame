using UnityEngine;
using System.Collections;

public class PlayerVitals : MonoBehaviour
{
	
	public enum BodyPart
	{
		Head,
		Torso,
		LeftArm,
		RightArm,
		LeftLeg,
		RightLeg
	}
	
	public float[] bodyPartHealth = new float[]{100.0f,500.0f,100.0f,100.0f,200.0f,200.0f};
    public float[] bodyPartMaxHealth = new float[] { 100.0f, 500.0f, 100.0f, 100.0f, 200.0f, 200.0f };

	private const float MaxHealth = 100;
	private const float MaxThirst = 100;
	
	public float CurrentHealth = 100;
	public float CurrentThirst = 100;
	
	public float CurrentTemp;     //current temperature (takes into account clothing)
    public float CurrentBaseTemp; //current base temperature
	
	private bool isNearFire = false;
	private skydomeScript2 GameTime;
	
	private Inventory inventory;
    private EquippedItemManager equippedItemScript;
    private GameObject textHints;
	
	// Use this for initialization
	void Start ()
	{
		GameTime = (skydomeScript2) GameObject.Find("Skybox Controller").GetComponent("skydomeScript2");
		
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        equippedItemScript = GameObject.Find("EquippedItem").GetComponent<EquippedItemManager>();
        textHints = GameObject.Find("TextHintGUI");

		CurrentTemp = 91.0f;
		CurrentBaseTemp = 91.0f;
    }
	
	// Update is called once per frame
	void Update ()
	{
		
		//Debug.Log (CurrentHealth);
		GameTime.TIME+= 0.001f;
		
		if (GameTime.TIME >= 23)
			GameTime.TIME = 0;
		
		if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
		{
			CurrentHealth -= 0.2f * Time.deltaTime;
            //CurrentThirst -= 0.4f * Time.deltaTime;
		}
		else
		{
            CurrentHealth -= 0.1f * Time.deltaTime;
           // CurrentThirst -= 0.2f * Time.deltaTime;
		}

        if (CurrentHealth < 0) Die();
        if (CurrentThirst < 0) Die();
		
		
		
		 //jackets increase temperature
        JacketItem j = inventory.getEquippedJacket() as JacketItem;
        if (j != null){
            CurrentTemp = CurrentBaseTemp + j.statPower;
        }
		
		
	}
	
	public float GetHealthRatio()
	{
		return CurrentHealth/MaxHealth;
	}
	
	public float GetThirstRatio()
	{
		return CurrentThirst/MaxThirst;
	}
	
	public void setIsNearFire(bool nearFire)
	{
		isNearFire = nearFire;
	}
	
	public bool IsNearFire()
	{
		return isNearFire;
	}
	
	public void HealThirst(float amount)
	{
		CurrentThirst += amount;
        if (CurrentThirst > MaxThirst) CurrentThirst = MaxThirst;
	}

    public void HealHunger(float amount) {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
    }
	
	public void TakenHit(BodyPart b, EnemyAttackBox eab)
	{
		
		//if this body part is dead, it is dead
		if(bodyPartHealth[(int)b] <= 0){
			return;
		}
		
		//subtract health
		bodyPartHealth[(int)b] -= eab.Damage;
		
		//if this body part crosses into 0, do what you will
		if(bodyPartHealth[(int)b] <= 0){
            bodyPartDeath(b);
		}
		
		//Debug.Log ("health of " + b + " is now " + bodyPartHealth[(int)b]);
	}
	
	void bodyPartDeath(BodyPart b){

		if(b == BodyPart.Head || b == BodyPart.Torso){
			Die ();
        } else if (b == BodyPart.RightArm) {
            //cannot hold main equippable
            inventory.unequipSlot(EquipmentItem.EquipmentType.equipable);
            equippedItemScript.UnequipItem();
        } else if (b == BodyPart.LeftArm) {
            //cannot hold secondary equippable
            inventory.unequipSlot(EquipmentItem.EquipmentType.secondary_equipable);
        }
	}
	
	void Die(){
		GameObject.Find ("GUIButtons").GetComponent<OpenCloseGUIs>().disableControls(this);
        GameObject.Find("GUIButtons").SetActive(false);
        GameObject.Find("StatusHUD").SetActive(false);
        StartCoroutine(FadeToBlack());
        textHints.SendMessage("ShowHint", "You are dead.");
	}

    IEnumerator FadeToBlack() {
        GameObject fade = new GameObject();
        fade.AddComponent<GUITexture>();
        fade.guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);

        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        fade.guiTexture.texture = tex;

        for (float alpha = 0.0f; alpha < 1.0f; ) {
            alpha += Time.deltaTime / 2;
            fade.guiTexture.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            yield return 0;
        }
    }
	   
}




