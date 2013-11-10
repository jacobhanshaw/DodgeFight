using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {
	
	private int timesHit = 0;
	private float ammo = 200.0f;
	public float leftChargeTime = 0.0f;
	public float rightChargeTime = 0.0f;
	
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
  			//--ammo;
  			leftChargeTime = 0.0f;
  			rightChargeTime = 0.0f;
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
	
	public void PunchReceived(Transform fistLocation, bool left)
	{
		if(ammo > 0)
		{
			--ammo;
			LaunchBullet(CoreLogicScript.bulletVelocity, CoreLogicScript.bulletLifetime, fistLocation, left);
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
			GameObject wall0 = (GameObject)Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
	//	}
	//	else if(angle >= 22.5 && angle < 67.5)
	//	{
			wallPosition = new Vector3(3.5f, 1.341022f, -3.5f);
			wallRotation = new Vector3(0.0f, -45.0f, 0.0f);
			GameObject wall1 = (GameObject)Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
	//	}
	//	else if(angle >= 67.5 && angle < 112.5)
	//	{
			wallPosition = new Vector3(0.0f, 1.341022f, -4.5f);
			wallRotation = new Vector3(0.0f, 0.0f, 0.0f);
			GameObject wall2 = (GameObject)Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
	//	}
	//	else if(angle >= 112.5 && angle < 157.5)
	//	{
			wallPosition = new Vector3(-3.5f, 1.341022f, -3.5f);
			wallRotation = new Vector3(0.0f, 45.0f, 0.0f);
			GameObject wall3 = (GameObject)Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
	//	}
	//	else
	//	{
			wallPosition = new Vector3(-4.5f, 1.341022f, 0.0f);
			wallRotation = new Vector3(0.0f, 90.0f, 0.0f);
			GameObject wall4 = (GameObject)Instantiate(wallPrefab, wallPosition, Quaternion.Euler(wallRotation.x, wallRotation.y, wallRotation.z));
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
		
		Vector3 initLocation = fistLocation.position + trueForward.normalized;
		initLocation += trueForward.normalized * chargeTime;
		
		GameObject fireball = (GameObject)Instantiate(bulletPrefab, initLocation, Quaternion.identity);
		FireballProperties fireballScript = fireball.GetComponent<FireballProperties>();
		fireballScript.ScaleToChargeTime(chargeTime);
		ammo -= chargeTime;
		
		if(left)
			leftChargeTime = 0.0f;
		else
			rightChargeTime = 0.0f;
	
		fireball.rigidbody.AddForce(trueForward.normalized * 800);

		Destroy(fireball, bulletLifetime);
	}
	
	public void UpdateUI()
	{
		hitTextBar.GetComponent<TextMesh>().text = "Hits: " + timesHit;
  		blockTextBar.GetComponent<TextMesh>().text = "Energy: " + ammo.ToString("0.00");
  		chargeTextBar.GetComponent<TextMesh>().text = "L: " + leftChargeTime.ToString("0.00") + "R: " + rightChargeTime.ToString("0.00");
	}
}
