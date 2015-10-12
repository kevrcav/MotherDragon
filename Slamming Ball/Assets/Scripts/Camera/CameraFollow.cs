using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

   public Transform target;

   public Bounds followBounds;

   bool cried;
	
	// Update is called once per frame
	void Update () {
      Vector3 newPos = target.position;
      newPos.z = transform.position.z;

      newPos = followBounds.ClosestPoint(newPos);

      transform.position = newPos;

      if (!cried && target.position.y < followBounds.min.y)
      {
         cried = true;
         AudioMgr.Instance.PlayCry();
      }
	}

   void OnEnable()
   {
      cried = false;
   }
}
