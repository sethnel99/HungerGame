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

	private const float MaxHunger = 100;
	private const float MaxThirst = 100;
	
	public float CurrentHunger = 50;
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
       //no sensing if the instructions screen is displayed
        if (GameObject.Find("Terrain").GetComponent("Instructions") != null && !GameObject.Find("Terrain").GetComponent<Instructions>().surviveText) {
            return;
        }

		if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
		{
			CurrentHunger -= 0.2f * Time.deltaTime;
            //CurrentThirst -= 0.4f * Time.deltaTime;
		}
		else
		{
            CurrentHunger -= 0.1f * Time.deltaTime;
           // CurrentThirst -= 0.2f * Time.deltaTime;
		}

        if (CurrentHunger < 0) Die("You died of hunger.");
        if (CurrentThirst < 0) Die("You died of thirst.");
		
		
		
		 //jackets increase temperature
        JacketItem j = inventory.getEquippedJacket() as JacketItem;
        if (j != null){
            CurrentTemp = CurrentBaseTemp + j.statPower;
        }

        //regenerate body part health slowly
        for (int i = 0; i < bodyPartHealth.Length; i++) {
            if (bodyPartHealth[i] > 0 && bodyPartHealth[i] < bodyPartMaxHealth[i]) {
                bodyPartHealth[i] += Time.deltaTime/4f * bodyPartMaxHealth[i]/100;
            }
        }
		
		
	}
	
	public float GetMaxHunger()
	{
		return MaxHunger;
	}

	
    public float GetHealthRatio()
	{
		return CurrentHunger/MaxHunger;
	}
	
	public float GetThirstRatio()
	{
		return CurrentThirst/MaxThirst;
	}
	
	public float GetMaxThirst()
	{
		return MaxThirst;
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
        CurrentHunger += amount;
        if (CurrentHunger > MaxHunger) CurrentHunger = MaxHunger;
    }

    public bool bandage() {
        //priority of bandaging -> right arm -> legs -> left arm
        if (bodyPartHealth[(int)BodyPart.RightArm] <= 0) {
            bodyPartHealth[(int)BodyPart.RightArm] = bodyPartMaxHealth[(int)BodyPart.RightArm] / 2;
            textHints.SendMessage("Show Hint", "Right Arm Repaired!");
            return true;
        } else if (bodyPartHealth[(int)BodyPart.LeftLeg] <= 0) {
            bodyPartHealth[(int)BodyPart.LeftLeg] = bodyPartMaxHealth[(int)BodyPart.LeftLeg] / 2;
            textHints.SendMessage("Show Hint", "Left Leg Repaired!");
            return true;
        } else if (bodyPartHealth[(int)BodyPart.RightLeg] <= 0) {
            bodyPartHealth[(int)BodyPart.RightLeg] = bodyPartMaxHealth[(int)BodyPart.RightLeg] / 2;
            textHints.SendMessage("Show Hint", "Right Leg Repaired!");
            return true;
        } else if (bodyPartHealth[(int)BodyPart.LeftArm] <= 0) {
            bodyPartHealth[(int)BodyPart.LeftArm] = bodyPartMaxHealth[(int)BodyPart.LeftArm] / 2;
            textHints.SendMessage("Show Hint", "Left Arm Repaired!");
            return true;
        } else{
            return false;
        }

    }

    public void TakenHit(BodyPart b, EnemyAttackBox eab)
	{
		
		//if this body part is dead, it is dead
		if(bodyPartHealth[(int)b] <= 0){
			return;
		}
		
		//subtract health
		bodyPartHealth[(int)b] -= eab.Damage * eab.damageMultiplier;
		
		//if this body part crosses into 0, do what you will
		if(bodyPartHealth[(int)b] <= 0){
            bodyPartDeath(b);
		}
		
		//Debug.Log ("health of " + b + " is now " + bodyPartHealth[(int)b]);
	}
	
	void bodyPartDeath(BodyPart b){

		if(b == BodyPart.Head || b == BodyPart.Torso){
			Die ("You died from " + ((b==BodyPart.Head) ? "head wounds." : "torso wounds."));
        } else if (b == BodyPart.RightArm) {
            //cannot hold main equippable
            inventory.unequipSlot(EquipmentItem.EquipmentType.equipable);
            equippedItemScript.UnequipItem();
        } else if (b == BodyPart.LeftArm) {
            //cannot hold secondary equippable
            inventory.unequipSlot(EquipmentItem.EquipmentType.secondary_equipable);
        }
	}
	
	void Die(string deathString){
		GameObject.Find ("GUIButtons").GetComponent<OpenCloseGUIs>().disableControls(this);
        GameObject.Find("GUIButtons").SetActive(false);
        GameObject.Find("StatusHUD").SetActive(false);
        StartCoroutine(FadeToBlack());
        textHints.SendMessage("ShowHint", deathString);
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




