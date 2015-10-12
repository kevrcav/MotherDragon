using UnityEngine;
using System.Collections;

public class Monument : MonoBehaviour {

   Rigidbody2D rigidbody;

   bool beenHit;

   void Start()
   {
      rigidbody = GetComponent<Rigidbody2D>();
      rigidbody.isKinematic = true;
   }

   public void Hit(Vector2 force, Vector2 position)
   {
      rigidbody.isKinematic = false;
      StartCoroutine(HitCoroutine(force, position));
      if (!beenHit)
      {
         LevelMgr.Instance.LevelWon();
         beenHit = true;
      }
   }

   IEnumerator HitCoroutine(Vector2 force, Vector2 position)
   {
      yield return new WaitForSeconds(0.01f);
      rigidbody.AddForceAtPosition(force, position);
   }
}
