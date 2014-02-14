using UnityEngine;
using System.Collections;

public class BulletLauncherBehavior : MonoBehaviour
{	
		public  bool  controlled = true;

		public  float bulletVelocityLocal = 250.0f;
		public  float bulletLifetimeLocal = 2.0f;
		public  float timeBetweenLaunches = 5.0f;
		private float timeSinceLastLaunch = 0.0f;
	
		[HideInInspector]
		public bool
				destroyed = false;
		private float burnTime = 1.5f;
		private float boxTime = 1.0f;
	
		public  GameObject bulletPrefab;
		public  GameObject destructionBurnPrefab;
	
		void Update ()
		{
				if (!controlled) {
						timeSinceLastLaunch += Time.deltaTime;
			
						if (timeSinceLastLaunch > timeBetweenLaunches) {
								timeSinceLastLaunch = 0.0f;
				
								LaunchBullet (bulletVelocityLocal, bulletLifetimeLocal);
						}
				}
		}
	
		void OnTriggerEnter (Collider other)
		{
				Debug.Log ("Trigger Enter Other: " + other.name);
		
				if (other.gameObject.tag.Equals ("Bullet")) {
						BulletProperties bulletScript = other.gameObject.GetComponent<BulletProperties> ();
						if (bulletScript) {
								if (bulletScript.creatorId == gameObject.GetInstanceID ())
										return;
						}
				} 
		
				destroy ();
		}
	
		void OnCollisionEnter (Collision collision)
		{
				Debug.Log ("Collision Enter Other: " + collision.gameObject.name);
		
				if (collision.gameObject.tag.Equals ("Bullet")) {
						BulletProperties bulletScript = collision.gameObject.GetComponent<BulletProperties> ();
						if (bulletScript) {
								if (bulletScript.creatorId == gameObject.GetInstanceID ())
										return;
						}
				} 
	
				destroy ();
		}
	
		void destroy ()
		{
				GameObject fire = (GameObject)Instantiate (destructionBurnPrefab, gameObject.transform.position, Quaternion.identity);
				destroyed = true;
				Destroy (gameObject, boxTime);
				Destroy (fire, burnTime);
		}
	
		public void LaunchBullet (float bulletVelocity, float bulletLifetime)
		{
				GameObject bullet = (GameObject)Instantiate (bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
				BulletProperties script = bullet.GetComponent<BulletProperties> ();
				script.creatorId = gameObject.GetInstanceID ();
				bullet.rigidbody.AddForce (gameObject.transform.forward * bulletVelocity);
				Destroy (bullet, bulletLifetime);
		}
}
