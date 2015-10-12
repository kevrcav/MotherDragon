using UnityEngine;
using System.Collections;

public class BoofyMesh : MonoBehaviour {

   public Rigidbody2D boofy;

   public float turnedAngle;

   Vector3 turnAxis;
   
   void Start()
   {
      float radianAngle = Mathf.PI / 2 - turnedAngle * Mathf.PI / 180;
      turnAxis = Vector3.back * Mathf.Sin(radianAngle) + Vector3.right * Mathf.Cos(radianAngle);
      Debug.Log(turnAxis);
   }

	// Update is called once per frame
	void Update () {
      Quaternion q1 = Quaternion.AngleAxis(boofy.transform.rotation.eulerAngles.z, turnAxis);
      Quaternion q2 = Quaternion.AngleAxis(turnedAngle, Vector3.up);
      transform.rotation = q1 * q2;
      transform.position = boofy.transform.position;
	}
}
