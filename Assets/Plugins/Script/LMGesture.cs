﻿using UnityEngine;
using System.Collections;

public class LMGesture {
	private LeapMotion lm;
	private float mouseX = 0.0f;
	private float mouseY = 0.0f;
	private float introY = 0.0f;
	
	public LMGesture () {
		lm = new LeapMotion();
	}
	
	public bool CheckLMConnection()
	{
		if(lm.CheckLMConnection())
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public float MoveHorizontal()
	{
		Vector3 movePosition = lm.PalmPosition(0,0,0,0);
		float x = movePosition.x;
		return x;
	}
	public float MoveVertical()
	{
		Vector3 movePosition = lm.PalmPosition(0,0,0,0);
		float z = movePosition.z;
		return z;
	}
	
	public float getMouseX() {
		if(lm.getHandsNum() == 0 || lm.getFingersNum() > 2)
		{
			return 0.0f;
		}
		Vector3 lookPosition = lm.FingertipPosition();
		if(lookPosition.x > 0)
		{
			if(lookPosition.x > 100)
			{
				if(mouseX < 0.1f )
				{
					mouseX += 0.01f;
				}
				else if (mouseX >= 0.1f)
				{
					mouseX = 0.1f;
				}
			}
			else
			{
				if(mouseX > 0.01f)
				{
					mouseX -= 0.01f;
				}
				else
				{
					mouseX = 0.0f;
				}
			}
		}
		else if(lookPosition.x < 0)
		{
			if(lookPosition.x < -100)
			{
				if(mouseX > -0.1f )
				{
					mouseX -= 0.01f;
				}
				else if (mouseX <= -0.1f)
				{
					mouseX = -0.1f;
				}
			}
			else
			{
				if(mouseX < -0.01f)
				{
					mouseX += 0.01f;
				}
				else
				{
					mouseX = 0.0f;
				}
			}
			
		}
		return mouseX;//-0.1 ~ 0.1(0.2)
	}
	
	public float getMouseY() {
		if(lm.getHandsNum() == 0 || lm.getFingersNum() > 2)
		{
			return 0.0f;
		}
		if(lm.HandEnter())
		{
			introY = lm.FingertipPosition().y;
		}
		introY = 200.0f;
		Vector3 lookPosition = lm.FingertipPosition();
		if(lookPosition.y - introY > 50)
		{
			return 0.05f;
		}
		else if(lookPosition.y - introY < -50)
		{
			return -0.05f;
		}
		return mouseY;
	}
}