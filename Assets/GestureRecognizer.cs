using UnityEngine;
using System.Collections;

public class GestureRecognizer : MonoBehaviour 
{
	public CharacterScript script;
	public CoreLogic  CoreLogicScript;
	
	//PUNCH VARIABLES
	int punchStateLeft = 0;
	float punchLeftStartTime = 0;
	int punchStateRight = 0;
	float punchRightStartTime = 0;
	float startArmDistance = 0.3f * 0.3f;
	float stopArmDistance = 0.543f * 0.453f;
	//float punchStartAngle = 40.0f;
	//float punchEndAngle = 165.0f;
	float maxPunchTime = 0.75f;
	
	//CHARGE VARIABLES
	int   chargeState = 0;
	float chargeHandDistance = 0.2f * 0.2f;

	//JUMP VARIABLES
	float floorHeight = float.MaxValue;
	float minJumpHeight = 0.5f;
	int  jumpState = 0;
	float jumpSpeed = 0.2f;
	float jumpStartTime = 0.0f;
	float jumpTime = 0.5f;
	float hangTime = 1.0f;
	
	int stompStateLeft = 0;
	float stompLeftStartTime = 0;
	int stompStateRight = 0;
	float stompRightStartTime = 0;
	
	float startLegDistance = 0.6f * 0.6f;
	float stopLegDistance = 0.78f * 0.78f;
	//float stompStartAngle = 70.0f;
	//float stompEndAngle = 160.0f;
	float maxStompTime = 1.5f;
	
	public Transform Head;
    public Transform Neck;
    public Transform Torso;
    public Transform Waist;


    public Transform LeftShoulder;
    public Transform LeftElbow;
    public Transform LeftWrist;

    public Transform RightShoulder;
    public Transform RightElbow;
    public Transform RightWrist;

    public Transform LeftHip;
    public Transform LeftKnee;
    public Transform LeftAnkle;

    public Transform RightHip;
    public Transform RightKnee;
    public Transform RightAnkle;
	
	// Use this for initialization
	void Start () 
	{
	//	minLeftArmDistance  *= gameObject.transform.localScale.x;
	//	maxLeftArmDistance  *= gameObject.transform.localScale.x;
	//	minRightArmDistance *= gameObject.transform.localScale.x;
	//	maxRightArmDistance *= gameObject.transform.localScale.x;
	
		floorHeight = script.gameObject.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckForCharge();
		CheckForPunch();
	//	bool jump = CheckForJump(); DO DIFFERENTLY, USE GESTURE RECOGNIZER
	//	if(!jump)
			CheckForStomp();
	}
	
	void CheckForCharge()
	{
		switch(chargeState)
		{
			case(0):
				if(GetDistanceSquared(RightWrist.transform, LeftWrist.transform) < chargeHandDistance)
				{
					CoreLogicScript.chargeTime = Time.deltaTime;
					++chargeState;
				}
			break;
			case(1):
				if(GetDistanceSquared(RightWrist.transform, LeftWrist.transform) < chargeHandDistance)
					CoreLogicScript.chargeTime += Time.deltaTime;
			break;
		}
		
		//script.UpdateUI(Vector3.Distance(RightWrist.transform.position, LeftWrist.transform.position), chargeTime);
	}
	
