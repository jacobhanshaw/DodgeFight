using UnityEngine;
using System.Collections;

public class GestureRecognizer : MonoBehaviour 
{
	//public ZigSkeleton zigScript;
	public CharacterScript script;
	
	int punchStateLeft = 0;
	float punchLeftStartTime = 0;
	int punchStateRight = 0;
	float punchRightStartTime = 0;
	
	float punchStartAngle = 40.0f;
	float punchEndAngle = 170.0f;
	float maxPunchTime = 2.5f;
	
	public Transform Head;
    public Transform Neck;
    public Transform Torso;
    public Transform Waist;

    // public Transform LeftCollar;
    public Transform LeftShoulder;
    public Transform LeftElbow;
    public Transform LeftWrist;
    // public Transform LeftHand;
    // public Transform LeftFingertip;

    // public Transform RightCollar;
    public Transform RightShoulder;
    public Transform RightElbow;
    public Transform RightWrist;
    // public Transform RightHand;
    // public Transform RightFingertip;

    public Transform LeftHip;
    public Transform LeftKnee;
    public Transform LeftAnkle;
    // public Transform LeftFoot;

    public Transform RightHip;
    public Transform RightKnee;
    public Transform RightAnkle;
    // public Transform RightFoot;
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckForPunch();
	}
	
	void CheckForPunch ()
	{
		//Debug.Log("Left State: " + punchStateLeft);
		//Debug.Log("Right State: " + punchStateRight);
		
		float leftWristToShoulderAngle = AngleBetweenTransformsGivenPivot(LeftShoulder, LeftWrist, LeftElbow);
		float rightWristToShoulderAngle = AngleBetweenTransformsGivenPivot(RightShoulder, RightWrist, RightElbow);;
		
		script.UpdateUI(leftWristToShoulderAngle, rightWristToShoulderAngle);
		
		switch(punchStateLeft)
		{
			case(0):
				if(leftWristToShoulderAngle <= punchStartAngle)
				{
					punchLeftStartTime = Time.time;
					++punchStateLeft;
				}
			break;
			case(1):
				if(Time.time - punchLeftStartTime <= maxPunchTime)
				{
					if(leftWristToShoulderAngle >= punchEndAngle)
					{
						punchStateLeft = 0;
						script.PunchReceived(LeftWrist, true);
					}
				}
				else
					punchStateLeft = 0;
			break;
		}
		
		switch(punchStateRight)
		{
			case(0):
				if(rightWristToShoulderAngle <= punchStartAngle)
				{
					punchRightStartTime = Time.time;
					++punchStateRight;
				}
			break;
			case(1):
				if(Time.time - punchRightStartTime <= maxPunchTime)
				{
					if(rightWristToShoulderAngle >= punchEndAngle)
					{
						punchStateRight = 0;
						script.PunchReceived(RightWrist, false);
					}
				}
				else
					punchStateRight = 0;
			break;
		}
	}
	
	float AngleBetweenTransformsGivenPivot(Transform a, Transform b, Transform pivot)
	{
		Vector3 side1 = a.position - pivot.position;
		Vector3 side2 = b.position - pivot.position;

		return Vector3.Angle(side1, side2);
	}
}
