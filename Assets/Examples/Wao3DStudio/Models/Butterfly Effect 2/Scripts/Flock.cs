using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {
	public float speed = 1f;
	float rotationSpeed = 4.0f;
	Vector3 avarageHeading;
	Vector3 avaragePosition;
	float neighbourDistance = 1.0f;
	//public bool startUpdate = false;
	bool turning = false;
	private Vector3 parentPos;
	private GlobalFlock GlobalFlock;
	// Use this for initialization
	void Start ()
	{
		parentPos= transform.parent.transform.position;
		GlobalFlock=transform.parent.GetComponent<GlobalFlock>();
		speed = Random.Range (1f, 2);
        GetComponent<Animator>().Play("Butterfly Effect 2 Fly Idle", -1, Random.Range(0f, 0.9f));

    }
	
	// Update is called once per frame
	void Update () 
	{
		//if(startUpdate==true)
		//{

			if (Vector3.Distance (transform.position, parentPos) >= GlobalFlock.areaSize) {
				
				turning = true;
			} else{
				
				turning = false;
				}
			if (turning) {
				
				Vector3 direction = parentPos - transform.position;
				transform.rotation = Quaternion.Slerp (transform.rotation,
					Quaternion.LookRotation (direction), rotationSpeed * Time.deltaTime);
				speed = Random.Range (0.5f, 1);

			}

			else 
			
			{
				
				if (Random.Range (0, 5) < 1){
					ApplyRules ();
					}

			}
			
			transform.Translate (0, 0, Time.deltaTime * speed);
		//}		 
	}
	void ApplyRules()
	{
		
		GameObject[] gos;
		gos = GlobalFlock.allButterfly;

		Vector3 goalPos = GlobalFlock.goalPos;

		Vector3 Vcentre = parentPos;
		Vector3 vavoid = parentPos;
		float gSpeed = 0.1f;



		float dist;

		int groupSize = 0;
		foreach (GameObject go in gos) 
		
		{
			if(go != this.gameObject)
			{
				dist = Vector3.Distance(go.transform.position, this.transform.position);
				if(dist <= neighbourDistance)
				{
					Vcentre += go.transform.position;
					groupSize++;

					if (dist < 1.0f)
					{
						vavoid = vavoid + (this.transform.position - go.transform.position);

					}
					Flock anotherFlock = go.GetComponent<Flock>();
					gSpeed = gSpeed + anotherFlock.speed ;

				}
			}
			
		}
		if(groupSize >0)
		{
			Vcentre = Vcentre/groupSize + (goalPos - this.transform.position);
			speed = gSpeed /groupSize;

			Vector3 direction = (Vcentre +vavoid) - transform.position;
			if(direction != Vector3.zero)
				transform.rotation = Quaternion.Slerp(transform.rotation,
					Quaternion.LookRotation(direction),
					rotationSpeed * Time.deltaTime);	
		}


	}
}
