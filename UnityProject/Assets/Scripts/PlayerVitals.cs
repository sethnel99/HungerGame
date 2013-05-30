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
	
	public float[] bodyPartHealth = new float[]{100.0f,100.0f,100.0f,100.0f,100.0f,100.0f};
	
	private const float MaxHealth = 100;
	private const float MaxThirst = 100;
	
	public float CurrentHealth = 100;
	public float CurrentThirst = 100;
	
	public float CurrentTemp;     //current temperature (takes into account clothing)
    public float CurrentBaseTemp; //current base temperature
	
	private bool isNearFire = false;
	private skydomeScript2 GameTime;
	
	private Inventory inventory;
	
	// Use this for initialization
	void Start ()
	{
		GameTime = (skydomeScript2) GameObject.Find("Skybox Controller").GetComponent("skydomeScript2");
		
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
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
			CurrentHealth -= 0.02f;
			CurrentThirst -= 0.04f;
		}
		else
		{
			CurrentHealth -= 0.01f;
			CurrentThirst -= 0.02f;
		}
		
		if (CurrentHealth < 0) CurrentHealth = 0;
		if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
		
		if (CurrentThirst < 0) CurrentThirst = 0;
		if (CurrentThirst > MaxThirst) CurrentThirst = MaxThirst;
		
		
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
	}

    public void HealHunger(float amount) {
        CurrentHealth += amount;
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
			
		}
		
		Debug.Log ("health of " + b + " is now " + bodyPartHealth[(int)b]);
	}
	
	void bodyPartDeath(BodyPart b){
		if(b == BodyPart.Head || b == BodyPart.Torso){
			Die ();	
		}
	}
	
	void Die(){
		GameObject.Find ("GUIButtons").GetComponent<OpenCloseGUIs>().disableControls(this);
		
	}
	   
}




