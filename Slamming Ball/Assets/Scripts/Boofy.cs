using UnityEngine;
using System.Collections;

public class Boofy : MonoBehaviour {

   Rigidbody2D rigidbody;

   Vector2 colTangent = Vector2.down;

   public float speedMod = 1.0f;
   float groundSpeed = 1;


   bool grounded;
   bool justEnteredGround;
   bool angryCoolingDown;

	// Use this for initialization
	void Start () {
      rigidbody = GetComponent<Rigidbody2D>();
      rigidbody.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
      if (grounded)
         rigidbody.AddForce(colTangent * groundSpeed);
	}

   public void StartMovement()
   {
      rigidbody.isKinematic = false;
   }

   public void EndMovement(Vector3 resetPosition)
   {
      transform.position = resetPosition;
      rigidbody.isKinematic = true;
      grounded = false;
      transform.rotation = Quaternion.identity;
   }

   void OnCollisionEnter2D(Collision2D col)
   {
      if (col.gameObject.tag == "Monument")
      {
         col.gameObject.GetComponent<Monument>().Hit(rigidbody.velocity * rigidbody.mass * 10, col.contacts[0].point);
         AudioMgr.Instance.PlayHappyChirp();
         return;
      }
      else if (col.gameObject.layer == 9) return;
      else if (col.gameObject.layer == 10)
      {
         AudioMgr.Instance.PlayAngryChirp();
         StartCoroutine(AngryCooldown());
         return;
      }
      Vector2 normal = col.contacts[0].normal;
      colTangent = new Vector2(normal.y, -normal.x);
      groundSpeed = col.gameObject.GetComponent<Curve>().GetBallSpeed()*speedMod;
      grounded = true;
      StartCoroutine(JustEnteredCoroutine());
   }

   IEnumerator AngryCooldown()
   {
      angryCoolingDown = true;
      yield return new WaitForSeconds(1.0f);
      angryCoolingDown = false;
   }

   IEnumerator JustEnteredCoroutine()
   {
      justEnteredGround = true;
      for (int i = 0; i < 1; ++i)
      {
         yield return new WaitForEndOfFrame();
      }
      justEnteredGround = false;
   }

   void OnCollisionExit2D(Collision2D col)
   {
      if (!justEnteredGround)
         grounded = false;
   }
}
