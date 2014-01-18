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
			int[] FingerId = new int[5];    //记录手指的id
			
			bool StartCount = false;
			int Count = 0;
			
			for (int f = CountFrames; f > 0; f--)
			{
				LMFrame = LeapMotionInitialize.GetFrame(f);
				Hand = LMFrame.Hands[0];
				Fingers = Hand.Fingers;
				if(Fingers.Count == 5)
				{
					for (int i = 0; i < 5; i++ )
					{
						FingerId[i] = Fingers[i].Id;
					}
					StartCount = true;
				}
				if(StartCount)
				{
					if(Fingers.Count == 1)
					{
						if (Fingers[0].Id == FingerId[4])   //经测验，五指的时候，拇指id在最后一位
						{
							Count++;
						}
					}
					else if(Fingers.Count == 0 && LMFrame.Hands.Count > 0)
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
			bool StartCountWithThumb = false; 
			int Count = 0;
			int ThumbID = 0;
			
			for (int f = CountFrames; f > 0; f--)
			{
				LMFrame = LeapMotionInitialize.GetFrame(f);
				Hand = LMFrame.Hands[0];
				Fingers = Hand.Fingers;
				if (Fingers.Count == 0 && LMFrame.Hands.Count > 0)
				{
					StartCount = true;
				}
				else if (Fingers.Count == 1)
				{
					ThumbID = Fingers[0].Id;
					StartCountWithThumb = true;
				}
				if (Fingers.Count == 5)
				{
					if (StartCount) //0手指开始统计
					{
						Count++;
					}
					else if (StartCountWithThumb)    //大拇指开始统计
					{
						if (ThumbID == Fingers[4].Id)
						{
							Count++;
						}
						else
						{
							StartCountWithThumb = false;
						}
					}
					else if (f < CountFrames / 2)//超过一半还没开始统计就不用统计了
					{
						return false;
					}
				}
			}
			if (Count >= CountFrames / 2)
			{
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// 手掌滑动手势
		/// 参数：坐标轴X Y Z
		/// 返回：1 正方向、2 负方向
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public int Swipe(string axis)
		{
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
						    || Math.Abs(Swipe.Position.x - Swipe.StartPosition.x) < 200     //滑动距离大于200
						    || Swipe.Position.y - Swipe.StartPosition.y <10)    //斜向上滑动才给通过
						{
							continue;
						}
						if(Swipe.Direction.x > 0.9f)
						{
							return 1;
						}
						else if(Swipe.Direction.x < -0.9f)
						{
							return 2;
						}
						break;
					case "y":
						if(Swipe.Speed < 800)
						{
							continue;
						}
						if (Swipe.Direction.y > 0.95f)
						{
							return 1;
						}
						else if (Swipe.Direction.y < -0.95f)
						{
							return 2;
						}
						break;
					case "z":
						if (Swipe.Speed < 500)
						{
							continue;
						}
						if (Swipe.Direction.z > 0.9f)
						{
							return 1;
						}
						else if (Swipe.Direction.z < -0.9f)
						{
							return 2;
						}
						break;
					default:
						return 0;
					}
				}
			}
			return 0;
		}
		
		/// <summary>
		/// 手指滑动手势
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public int FingerSwipe(string axis)
		{
			Frame LMFrame = LeapMotionInitialize.GetFrame();
			Finger finger = LMFrame.Hands[0].Fingers[0];
			Vector tipVeloctiy = finger.TipVelocity;
			switch(axis.ToLower())
			{
			case "x":
				if(Math.Abs(tipVeloctiy.x) > 1000 && tipVeloctiy.y > 0)
				{
					if(tipVeloctiy.x > 0)
					{
						return 1;
					}
					else if(tipVeloctiy.x < 0)
					{
						return 2;
					}
				}
				break;
			case "y":
				if(tipVeloctiy.y > 1000)
				{
					return 1;
				}
				else if(tipVeloctiy.y < -1000)
				{
					return 2;
				}
				break;
			case "z":
				if(tipVeloctiy.z > 500)
				{
					return 1;
				}
				else if(tipVeloctiy.z < -500)
				{
					return 2;
				}
				break;
			default:
				return 0;
			}
			return 0;
		}
		
		
		/// <summary>
		/// 两只手滑动
		/// 触发效果非常差
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public int SwipeWithTwoHands(string axis)
		{
			Frame LMFrame = LeapMotionInitialize.GetFrame();
			GestureList gestures = LMFrame.Gestures();
			SwipeGesture swipeNegative = null;     //负方向
			SwipeGesture swipePositive = null;     //正方向
			for (int g = 0; g < gestures.Count; g++ )
			{
				if(gestures[g].Type == Gesture.GestureType.TYPESWIPE)
				{
					SwipeGesture swipe = new SwipeGesture(gestures[g]);
					float direction = 0.0f;
					switch(axis.ToLower())
					{
					case "x":
						direction = swipe.Direction.x;
						break;
					case "y":
						direction = swipe.Direction.y;
						break;
					case "z":
						direction = swipe.Direction.z;
						break;
					default:
						return 0;
					}
					if (swipePositive == null)  //未发现正方向手势
					{
						if (direction > 0.9f)
						{
							swipePositive = swipe;
						}
					}
					else if(swipeNegative == null)  //未发现负方向手势
					{
						if (direction < -0.9f)
						{
							swipeNegative = swipe;
						}
					}
					else /*if (swipeNegative && swipePositive)*/    //两个方向的手势齐全
					{
						float StartPositionNegative = 0.0f;
						float StartPositionPositive = 0.0f;
						switch(axis.ToLower())
						{
						case "x":
							StartPositionNegative = swipeNegative.StartPosition.x;
							StartPositionPositive = swipePositive.StartPosition.x;
							break;
						case "y":
							StartPositionNegative = swipeNegative.StartPosition.y;
							StartPositionPositive = swipePositive.StartPosition.y;
							break;
						case "z":
							StartPositionNegative = swipeNegative.StartPosition.z;
							StartPositionPositive = swipePositive.StartPosition.z;
							break;
						}
						if (StartPositionNegative < StartPositionPositive)
						{
							return 1;   //<>
						}
						else if (StartPositionNegative > StartPositionPositive)
						{
							return 2;   //><
						}
					}
				}
			}
			return 0;
		}
		
		/// <summary>
		/// 两只手指滑动
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public int SwipeWithTwoFingers(string axis)
		{
			Frame LMFrame = LeapMotionInitialize.GetFrame();
			if(LMFrame.Hands.Count >= 2)
			{
				
			}
			return 0;
		}
	}
}
