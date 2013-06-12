using UnityEngine;
using System.Collections;
using Behave.Runtime;
using Tree = Behave.Runtime.Tree;

public class Lizard : MonoBehaviour, IAgent 
{
	Tree m_tree;
	public GameObject player;
	public GameObject swipe;
    public GameObject bite;
	
	private bool inAnimation; // are we already animating for a behavior?
	
	private float TickTime =0f; // time in between each AI tick
	
	private bool doneLooking = false;
	private float lookTimer = 0f;
	private float lookMaxTime = 5f;

	private bool doneWandering = false;
	private float wanderTimer = 0f;
	private float wanderMaxTime = 20f;

    private bool dead = false;
	
	// AI Vars
	float sightCastRadius = 6f;
	float sightDistance = 30;
	float attackRange = 7f;
    float senseRange = 20;
    float leashDistance = 80; //change back to 40

    public bool leashingBackToSpawn = false;

	public bool senseSomething =false;
	public bool seeTarget = false;
	public bool closeEnough = false;
	public bool aligned = false;

    public float recentlyAttackedTimer = 0.0f;
	// /AI Vars
	
	private Vector3[] PatrolPoints = new Vector3[3];
    private Vector3 spawnPoint;

	private float takingHitTimer = 0f;
	private float takingHitTimerMax = 1f;

    float animationMultiplier = 1.0f;
	
	IEnumerator Start () {
        spawnPoint = this.gameObject.transform.position;
        PatrolPoints[0] = spawnPoint + new Vector3(Random.Range(5,8), Random.Range(5,8), 0);
        PatrolPoints[1] = spawnPoint + new Vector3(Random.Range(-8,-5), Random.Range(5,8), 0);
        PatrolPoints[2] = spawnPoint + new Vector3(Random.Range(5,8), Random.Range(-8,-5), 0);

        player = GameObject.FindGameObjectWithTag("Player");
		
		m_tree = BLBehaveLibrary0.InstantiateTree(BLBehaveLibrary0.TreeType.EnemyBehaviors_PatrolerTree,this);

        slowDownAnimations(.5f);


        //Debug.Log(this.gameObject.animation.

		while(Application.isPlaying && m_tree != null)
		{	
			yield return new WaitForSeconds(1.0f / m_tree.Frequency/2);
			AIUpdate();
		}
	}

    void slowDownAnimations(float m) {
        animationMultiplier = m;
        foreach(AnimationState state in this.gameObject.animation){
            animation[state.name].speed = m;
        }
    }
	
	void AIUpdate()
	{
		RunChecks();
        if (m_tree != null) {
            m_tree.Tick();
        }
		TickTime = 0; //set our AI tick timer to 0 b/c we just ticked
	}
	
	void Update () {
		TickTime += Time.deltaTime;

        if(recentlyAttackedTimer > 0){
         recentlyAttackedTimer -= Time.deltaTime;
        }

		if(takingHitTimer>0)
		{
			takingHitTimer -= Time.deltaTime;
		}

        
	}
	
	// IAgent
	public BehaveResult Tick (Tree sender, bool init)
	{
		return BehaveResult.Success;
	}
	public void Reset(Tree sender)
	{
		
	}
	public int SelectTopPriority(Tree sender, params int[] IDs)
	{
		return IDs[0];
	}
	// /IAgent
	
	public BehaveResult TickExitIfDecorator(Tree sender, string exp, float floatParameter, IAgent agent, object data)
	{
		if(EvaluateExpression(exp))
		{
			inAnimation = false;
			return BehaveResult.Failure;
		}
		{
			return BehaveResult.Success;
		}
	}
	
//	public BehaveResult TickCheckAction(Tree sender)
//	{
//		RunChecks();
//		
//		return BehaveResult.Success;	
//	}
	
