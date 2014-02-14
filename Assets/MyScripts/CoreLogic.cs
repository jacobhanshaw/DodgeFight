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
		private float bulletVelocityRange = 0.0f;
		public float bulletVelocityRangeStep = 20.0f;
		public float bulletLifetime = 12.0f; 
	
		public float initialDelayTime = 5.0f;
	
		public GameObject scriptContainer;
		private TextRenderScript textScript;
		public GameObject launchersPrefab;
	
		private bool waitedInitialDelay = false;
		private bool instantiatedLaunchers = true;
		
		void Start ()
		{
				//		textScript = scriptContainer.GetComponent<TextRenderScript> ();
		}
	
		void Update ()
		{	
				timeSinceLastLaunch += Time.deltaTime;
		
				if (waitedInitialDelay) {
				
						if (!instantiatedLaunchers) {
								instantiatedLaunchers = true;
								Instantiate (launchersPrefab, Vector3.zero, Quaternion.identity);
						}
				
						ArrayList launchers = new ArrayList (GameObject.FindGameObjectsWithTag ("Launcher"));
			
						if (launchers.Count == 0) {
								++level;
								staggered = level % 2 == 0;
								bulletVelocityRange += bulletVelocityRangeStep;
								//			textScript.allowTextToDisappear = true;
								//			textScript.DisplayText ("Welcome to Level: " + (level - 1));
								
								timeSinceLastLaunch = 0.0f;
								waitedInitialDelay = false;
								instantiatedLaunchers = false;
						} else {
								int numToLaunch = Mathf.Min (level, launchers.Count);
			
								if (timeSinceLastLaunch > timeBetweenLaunches || (staggered && timeSinceLastLaunch > timeBetweenLaunches / (float)numToLaunch)) {
										timeSinceLastLaunch = 0.0f;
		
										for (int i = 0; i < (staggered ? 1 : numToLaunch); ++i) {
												if (launchers.Count == 0)
														return;
												int launcherIndex = Random.Range (0, launchers.Count);
												BulletLauncherBehavior launchScript = ((GameObject)launchers [launcherIndex]).GetComponent<BulletLauncherBehavior> ();
												float randBulletVelocity = Random.Range (bulletVelocity - bulletVelocityRange, bulletVelocity + bulletVelocityRange);
												if (!launchScript.destroyed)
														launchScript.LaunchBullet (randBulletVelocity, bulletLifetime);
												else
														--i;
												launchers.Remove (launcherIndex);
										}
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
