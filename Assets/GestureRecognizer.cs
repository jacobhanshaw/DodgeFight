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
	float punchEndAngle = 165.0f;
	float maxPunchTime = 3.0f;
	
	int stompStateLeft = 0;
	float stompLeftStartTime = 0;
	int stompStateRight = 0;
	float stompRightStartTime = 0;
	
	float stompStartAngle = 70.0f;
	float stompEndAngle = 160.0f;
	float maxStompTime = 3.0f;
	
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
		CheckForStomp();
	}
	
	void CheckForPunch ()
	{
		//Debug.Log("Left State: " + punchStateLeft);
		//Debug.Log("Right State: " + punchStateRight);
		
		float leftWristToShoulderAngle = AngleBetweenTransformsGivenPivot(LeftShoulder, LeftWrist, LeftElbow);
		float rightWristToShoulderAngle = AngleBetweenTransformsGivenPivot(RightShoulder, RightWrist, RightElbow);
		
		//script.UpdateUI(leftWristToShoulderAngle, rightWristToShoulderAngle);
		
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
					if(leftWristToShoulderAngle <= punchStartAngle)
						punchLeftStartTime = Time.time;
					else if(leftWristToShoulderAngle >= punchEndAngle)
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
					if(rightWristToShoulderAngle <= punchStartAngle)
						punchRightStartTime = Time.time;
					else if(rightWristToShoulderAngle >= punchEndAngle)
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
	
	void CheckForStomp ()
	{
		float leftFootToHipAngle = AngleBetweenTransformsGivenPivot(LeftHip, LeftAnkle, LeftKnee);
		float rightFootToHipAngle = AngleBetweenTransformsGivenPivot(RightHip, RightAnkle, RightKnee);
		
		script.UpdateUI(leftFootToHipAngle, rightFootToHipAngle);
		
		switch(stompStateLeft)
		{
			case(0):
				if(leftFootToHipAngle <= stompStartAngle)
				{
					stompLeftStartTime = Time.time;
					++stompStateLeft;
				}
			break;
			case(1):
				if(Time.time - stompLeftStartTime <= maxStompTime)
				{
					if(leftFootToHipAngle <= stompStartAngle)
						stompLeftStartTime = Time.time;
					else if(leftFootToHipAngle >= stompEndAngle)
					{
						stompStateLeft = 0;
						script.StompReceived(LeftAnkle);
					}
				}
				else
					stompStateLeft = 0;
			break;
		}
		
		switch(stompStateRight)
		{
			case(0):
				if(rightFootToHipAngle <= stompStartAngle)
				{
					stompRightStartTime = Time.time;
					++stompStateRight;
				}
			break;
			case(1):
				if(Time.time - stompRightStartTime <= maxStompTime)
				{
					if(rightFootToHipAngle <= stompStartAngle)
						stompRightStartTime = Time.time;
					else if(rightFootToHipAngle >= stompEndAngle)
					{
						stompStateRight = 0;
						script.StompReceived(RightAnkle);
					}
				}
				else
					stompStateRight = 0;
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