	void RunChecks()
	{
        //no sensing if the instructions screen is displayed
        if (GameObject.Find("Terrain").GetComponent("Instructions") != null && !GameObject.Find("Terrain").GetComponent<Instructions>().surviveText) {
            return;
        }

        float distanceFromSpawn = Vector3.Distance(new Vector3(transform.position.x,transform.position.y,0), new Vector3(spawnPoint.x,spawnPoint.y,0));
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
       // Debug.Log("distance from spawn: " + distanceFromSpawn + "and leashing: " + leashingBackToSpawn);
        
        //leash to spawn if the lizard is too far away from spawn or too far away from the player. But don't leash if he's been attacked recently.
        if (recentlyAttackedTimer <= 0  && (distanceFromSpawn > leashDistance || (seeTarget && distanceFromPlayer > leashDistance) || (leashingBackToSpawn && distanceFromSpawn > 8.0f))) {
            senseSomething = false;
            seeTarget = false;
            closeEnough = false;
            aligned = false;
            leashingBackToSpawn = true;
            //Debug.Log("leashing back to spawn");
            return;
        } else if (leashingBackToSpawn && distanceFromSpawn < 8.0f) {
            Debug.Log("FBGM");
            gameObject.animation.Play("Roar");
            inAnimation = false;
            leashingBackToSpawn = false;
        } 


        if(!seeTarget){

            senseSomething = distanceFromPlayer <= senseRange;

            if (senseSomething) {

                RaycastHit[] hits = Physics.SphereCastAll(transform.position - 10 * transform.forward, sightCastRadius, transform.forward, sightDistance);
                foreach (RaycastHit hit in hits) {
                    if (hit.collider != null && hit.collider.gameObject.tag == "Player") {
                        seeTarget = true;
                    }
                }
            }

            aligned = false;
            closeEnough = false;

        }else{
	    		closeEnough = distanceFromPlayer <= attackRange;
                Vector3 dirToPlayer = Vector3.Normalize(player.transform.position - transform.position);
                dirToPlayer.y = 0;
                Vector3 lizardForward = this.gameObject.transform.forward;
                lizardForward.y = 0;
                aligned = Vector3.Angle(dirToPlayer, lizardForward) < 20;
                //Debug.Log("lizard to player: " + dirToPlayer + " lizard forward: " + lizardForward + " Angle: " + Vector3.Angle(dirToPlayer, lizardForward));
	  }


        //Debug.Log("Sense: " + senseSomething + " See Target: " + seeTarget + " close Enough: " + closeEnough + " aligned: " + aligned + " leash: " + leashingBackToSpawn + "recentlyAttacked: " + recentlyAttackedTimer);

	}
	
	
	public BehaveResult InitLookAroundAction(Tree sender)
	{
		inAnimation = false;
		return BehaveResult.Running;
	}
	public BehaveResult InitWanderAction(Tree sender)
	{
		inAnimation = false;
		return BehaveResult.Running;
	}
	public BehaveResult InitAttackAction(Tree sender)
	{
		inAnimation = false;
		return BehaveResult.Running;
	}
	public BehaveResult InitCloseDistanceAndAlignAction(Tree sender)
	{
		inAnimation = false;
		return BehaveResult.Running;
	}

    public BehaveResult InitLeashBackToSpawnAction(Tree sender) {
        inAnimation = false;
        return BehaveResult.Running;
    }

	
	public BehaveResult TickLookAroundAction(Tree sender)
	{
		gameObject.SendMessage("StopMoving","Look Around");
		if(!doneLooking) //Haven't found a target and aren't done durdling
		{
			lookTimer += TickTime;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
			if(!inAnimation) //If we aren't already animating for this behavior, start
			{/*
				System.Random rnd = new System.Random();
				int randomTwitch = rnd.Next(0, 3);
				if(randomTwitch==1)
				{
					StartCoroutine(COTwitch());
				}
				else if(randomTwitch ==2)
				{
					StartCoroutine(COBark());
				}*/
			}
			if(lookTimer > lookMaxTime)
			{
				doneLooking = true;
			}
			
			return BehaveResult.Running;
		}
		else
		{
			lookTimer = 0;
			doneLooking = false;
			inAnimation = false;
			return BehaveResult.Success;
		}
		
	}
	
