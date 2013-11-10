using UnityEngine;
using System.Collections;

public class FireballProperties : MonoBehaviour
{
	private Component[] emitters;
	private float[] originalMinSize;
	private float[] originalMaxSize;
	private float[] originalMinEmission;
	private float[] originalMaxEmission;
	private float originalScale;
	private Vector3 newScale;
	private float chargeTime;
	private float collisionScaleAmount = 0.5f;
	private int   collisionFireballSpawnAmount = 5;
	
	void Start ()
	{
		originalScale = gameObject.transform.localScale.x;
		newScale = gameObject.transform.localScale;
		emitters = gameObject.GetComponentsInChildren<ParticleEmitter> ();
		originalMinSize = new float[emitters.Length];
		originalMaxSize = new float[emitters.Length];
		originalMinEmission = new float[emitters.Length];
		originalMaxEmission = new float[emitters.Length];
		
		for (int i = 0; i < emitters.Length; ++i) 
		{
			ParticleEmitter emitter = (ParticleEmitter)emitters [i];
			originalMinSize [i] = emitter.minSize;
			originalMaxSize [i] = emitter.maxSize;
			originalMinEmission [i] = emitter.minEmission;
			originalMaxEmission [i] = emitter.maxEmission;
		}
	}
	
	void Update ()
	{
		//gameObject.rigidbody.AddForce(-50 * gameObject.transform.forward.normalized);
		
		if (gameObject.transform.localScale.x != newScale.x)
			UpdateScale ();
	}
	
	void OnCollisionEnter (Collision collision)
	{
		ReactToCollision ();
	}
	
	void OnTriggerEnter (Collider other)
	{
		ReactToCollision ();
	}
	
	void ReactToCollision ()
	{
		AnimateCollision ();
		if (gameObject.transform.localScale.x > collisionScaleAmount)
			ScaleToChargeTime (gameObject.transform.localScale.x - collisionScaleAmount - originalScale);
		else
			Destroy (gameObject);
			
	}
	
	void AnimateCollision ()
	{
		float maxRangePosition = transform.localScale.x;
		for (int i = 0; i < collisionFireballSpawnAmount; ++i) 
		{
			Vector3 position = new Vector3 (Random.Range (-maxRangePosition, maxRangePosition), Random.Range (-maxRangePosition, maxRangePosition), Random.Range (-maxRangePosition, maxRangePosition));
			float scaleNumber = Random.Range (0.10F, transform.localScale.x / 4.0f);
			Vector3 scale = new Vector3 (scaleNumber, scaleNumber, scaleNumber);
			Vector3 force = position * 1200;
			GameObject fireball = (GameObject)Instantiate (gameObject, gameObject.transform.position + position, Quaternion.identity);
			fireball.transform.localScale = scale;
			FireballProperties fireballScript = fireball.GetComponent<FireballProperties>();
			Destroy(fireball.collider);
			fireball.rigidbody.useGravity = true;
			fireball.rigidbody.AddForce (force);

			Destroy (fireball, 1.0f);
		}
	}
	
	public void ScaleToChargeTime (float aChargeTime)
	{
		chargeTime = aChargeTime;
		float currentScale = originalScale + chargeTime;
		newScale = new Vector3 (currentScale, currentScale, currentScale);
	}
	
	private void UpdateScale ()
	{
		gameObject.transform.localScale = Vector3.Lerp (gameObject.transform.localScale, newScale, Time.deltaTime);
		
		for (int i = 0; i < emitters.Length; ++i) {
			ParticleEmitter emitter = (ParticleEmitter)emitters [i];
			emitter.minSize = Mathf.Lerp (emitter.minSize, originalMinSize [i] + chargeTime, Time.deltaTime);
			emitter.maxSize = Mathf.Lerp (emitter.maxSize, originalMaxSize [i] + chargeTime, Time.deltaTime);
			emitter.minEmission = Mathf.Lerp (emitter.minEmission, originalMinEmission [i] + (chargeTime * 100), Time.deltaTime);
			emitter.maxEmission = Mathf.Lerp (emitter.maxEmission, originalMaxEmission [i] + (chargeTime * 100), Time.deltaTime);
		}
	}
}
