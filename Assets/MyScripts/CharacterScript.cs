using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {
	
	private int timesHit = 0;
	private float ammo = 0.0f;
	public  float leftChargeTime = 0.0f;
	public  float rightChargeTime = 0.0f;
	
	private bool      charging;
	private float     prevChargePercentLeft = 0.0f;
	private float     prevChargePercentRight = 0.0f;
	private float 	  maxChargeTime = 4.75f;
	public GameObject leftChargingFire;
	public GameObject leftChargedFire;
	public GameObject rightChargingFire;
	public GameObject rightChargedFire;
	
	public GameObject bulletPrefab;
	public GameObject wallPrefab;
	public CoreLogic  CoreLogicScript;
	public GameObject statusTextBar;
	public GameObject hitTextBar;
	public GameObject chargeTextBar;
	public GameObject blockTextBar;
	public GameObject floor;
	public GameObject invisibleWall;
	
	//private TextRenderScript script;
	private AudioSource[] sources;
	
	void Start () 
	{
		if(CoreLogicScript.difficulty == 0)
			ammo += 200.0f;
		else if(CoreLogicScript.difficulty == 1)
			ammo += 20.0f;
	
		//script = statusTextBar.GetComponent<TextRenderScript>();
		sources = gameObject.GetComponents<AudioSource>();
		sources[0].pitch = 1.2f;
	}
	
	void Update () 
	{
		UpdateUI();
	}
	
	void OnTriggerEnter(Collider other)
	{
  		if(other.gameObject != floor && other.gameObject != invisibleWall)
  		{
  			++timesHit;

  			if(CoreLogicScript.difficulty > 1)
  			{
  				leftChargeTime = 0.0f;
  				rightChargeTime = 0.0f;
  				if(CoreLogicScript.difficulty > 2)
  					--ammo;
  			}
  			sources[0].Stop();
  			sources[0].Play();  			
  			Destroy(other.gameObject);
  		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == invisibleWall)
		{
			//script.allowTextToDisappear = true;
  			//script.DisplayText("Out of Bounds");
		}
	}
	
	public void BlockReceived()
	{
		ammo++;
		sources[1].Stop();
		sources[1].Play();
	}
	
	public void StartedCharging()
	{
		charging = true;
		ChangeVisibleFires();
	}
	
	public void StoppedCharging()
	{
		charging = false;
		prevChargePercentLeft = -1.0f;
		prevChargePercentRight = -1.0f;
		ChangeVisibleFires();
	}
	
	private void ChangeVisibleFires()
	{
		leftChargingFire.SetActive(charging);
		leftChargedFire.SetActive(!charging);
		rightChargingFire.SetActive(charging);
		rightChargedFire.SetActive(!charging);
	}
	
	private void adjustFires()
	{
		
		float percentOfMaxLeft = Mathf.Min(ammo, leftChargeTime/maxChargeTime);
		if(percentOfMaxLeft > 1.0f)
				percentOfMaxLeft = 1.0f;
		
		if(prevChargePercentLeft != percentOfMaxLeft)
		{
			prevChargePercentLeft = percentOfMaxLeft;
			if(charging)
			{
				GameObject outerCore = leftChargingFire.transform.Find("FireballOuterCore").gameObject;
				ParticleEmitter emitter = outerCore.GetComponent<ParticleEmitter>();
				emitter.maxEmission = 20 + percentOfMaxLeft * 180;
				emitter.angularVelocity = 55 + percentOfMaxLeft * 105;
			}
			else
			{
				GameObject innerCore = leftChargedFire.transform.Find("InnerCore").gameObject;
				ParticleEmitter emitter = innerCore.GetComponent<ParticleEmitter>();
				emitter.minEmission = percentOfMaxLeft * 100;
				emitter.maxEmission = percentOfMaxLeft * 100;
			}
		}
		
		float percentOfMaxRight = Mathf.Min(ammo, rightChargeTime/maxChargeTime);
		if(percentOfMaxRight > 1.0f)
				percentOfMaxRight = 1.0f;
		
		if(prevChargePercentRight != percentOfMaxRight)
		{
			prevChargePercentRight = percentOfMaxRight;
			
			if(charging)
			{
				GameObject outerCore = rightChargingFire.transform.Find("FireballOuterCore").gameObject;
				ParticleEmitter emitter = outerCore.GetComponent<ParticleEmitter>();
				emitter.maxEmission = 20 + percentOfMaxRight * 180;
				emitter.angularVelocity = 55 + percentOfMaxRight * 105;
			}
			else
			{
				GameObject innerCore = rightChargedFire.transform.Find("InnerCore").gameObject;
				ParticleEmitter emitter = innerCore.GetComponent<ParticleEmitter>();
				emitter.minEmission = percentOfMaxRight * 100;
				emitter.maxEmission = percentOfMaxRight * 100;
			}
		}
	}
	
	public void PunchReceived(Transform fistLocation, bool left)
	{
		float chargeTime = left ? leftChargeTime : rightChargeTime;
		if(ammo > chargeTime)
		{
			ammo -= chargeTime;
			LaunchBullet(CoreLogicScript.bulletVelocity, CoreLogicScript.bulletLifetime, fistLocation, left);
		}
		else
		{
			leftChargeTime = 0.0f;
			rightChargeTime = 0.0f;
		}
	}
	
	public void StompReceived(Transform ankleLocation)
	{
		Vector3 wallPosition;
		Vector3 wallRotation;
		if(ammo >= 5)
		{
			ammo -= 5;
	//	if(angle < 22.5)
	//	{
			wallPosition = new Vector3(4.5f, 1.341022f, 0.0f);
			wallRotation = new Vector3(0.0f, 90.0f, 0.0f);
			Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
	//	}
	//	else if(angle >= 22.5 && angle < 67.5)
	//	{
			wallPosition = new Vector3(3.5f, 1.341022f, -3.5f);
			wallRotation = new Vector3(0.0f, -45.0f, 0.0f);
			Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
	//	}
	//	else if(angle >= 67.5 && angle < 112.5)
	//	{
			wallPosition = new Vector3(0.0f, 1.341022f, -4.5f);
			wallRotation = new Vector3(0.0f, 0.0f, 0.0f);
			Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
	//	}
	//	else if(angle >= 112.5 && angle < 157.5)
	//	{
			wallPosition = new Vector3(-3.5f, 1.341022f, -3.5f);
			wallRotation = new Vector3(0.0f, 45.0f, 0.0f);
			Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
	//	}
	//	else
	//	{
			wallPosition = new Vector3(-4.5f, 1.341022f, 0.0f);
			wallRotation = new Vector3(0.0f, 90.0f, 0.0f);
			Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
	//	}
	}
		
	}
	
	public void UpdateUI(float a, float b)
	{
		hitTextBar.GetComponent<TextMesh>().text = "A: " + a;
  		blockTextBar.GetComponent<TextMesh>().text = "B: " + b;
	}
	
	public void LaunchBullet(float bulletVelocity, float bulletLifetime, Transform fistLocation, bool left)
	{
		Vector3 trueForward = fistLocation.right;
		if(left)
			trueForward *= -1;
		
		float chargeTime = left ? leftChargeTime : rightChargeTime;
		if(chargeTime > maxChargeTime)
			chargeTime = maxChargeTime;	
		
		Vector3 initLocation = fistLocation.position + trueForward.normalized * 0.4f; //* (chargeTime + 0.25f)/2.0f;
		
		GameObject fireball = (GameObject)Instantiate(bulletPrefab, initLocation, Quaternion.identity);
		FireballProperties fireballScript = fireball.GetComponent<FireballProperties>();
		fireballScript.ScaleToChargeTime(chargeTime);
		
		if(left)
			leftChargeTime = 0.0f;
		else
			rightChargeTime = 0.0f;
	
		fireball.rigidbody.AddForce(trueForward.normalized * 800);

		Destroy(fireball, bulletLifetime);
	}
	
	public void UpdateUI()
	{
		adjustFires();
	//	hitTextBar.GetComponent<TextMesh>().text = "Hits: " + timesHit;
  	//	blockTextBar.GetComponent<TextMesh>().text = "Energy: " + ammo.ToString("0.00");
  	//	chargeTextBar.GetComponent<TextMesh>().text = "L: " + Mathf.Min(leftChargeTime, maxChargeTime).ToString("0.00") + " R: " + Mathf.Min(rightChargeTime, maxChargeTime).ToString("0.00");
	}
}
