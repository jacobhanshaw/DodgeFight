using UnityEngine;
using System.Collections;

public class BulletLauncherBehavior : MonoBehaviour {
	
	public  static int topId = 1;
	private int localId;
	
	public  bool  controlled = true;
	public  float bulletVelocityLocal = 250.0f;
	public  float bulletLifetimeLocal = 2.0f;
	public  float timeBetweenLaunches = 5.0f;
	
	private float timeSinceLastLaunch = 0.0f;
	
	public  GameObject bulletPrefab;
	// Use this for initialization
	
	void Awake ()
	{
		localId = topId++;
	}
	
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!controlled)
		{
			timeSinceLastLaunch += Time.deltaTime;
			
			if(timeSinceLastLaunch > timeBetweenLaunches)
			{
				timeSinceLastLaunch = 0.0f;
				
				LaunchBullet(bulletVelocityLocal, bulletLifetimeLocal);
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag.Equals("Bullet"))
		{
			BulletProperties bulletScript = other.gameObject.GetComponent<BulletProperties>();
			if(bulletScript.creatorId == localId)
				return;
		} 
		
  		Destroy(gameObject); 
	}
	
	public void LaunchBullet(float bulletVelocity, float bulletLifetime)
	{
		GameObject bullet = (GameObject)Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
		BulletProperties script = bullet.GetComponent<BulletProperties>();
		script.creatorId = localId;
		bullet.rigidbody.AddForce(gameObject.transform.forward * bulletVelocity);
		Destroy(bullet, bulletLifetime);
	}
}
