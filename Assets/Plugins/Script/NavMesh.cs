using UnityEngine;
using System.Collections;
using StoneAnt.LeapMotion;

public class NavMesh : MonoBehaviour {
	//private LeapMotion lm;
	private LeapMotionGesture LMG;
	private LeapMotionParameter LMP;
	private NavMeshAgent man;
	private Transform target;
	
	private string CurrentRoute = "Cube_Target";
	private string NextRoute;
	private int Sections = 0;
	
	private bool StartWalking = true;
	
	// Use this for initialization
	void Start () {
		man = gameObject.GetComponent<NavMeshAgent>();
		//lm = new LeapMotion();
		LMG = new LeapMotionGesture();
		LMP = new LeapMotionParameter();
		NextRoute = CurrentRoute;
		target = GameObject.Find(CurrentRoute+"0").transform;
	}
	/*
	// Update is called once per frame
	void Update () {
		if(LMP.GetFingersNumber() == 5)
		{
			if(man.destination == man.nextPosition)//Stagnant
			{
				//if(man.destination != target.position)//problem:Invalid
				if(StartWalking)
				{
					man.SetDestination(target.position);
					Debug.Log("man.SetDestination");
					StartWalking = false;
				}
				else if(GameObject.Find(CurrentRoute + (++Sections).ToString() ))
				{
					Debug.Log("find: "+CurrentRoute+"_"+Sections);
					target = GameObject.Find(CurrentRoute + Sections.ToString() ).transform;
					StartWalking = true;
				}
				else
				{
					Debug.Log(CurrentRoute + " end");
					StartWalking = false;
					if(LMG.Circle() == 1)
					{
						Debug.Log("--->");
						setNextRoute("c");
					}
					else if(LMG.Circle() == 2)
					{
						Debug.Log("<---");
						setNextRoute("b");
					}
				}
			}
		}
		else if(LMP.GetFingersNumber() < 4)
		{
			man.SetDestination(man.nextPosition);
			StartWalking = true;
		}
	}
*/

	void Update () 
	{
		if(LMG.FingersExpand())
		{
			Debug.Log("FingersExpand");
		}
		if(LMG.FingersShrink())
		{
			Debug.Log("FingersShrink");
		}
		if(LMG.Circle() == 1)
		{
			Debug.Log("Circle 1");
		}
		if(LMG.Circle() == 2)
		{
			Debug.Log("Circle 2");
		}
	}

	public void setNextRoute(string route)
	{
		CurrentRoute = route ;
		Sections = -1;
		StartWalking = true;
	}
	
}
