using UnityEngine;
using System.Collections;

public class BulletProperties : MonoBehaviour {
	
	public float invincibleTime = 0.25f;
	public int creatorId;
	
	private float timeSinceCreate = 0.0f;
	
	void Start () 
	{
		AudioSource source = gameObject.GetComponent<AudioSource>();
		source.pitch = 0.25f;
	}
	
	void Update () 
	{
		timeSinceCreate += Time.deltaTime;
		if(timeSinceCreate > invincibleTime)
			creatorId = 0;
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		Destroy(gameObject);
	}
	
	void OnTriggerEnter(Collider other)
	{	
		if(other.GetInstanceID() != creatorId)
			Destroy(gameObject);
	}
	
}
