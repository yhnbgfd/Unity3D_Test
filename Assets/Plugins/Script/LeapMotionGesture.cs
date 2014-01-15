using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leap;

namespace StoneAnt.LeapMotion
{
	/// <summary>
	/// LeapMotion 手势
	/// </summary>
	public class LeapMotionGesture
	{
		/// <summary>
		/// 画圈手势
		/// 顺时针返回：1
		/// 逆时针返回：2
		/// </summary>
		/// <returns></returns>
		public int Circle()
		{
			Frame LMFrame = LeapMotionInitialize.GetFrame();
			GestureList gestures = LMFrame.Gestures();
			for (int g = 0; g < gestures.Count; g++)
			{
				if (gestures[g].Type == Gesture.GestureType.TYPECIRCLE)
				{
					CircleGesture circle = new CircleGesture(gestures[g]);
					if (circle.Radius < 38.0f) return 0;
					if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 2)
					{
						return 1;//Clockwise
					}
					else
					{
						return 2;//Counterclockwise
					}
				}
			}
			return 0;
		}
		
		/// <summary>
		/// 五指收缩手势
		/// </summary>
		/// <returns></returns>
		public bool FingersShrink()
		{
			int CountFrames = 20;
			
			Frame LMFrame;
			Hand Hand;
			FingerList Fingers;
			
			bool StartCount = false;
			int Count = 0;
			
			for (int f = CountFrames; f > 0; f--)
			{
				LMFrame = LeapMotionInitialize.GetFrame(f);
				Hand = LMFrame.Hands[0];
				Fingers = Hand.Fingers;
				if(Fingers.Count >= 4)
				{
					StartCount = true;
				}
				if(StartCount)
				{
					if(Fingers.Count == 0 && LMFrame.Hands.Count > 0)
					{
						Count++;
					}
				}
				else if (f < CountFrames / 2)//超过一半还没开始统计就不用统计了
				{
					return false;
				}
			}
			if (Count >= CountFrames / 2)
			{
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// 五指张开手势
		/// </summary>
		/// <returns></returns>
		public bool FingersExpand()
		{
			int CountFrames = 20;
			
			Frame LMFrame;
			Hand Hand;
			FingerList Fingers;
			
			bool StartCount = false;
			int Count = 0;
			
			for (int f = CountFrames; f > 0; f--)
			{
				LMFrame = LeapMotionInitialize.GetFrame(f);
				Hand = LMFrame.Hands[0];
				Fingers = Hand.Fingers;
				if (Fingers.Count == 0 && LMFrame.Hands.Count > 0)
				{
					StartCount = true;
				}
				if (StartCount)
				{
					if (Fingers.Count >= 4)
					{
						Count++;
					}
				}
				else if (f < CountFrames / 2)//超过一半还没开始统计就不用统计了
				{
					return false;
				}
			}
			if (Count >= CountFrames / 2)
			{
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// 滑动手势
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public int Swipe(string axis)
		{
			int result = 0;
			Frame LMFrame = LeapMotionInitialize.GetFrame();
			GestureList Gestures = LMFrame.Gestures();
			for (int g = 0; g < Gestures.Count; g++ )
			{
				if(Gestures[g].Type == Gesture.GestureType.TYPESWIPE)
				{
					SwipeGesture Swipe = new SwipeGesture(Gestures[g]);
					switch(axis.ToLower())
					{
					case "x":
						if (Swipe.Speed < 500 
						    || Math.Abs(Swipe.Position.x - Swipe.StartPosition.x) < 200 
						    || Swipe.Position.y - Swipe.StartPosition.y <10)
						{
							continue;
						}
						if(Swipe.Direction.x > 0)
						{
							return 1;
						}
						else if(Swipe.Direction.x < 0)
						{
							return 2;
						}
						break;
					case "y":
						if (Swipe.Direction.y > 0.95f)
						{
							if (Swipe.Speed > 1000 && Swipe.Position.y > 200)
							{
								return 1;
							}
						}
						else if (Swipe.Direction.y < -0.95f)
						{
							if (Swipe.Speed > 1000 && Swipe.Position.y < 100)
							{
								return 2;
							}
						}
						break;
					case "z":
						if (Swipe.Speed < 500)
						{
							continue;
						}
						if (Swipe.Direction.z > 0)
						{
							return 1;
						}
						else if (Swipe.Direction.z < 0)
						{
							return 2;
						}
						break;
					}
				}
			}
			return result;
		}
		
		
	}
}
