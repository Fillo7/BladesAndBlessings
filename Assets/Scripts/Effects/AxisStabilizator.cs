using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisStabilizator : MonoBehaviour {

	[SerializeField] bool stabilizeX = false;
	[SerializeField] bool stabilizeY = false;
	[SerializeField] bool stabilizeZ = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.rotation = new Quaternion(stabilizeX? 0f: transform.rotation.x, stabilizeY? 0f: transform.rotation.y, stabilizeZ? 0f: transform.rotation.z, transform.rotation.w);

	}
}
