using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

   public float cameraSize;

   public Transform startLocation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public Vector3 GetStartLocation()
   {
      return startLocation.position;
   }
}
