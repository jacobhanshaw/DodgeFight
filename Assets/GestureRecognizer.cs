using UnityEngine;
using System.Collections;

public class GestureRecognizer : MonoBehaviour 
{
	public CharacterScript script;
	
	private bool newUser;
	
	//PUNCH VARIABLES
	int punchStateLeft = 0;
	float punchLeftStartTime = 0;
	int punchStateRight = 0;
	float punchRightStartTime = 0;
	float startArmDistance = 0.3f * 0.3f;
	float stopArmDistance = 0.547f * 0.547f; //0.543f
	float maxPunchTime = 0.5f;
	
	//CHARGE VARIABLES
	bool  wasCharging;
	float chargeTimeDivider = 3.0f;
	float chargeHandDistance = 0.24f * 0.24f;
	
	//JUMP VARIABLES
	Vector3 floorRootPosition;
	Vector3 currentRootPosition;
	float jumpStartTime = 0.0f;
	float minJumpHeight = 0.05f;
	int  jumpState = 0;
	float jumpSpeed = 9.0f;
	float jumpTime = 0.75f;
	float hangTime = 3.0f;
	
	//STOMP VARIABLES
	int stompStateLeft = 0;
	float stompLeftStartTime = 0;
	int stompStateRight = 0;
	float stompRightStartTime = 0;
	float startLegDistance = 0.6f * 0.6f;
	float stopLegDistance = 0.78f * 0.78f;
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
	
	//private float time;
	private bool  done;
	
	void Start () 
	{

	}
	
	void Update () 
	{
		CheckForCharge();
		CheckForPunch();
		bool jump = CheckForJump(); 
		if(!jump)
			CheckForStomp();
	}
	
	void testJump()
	{
		if(Time.time> 3.0f && !done)
		{
			done = true;
			jumpState = 1;
			jumpStartTime = Time.time;
		} 
	}
	
	void testPunch()
	{
		if(!done && ((int)Time.time % 2) == 0)
		{
			done = true;
			script.leftChargeTime += Time.time;
			script.PunchReceived(LeftWrist, true);
		}
		else if(((int)Time.time % 2) == 1)
			done = false;
	}
	
	public void NewUserFound()
	{
		newUser = true;
	}
	
	public void RootUpdate(Vector3 rootPosition)
	{
		if(newUser)
		{
			newUser = false;
			floorRootPosition = rootPosition;
		}
		
		currentRootPosition = rootPosition;
	}
	
	void CheckForCharge()
	{
		if(GetDistanceSquared(RightWrist.transform, LeftWrist.transform) < chargeHandDistance)
		{
			if(!wasCharging)
			{
				wasCharging = true;
				script.StartedCharging();
			}
			script.leftChargeTime += Time.deltaTime/chargeTimeDivider;
			script.rightChargeTime += Time.deltaTime/chargeTimeDivider;
		}
		else
		{
			if(wasCharging)
			{
				wasCharging = false;
				script.StoppedCharging();
			}
		}
	}
	
	void CheckForPunch ()
	{
		float leftShoulderWristDistanceSquared = GetDistanceSquared(LeftShoulder.transform, LeftWrist.transform);
		float rightShoulderWristDistanceSquared = GetDistanceSquared(RightShoulder.transform, RightWrist.transform);
																						
		switch(punchStateLeft)
		{
			case(0):
				if(leftShoulderWristDistanceSquared < startArmDistance)
				{
					punchLeftStartTime = Time.time;
					++punchStateLeft;
				}
			break;
			case(1):
				if(Time.time - punchLeftStartTime <= maxPunchTime)
				{
					if(leftShoulderWristDistanceSquared < startArmDistance)
						punchLeftStartTime = Time.time;
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
				if(rightShoulderWristDistanceSquared < startArmDistance)
				{
					punchRightStartTime = Time.time;
					++punchStateRight;
				}
			break;
			case(1):
				if(Time.time - punchRightStartTime <= maxPunchTime)
				{
					if(rightShoulderWristDistanceSquared < startArmDistance)
						punchRightStartTime = Time.time;
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
		script.UpdateUI(floorRootPosition.y, currentRootPosition.y);
		switch(jumpState)
		{
			case(0):
			if(currentRootPosition.y - floorRootPosition.y > minJumpHeight)
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
				transform.position = transform.up * floorRootPosition.y;	
			}
			break;
		}
		
		return jumpState != 0;
	}
	
	void CheckForStomp ()
	{
		float leftHipAnkleDistanceSquared = GetDistanceSquared(LeftHip.transform, LeftAnkle.transform);
		float rightHipAnkleDistanceSquared = GetDistanceSquared(RightHip.transform, RightAnkle.transform);
		
		switch(stompStateLeft)
		{
			case(0):
				if(leftHipAnkleDistanceSquared < startLegDistance)
				{
					stompLeftStartTime = Time.time;
					++stompStateLeft;
				}
			break;
			case(1):
				if(Time.time - stompLeftStartTime <= maxStompTime)
				{
					if(leftHipAnkleDistanceSquared < startLegDistance)
						stompLeftStartTime = Time.time;
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
				if(rightHipAnkleDistanceSquared < startLegDistance)
				{
					stompRightStartTime = Time.time;
					++stompStateRight;
				}
			break;
			case(1):
				if(Time.time - stompRightStartTime <= maxStompTime)
				{
					if(rightHipAnkleDistanceSquared < startLegDistance)
						stompRightStartTime = Time.time;
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