	void CheckForPunch ()
	{
	//	float leftWristToShoulderAngle = AngleBetweenTransformsGivenPivot(LeftShoulder, LeftWrist, LeftElbow);
	//	float rightWristToShoulderAngle = AngleBetweenTransformsGivenPivot(RightShoulder, RightWrist, RightElbow);
		
		//script.UpdateUI(Vector3.Distance(LeftShoulder.transform.position, LeftWrist.transform.position), Vector3.Distance(RightShoulder.transform.position, RightWrist.transform.position));
		
		float leftShoulderWristDistanceSquared = GetDistanceSquared(LeftShoulder.transform, LeftWrist.transform);
		float rightShoulderWristDistanceSquared = GetDistanceSquared(RightShoulder.transform, RightWrist.transform);
																						
		switch(punchStateLeft)
		{
			case(0):
				//if(leftWristToShoulderAngle <= punchStartAngle || 
				if(leftShoulderWristDistanceSquared < startArmDistance)
				{
					punchLeftStartTime = Time.time;
					++punchStateLeft;
				}
			break;
			case(1):
				if(Time.time - punchLeftStartTime <= maxPunchTime)
				{
					//if(leftWristToShoulderAngle <= punchStartAngle || 
					if(leftShoulderWristDistanceSquared < startArmDistance)
						punchLeftStartTime = Time.time;
					//else if(leftWristToShoulderAngle >= punchEndAngle || 
					else if(leftShoulderWristDistanceSquared > stopArmDistance)
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
				//if(rightWristToShoulderAngle <= punchStartAngle || 
				if(rightShoulderWristDistanceSquared < startArmDistance)
				{
					punchRightStartTime = Time.time;
					++punchStateRight;
				}
			break;
			case(1):
				if(Time.time - punchRightStartTime <= maxPunchTime)
				{
					//if(rightWristToShoulderAngle <= punchStartAngle || 
					if(rightShoulderWristDistanceSquared < startArmDistance)
						punchRightStartTime = Time.time;
					//else if(rightWristToShoulderAngle >= punchEndAngle || 
					else if(rightShoulderWristDistanceSquared > stopArmDistance)
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
	
	bool CheckForJump()
	{
		floorHeight = Mathf.Min(floorHeight, transform.position.y);
		
		switch(jumpState)
		{
			case(0):
			if(transform.position.y - floorHeight > minJumpHeight)
			{
				jumpStartTime = Time.time;
				transform.position += transform.up * jumpSpeed * Time.deltaTime;
				++jumpState;
			}
			break;
			case(1):
			if(Time.time - jumpStartTime < jumpTime)
			{
				transform.position += transform.up * jumpSpeed * Time.deltaTime;
			}
			else
				++jumpState;
			break;
			case(2):
			if(!(Time.time - jumpStartTime < jumpTime + hangTime))
				++jumpState;
			break;
			case(3):
			if(Time.time - jumpStartTime < 2 * jumpTime + hangTime)
			{
				transform.position -= transform.up * jumpSpeed * Time.deltaTime;
			}
			else
			{
				jumpState = 0;
				transform.position = transform.up * floorHeight;	
			}
			break;
		}
		
		script.UpdateUI(transform.position.y, floorHeight);
		
		return jumpState != 0;
	}
	
	void CheckForStomp ()
	{
	//	float leftFootToHipAngle = AngleBetweenTransformsGivenPivot(LeftHip, LeftAnkle, LeftKnee);
	//	float rightFootToHipAngle = AngleBetweenTransformsGivenPivot(RightHip, RightAnkle, RightKnee);
		
	//	script.UpdateUI(Vector3.Distance(LeftHip.transform.position, LeftAnkle.transform.position), Vector3.Distance(RightHip.transform.position, RightAnkle.transform.position));
		float leftHipAnkleDistanceSquared = GetDistanceSquared(LeftHip.transform, LeftAnkle.transform);
		float rightHipAnkleDistanceSquared = GetDistanceSquared(RightHip.transform, RightAnkle.transform);
		
		switch(stompStateLeft)
		{
			case(0):
				//if(leftFootToHipAngle <= stompStartAngle || 
				if(leftHipAnkleDistanceSquared < startLegDistance)
				{
					stompLeftStartTime = Time.time;
					++stompStateLeft;
				}
			break;
			case(1):
				if(Time.time - stompLeftStartTime <= maxStompTime)
				{
					//if(leftFootToHipAngle <= stompStartAngle || 
					if(leftHipAnkleDistanceSquared < startLegDistance)
						stompLeftStartTime = Time.time;
					//else if(leftFootToHipAngle >= stompEndAngle || 
					else if(leftHipAnkleDistanceSquared > stopLegDistance)
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
				//if(rightFootToHipAngle <= stompStartAngle || 
				if(rightHipAnkleDistanceSquared < startLegDistance)
				{
					stompRightStartTime = Time.time;
					++stompStateRight;
				}
			break;
			case(1):
				if(Time.time - stompRightStartTime <= maxStompTime)
				{
					
					//if(rightFootToHipAngle <= stompStartAngle || 
					if(rightHipAnkleDistanceSquared < startLegDistance)
						stompRightStartTime = Time.time;
					//else if(rightFootToHipAngle >= stompEndAngle || 
					else if(rightHipAnkleDistanceSquared > stopLegDistance)
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
	
	float GetDistanceSquared(Transform a, Transform b)
	{
		Vector3 offset = a.position - b.position;
        return offset.sqrMagnitude;
	}
	
}