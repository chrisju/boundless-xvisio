using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {
	public GameObject[] butterflyPrefab;
	public int areaSize = 3;

	public int numButterflies = 15;
	public GameObject[] allButterfly;
	//public GameObject goalPosGameobject;
	public Vector3 goalPos = Vector3.zero;


	// Use this for initialization
	void Start () 

	{
		allButterfly = new GameObject[numButterflies];
		goalPos = gameObject.transform.position;

		for (int i = 0; i< numButterflies; i++)
		{
			Vector3 pos = gameObject.transform.position + new Vector3 (Random.Range(-areaSize, areaSize),
			Random.Range(-areaSize,areaSize),
			Random.Range(-areaSize,areaSize));

            int rdm = Random.Range(0, butterflyPrefab.Length);
			allButterfly[i] = (GameObject) Instantiate(butterflyPrefab[rdm], pos, Quaternion.identity);
			allButterfly[i].transform.parent = gameObject.transform;
			//allButterfly[i].transform.position=new Vector3(0,0,0);
			//allButterfly[i].GetComponent<Flock>().startUpdate=true;
		}	
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range (0, 10000) < 50) 
		{
			goalPos = new Vector3 (Random.Range(-areaSize, areaSize),
				Random.Range(-areaSize,areaSize),
				Random.Range(-areaSize,areaSize)); 
		}
		
	}
}
