using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leap;

namespace StoneAnt.LeapMotion
{
    /// <summary>
    /// 获取LeapMotion各种参数
    /// </summary>
    public class LeapMotionParameter
    {
        private Frame LMFrame;
        private int HandsNumber = 0;
        private int FingersNumber = 0;

        /// <summary>
        /// 获取当前识别到的手的数量
        /// </summary>
        /// <returns></returns>
        public int GetHandsNumber()
        {
            LMFrame = LeapMotionInitialize.GetFrame();
            HandList Hands = LMFrame.Hands;
            HandsNumber = Hands.Count;
            return HandsNumber;
        }

        /// <summary>
        /// 获取第一只手被识别出的手指数量
        /// </summary>
        /// <returns></returns>
        public int GetFingersNumber()
        {
            if(this.GetHandsNumber() == 0)
            {
                return 0;
            }
            LMFrame = LeapMotionInitialize.GetFrame();
            Hand hand = LMFrame.Hands[0];
            FingerList Fingers = hand.Fingers;
            FingersNumber = Fingers.Count;
            return Fingers.Count;
        }

        /// <summary>
        /// 获取第一只手手掌的坐标
        /// </summary>
        /// <returns></returns>
        public Vector GetPalmPosition()
        {

            return new Vector(0,0,0);
        }

        /// <summary>
        /// 获取第一只手的第一根手指指尖的坐标
        /// </summary>
        /// <returns></returns>
        public Vector GetFingertipPosition()
        {
            LMFrame = LeapMotionInitialize.GetFrame();
            return LMFrame.Hands[0].Fingers[0].TipPosition;
        }







    }
}
