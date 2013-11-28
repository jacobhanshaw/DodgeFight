using UnityEngine;
using System.Collections;

public class CoreLogic : MonoBehaviour
{
	
	public int   difficulty = 0;
	public int   level = 2;
	public bool  staggered = true;
	public float timeBetweenLaunches = 3.0f;
	public float timeSinceLastLaunch = 0.0f;
	public float bulletVelocity = 800.0f;
	public float bulletLifetime = 12.0f; 
	
	public float initialDelayTime = 5.0f;
	
	public TextRenderScript script;
	public GameObject launchersPrefab;
	
	private bool waitedInitialDelay = false;
	
	
	// Use this for initialization
//	void Start () 	{}
	
	// Update is called once per frame
	void Update ()
	{	
		timeSinceLastLaunch += Time.deltaTime;
		
		if (waitedInitialDelay) {
			ArrayList launchers = new ArrayList (GameObject.FindGameObjectsWithTag ("Launcher"));
			
			if (launchers.Count == 0) {
				++level;
				staggered = level % 2 == 0;
				Instantiate (launchersPrefab, Vector3.zero, Quaternion.identity);
				script.allowTextToDisappear = true;
				script.DisplayText ("Welcome to Level: " + (level - 1));
			}
			
			int numToLaunch = Mathf.Min (level, launchers.Count);
			
			if (timeSinceLastLaunch > timeBetweenLaunches || (staggered && timeSinceLastLaunch > timeBetweenLaunches / (float)numToLaunch)) {
				timeSinceLastLaunch = 0.0f;
		
				for (int i = 0; i < (staggered ? 1 : numToLaunch); ++i) {
					if (launchers.Count == 0)
						break;
					int launcherIndex = Random.Range (0, launchers.Count);
					BulletLauncherBehavior launchScript = ((GameObject)launchers [launcherIndex]).GetComponent<BulletLauncherBehavior> ();
					if (!launchScript.destroyed)
						launchScript.LaunchBullet (bulletVelocity, bulletLifetime);
					else
						--i;
					launchers.Remove (launcherIndex);
				}
			}
		} else {
			if (timeSinceLastLaunch > initialDelayTime) {
				timeSinceLastLaunch = 0.0f;
				waitedInitialDelay = true;
			}
		}
	}
}