	public BehaveResult TickWanderAction(Tree sender)
	{
		if(!doneWandering)
		{
			wanderTimer += TickTime;
			
			if(!inAnimation) //If we aren't already animating for this behavior, start
			{
				System.Random rnd = new System.Random();
				int randomPatrolPoint = rnd.Next(0, PatrolPoints.Length);

                //going to the same place? twitch instead
                if (Vector3.Distance(PatrolPoints[randomPatrolPoint], gameObject.transform.position) < 3) {
				    int randomTwitch = rnd.Next(0, 3);
				    if(randomTwitch==1)
				    {
				    	StartCoroutine("COTwitch");
				    }
				    else if(randomTwitch ==2)
				    {
				    	StartCoroutine("COBark");
				    }
                }

				gameObject.SendMessage("StartMovingTo", PatrolPoints[randomPatrolPoint]);
				StartCoroutine("COWander");
			}
			
			if(wanderTimer > wanderMaxTime)
			{
				gameObject.SendMessage("StopMoving","TickWanderActionTimeOut");
				doneWandering = true;
			}
			return BehaveResult.Running;
		}
		else
		{
			gameObject.SendMessage("StopMoving","TickWanderActionBottom");
			wanderTimer = 0;
			doneWandering = false;
			inAnimation = false;
			return BehaveResult.Success;
		}
	}
	
	public BehaveResult TickAttackAction(Tree sender)
	{
		transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
		if(!inAnimation) //If we aren't already animating for this behavior, start
		{
            StartCoroutine((Random.Range(0,2) < 1.0) ? "COAttack1" : "COAttack2");
            //StartCoroutine("COAttack1");
		}
		return BehaveResult.Running;
	}
	
	public BehaveResult TickCloseDistanceAndAlignAction(Tree sender)
	{
        //Debug.Log("CDAA");
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        gameObject.SendMessage("StartMovingTo", player.transform.position +  3 * player.transform.forward);
		if(!inAnimation) //If we aren't already animating for this behavior, start
		{
			StartCoroutine("COChase");
		}
		
		return BehaveResult.Running;
	}

    public BehaveResult TickLeashBackToSpawnAction(Tree sender) {
  
        //Debug.Log("current pos: " + this.gameObject.transform.position + " spawn point: " + spawnPoint);
        gameObject.SendMessage("StartMovingTo", spawnPoint);
        if (!inAnimation) //If we aren't already animating for this behavior, start
		{
            StartCoroutine("COChase");
        }

        return BehaveResult.Running;
    }
	
	
	IEnumerator CORoar() {
        Debug.Log("CORoar");
		inAnimation = true;
		gameObject.animation.CrossFade("Idle");
	    yield return new WaitForSeconds(.5f);
	    gameObject.animation.CrossFade("Roar");
		yield return new WaitForSeconds(1f);
		gameObject.animation.CrossFade("Idle");
		inAnimation = false;
	}
	IEnumerator COTwitch() {
        Debug.Log("COTwitch");
		inAnimation = true;
		gameObject.animation.CrossFade("Idle");
	    yield return new WaitForSeconds(.5f);
	    gameObject.animation.CrossFade("Look_Around");
	    yield return new WaitForSeconds(2f);
		gameObject.animation.CrossFade("Idle");
		inAnimation = false;
	}
	
	IEnumerator COWander() {
        Debug.Log("COWander");
		inAnimation = true;
		gameObject.SendMessage("SetSpeed", 1f);
	    gameObject.animation.CrossFade("Walk_Loop");
        yield return new WaitForSeconds(gameObject.animation.GetClip("Walk_Loop").length / animationMultiplier);
	}
	IEnumerator COChase() {
        Debug.Log("COChase");
		inAnimation = true;
		gameObject.SendMessage("SetSpeed", 7f);
	    gameObject.animation.CrossFade("monster_ground_run");
        yield return new WaitForSeconds(gameObject.animation.GetClip("monster_ground_run").length / animationMultiplier);
	}

	IEnumerator COAttack1() {
        Debug.Log("COAttack1");
		inAnimation = true;
		swipe.collider.enabled = true;
	    gameObject.animation.CrossFade("Attack_1");
        yield return new WaitForSeconds(gameObject.animation.GetClip("Attack_1").length / animationMultiplier);
        swipe.collider.enabled = false;
		inAnimation = false;
	}

    IEnumerator COAttack2() {
        Debug.Log("COAttack2");
        inAnimation = true;
        bite.collider.enabled = true;
        gameObject.animation.Play("Attack_2");
        yield return new WaitForSeconds(gameObject.animation.GetClip("Attack_2").length / animationMultiplier);
        bite.collider.enabled = false;
        inAnimation = false;
    }



