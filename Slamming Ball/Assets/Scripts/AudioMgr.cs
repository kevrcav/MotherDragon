using UnityEngine;
using System.Collections;

public class AudioMgr : MonoBehaviour {

   public static AudioMgr Instance;

   public AudioSource ambience;
   public AudioSource bgm;

   public AudioClip[] angryChirps;
   public AudioClip[] happyChirps;
   public AudioClip[] cries;

   

   void Awake()
   {
      Instance = this;
   }

   public void PlayAmbience()
   {
      ambience.Play();
   }

   public void PlayBGM()
   {
      bgm.Play();
   }

   public void PlayAngryChirp()
   {
      AudioClip chirp = angryChirps[Random.Range(0, angryChirps.Length)];
      AudioSource.PlayClipAtPoint(chirp, transform.position);
   }

   public void PlayHappyChirp()
   {
      AudioClip chirp = happyChirps[Random.Range(0, happyChirps.Length)];
      AudioSource.PlayClipAtPoint(chirp, transform.position);
   }

   public void PlayCry()
   {
      AudioClip cry = cries[Random.Range(0, cries.Length)];
      AudioSource.PlayClipAtPoint(cry, transform.position);
   }
	
}
