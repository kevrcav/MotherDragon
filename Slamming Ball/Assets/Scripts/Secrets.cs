using UnityEngine;
using System.Collections;

public class Secrets : MonoBehaviour {

   bool played;
   public AudioSource secretAsset;


   void OnCollisionEnter2D(Collision2D col)
   {
      if (played) return;
      played = true;
      secretAsset.Play();
   }
}
