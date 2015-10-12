using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
      HUDMgr.Instance.HideMessage();
      HUDMgr.Instance.DisableGameplayHUD();
	}
	
	// Update is called once per frame
	void Update () {
      if (Input.GetMouseButtonDown(0))
      {
         HUDMgr.Instance.HideMessage();
         StartTransitionAnimation();
      }
	}

   void StartTransitionAnimation()
   {
      LevelMgr.Instance.LevelWon(true);
      HUDMgr.Instance.EnableGameplayHUD();
      AudioMgr.Instance.PlayAmbience();
   }
}
