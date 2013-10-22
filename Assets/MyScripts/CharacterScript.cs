using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {
	
	private int timesHit = 0;
	private int ammo = 0;
	public GameObject bulletPrefab;
	public CoreLogic  CoreLogicScript;
	public GameObject statusTextBar;
	public GameObject hitTextBar;
	public GameObject blockTextBar;
	public GameObject floor;
	public GameObject invisibleWall;
	
	//private TextRenderScript script;
	private AudioSource[] sources;
	
	// Use this for initialization
	void Start () 
	{
		//script = statusTextBar.GetComponent<TextRenderScript>();
		sources = gameObject.GetComponents<AudioSource>();
		sources[0].pitch = 1.2f;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void OnTriggerEnter(Collider other)
	{
  		if(other.gameObject != floor && other.gameObject != invisibleWall)
  		{
  			timesHit++;
  			sources[0].Stop();
  			sources[0].Play();
  			UpdateUI();
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
		UpdateUI();
	}
	
	public void PunchReceived(Transform fistLocation, bool left)
	{
		if(ammo > 0)
		{
			--ammo;
			UpdateUI();
			
			LaunchBullet(CoreLogicScript.bulletVelocity, CoreLogicScript.bulletLifetime, fistLocation, left);
		}
	}
	
	public void UpdateUI(float a, float b)
	{
		hitTextBar.GetComponent<TextMesh>().text = "Left Arm Angle: " + ((int)a);
  		blockTextBar.GetComponent<TextMesh>().text = "Right Arm Angle " + ((int)b);
	}
	
	public void LaunchBullet(float bulletVelocity, float bulletLifetime, Transform fistLocation, bool left)
	{
		Vector3 trueForward = fistLocation.right;
		if(left)
			trueForward *= -1;

		GameObject bullet = (GameObject)Instantiate(bulletPrefab, fistLocation.position + trueForward.normalized, Quaternion.identity);//fistLocation.rotation);
		BulletProperties bulletScript = bullet.GetComponent<BulletProperties>();
		bulletScript.creatorId = -5;
		//TODO: Possibly Make Use of Creator Id
		bullet.rigidbody.AddForce(trueForward.normalized * 800);

		Destroy(bullet, bulletLifetime);
	}
	
	private void UpdateUI()
	{
		//hitTextBar.GetComponent<TextMesh>().text = "Hits: " + timesHit;
  		//blockTextBar.GetComponent<TextMesh>().text = "Ammo: " + ammo;
	}
}