	IEnumerator COHitRecoil() {
        Debug.Log("COHitRecoil");
		inAnimation = true;
		//gameObject.animation.CrossFade("Hit_Recoil");
        gameObject.animation.Play("Hit_Recoil");
        yield return new WaitForSeconds(gameObject.animation.GetClip("Hit_Recoil").length / animationMultiplier);
		inAnimation = false;
	}

    IEnumerator CODie() {
        Debug.Log("CODie");
        inAnimation = true;
        Debug.Log("beginning death animation");
        gameObject.animation.Play("Death_1");
        //Debug.Log("yielding");
        Debug.Log(gameObject.animation.GetClip("Death_1").length);
        yield return new WaitForSeconds(gameObject.animation.GetClip("Death_1").length / animationMultiplier);
    }
    
    
    void StartDeathAnim() {
        //Debug.Log("beginning die function");
        if (this.transform.parent != null) {
            this.transform.parent.GetComponent<EnemyNodeManager>().startRespawnTimer();
        }
        dead = true;
        m_tree = null;  //stop behavior tree from running
        StopCoroutine("COAttack1");
        StopCoroutine("COChase");
        StartCoroutine(CODie()); //run death animation

        this.gameObject.AddComponent<DeadLizardManager>(); //add item manager to lizard carcass

        //disable colliders which are no longer needed
        this.gameObject.GetComponentInChildren<SphereCollider>().enabled = false;
        this.gameObject.GetComponent<CharacterController>().enabled = false;
        //resize box collider to be used for gathering interaction
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(.5f, .6f, .06f);
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(2.0f, 1.0f, 2.8f);

    }
	

    void OnTriggerEnter(Collider col) {
        if (!dead && col.gameObject.tag == "PlayerWeaponBone") {
            //Debug.Log("I've been hit!");
            StartCoroutine("COHitRecoil");
            takingHitTimer = takingHitTimerMax;
            col.gameObject.transform.parent.parent.SendMessage("Hit");
            this.gameObject.SendMessage("TakeDamage", (col.gameObject.transform.parent.parent.GetComponent("DamageDealer") as DamageDealer).damage);
            recentlyAttackedTimer = 7.0f;

            senseSomething = true;
            seeTarget = true;
        }
    }

	//Poorly named, actually used for chasing also
	void WanderingDone(int retValue)
	{
		switch(retValue)
		{
		case -1:
			//we got stuck or something weird happened
			doneWandering = true;
            //Debug.Log("shit im stuck");
			break;
		case 1:
			//we got to or close enough to our wander target
			doneWandering = true;
			break;
		default:
			break;
		}
	}
	
	private bool EvaluateExpression( string expr ) {
	    string[] exprs;
	    exprs = expr.Split( new string[] { "&&"} , System.StringSplitOptions.None );
	    if( exprs.Length > 1 ) {
	        foreach( string e in exprs ) {
	            if( !EvaluateExpression( e.Trim() ) ) {
	                return false;
	            }
	        }
	        return true;
	    } else {
	        exprs = expr.Split( new string[] { "||" }, System.StringSplitOptions.None );
	        if( exprs.Length > 1 ) {
	            foreach( string e in exprs ) {
	                if( EvaluateExpression( e.Trim() ) ) {
	                    return true;
	                }
	            }
	            return false;
	        } else {
	            bool not = false;
	            if( expr.StartsWith( "!" ) ) {
	                not = true;
	                expr = expr.Replace( "!" , "" );
	            }
	            bool b = (bool)this.GetType().GetField( expr ).GetValue( this );
	            if( not ) {
	                return !b;
	            } else {
	                return b;
	            }
	        }
	    }
	}

    public void setDaytime() {
        for (int i = 0; i < PatrolPoints.Length; i++) {
            PatrolPoints[i] /= 2;
        }

        this.gameObject.SendMessage("setDamageMultiplier", 1.0f);
        this.sightDistance = 30f;
        this.leashDistance = 40f;
    }

    public void setNightime() {
        for (int i = 0; i < PatrolPoints.Length; i++) {
            PatrolPoints[i] *= 2;
        }

        this.gameObject.SendMessage("setDamageMultiplier", 2.0f);
        this.sightDistance = 45f;
        this.leashDistance = 50f;
    }
}
