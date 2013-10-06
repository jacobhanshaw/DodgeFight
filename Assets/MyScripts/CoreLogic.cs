using UnityEngine;
using System.Collections;

public class CoreLogic : MonoBehaviour {
	
	public int   level = 2;
	public bool  staggered = true;
	public float timeBetweenLaunches = 3.0f;
	public float timeSinceLastLaunch = 0.0f;
	public float bulletVelocity = 800.0f;
	public float bulletLifetime = 16.0f; 
	
	public float initialDelayTime = 5.0f;
	private bool waitedInitialDelay = false;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{	
		timeSinceLastLaunch += Time.deltaTime;
		
		if(waitedInitialDelay)
			{
			if(timeSinceLastLaunch > timeBetweenLaunches || (staggered && timeSinceLastLaunch > timeBetweenLaunches/(float)level))
			{
				timeSinceLastLaunch = 0.0f;
				ArrayList launchers = new ArrayList(GameObject.FindGameObjectsWithTag ("Launcher")); //.GetComponent(GUIButtons).test
		
				for(int i = 0; i < (staggered ? 1 : level); ++i)
				{
					int launcherIndex = Random.Range(0, launchers.Count);
					BulletLauncherBehavior launchScript = ((GameObject)launchers[launcherIndex]).GetComponent<BulletLauncherBehavior>();
					launchScript.LaunchBullet(bulletVelocity, bulletLifetime);
					launchers.Remove(launcherIndex);
				}
			}
			}
			else
			{
				if(timeSinceLastLaunch > initialDelayTime)
				{
					timeSinceLastLaunch = 0.0f;
					waitedInitialDelay = true;
				}
			}
	}
}
