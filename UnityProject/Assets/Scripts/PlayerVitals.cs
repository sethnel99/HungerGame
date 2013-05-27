using UnityEngine;
using System.Collections;

public class PlayerVitals : MonoBehaviour
{
	private const float MaxHealth = 100;
	private const float MaxThirst = 100;
	
	private float CurrentHealth = 100;
	private float CurrentThirst = 100;
	
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
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
	}
	
	public float GetHealthRatio()
	{
		return CurrentHealth/MaxHealth;
	}
	
	public float GetThirstRatio()
	{
		return CurrentThirst/MaxThirst;
	}
	
	public void HealThirst(float amount)
	{
		CurrentThirst+= amount;
	}
	
	
	public void TakenHit(BodyHit bodyHitObject)
	{
		Debug.Log(bodyHitObject.BodyPart + " " + bodyHitObject.Col.gameObject.name + " "+ bodyHitObject.Col.gameObject.GetComponent<EnemyAttackBox>().Damage );
	}
	
}

public class BodyHit
{
	public enum BodyPartEnum
	{
		Head,
		Torso,
		LeftArm,
		RightArm,
		LeftLeg,
		RightLeg
	}
	public BodyPartEnum BodyPart;
	public Collider Col;
	public BodyHit(BodyPartEnum bodyPart, Collider col)
	{
		BodyPart = bodyPart;
		Col = col;
	}
}

