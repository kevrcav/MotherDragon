using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDMgr : MonoBehaviour {

   public static HUDMgr Instance;

   public Text message;
   public Image currentMaterial;
   public Image playIndicator;
   public GameObject gameplayHUD;

   public Sprite PauseIcon;
   public Sprite PlayIcon;

   void Awake()
   {
      Instance = this;
   }

   public void DisplayMessage(string msg)
   {
      message.text = msg;
      message.gameObject.SetActive(true);
   }

   public void HideMessage()
   {
      message.gameObject.SetActive(false);
   }

   public void ChangeCurrentMaterial(Sprite sprite)
   {
      currentMaterial.sprite = sprite;
   }

   public void DisableGameplayHUD()
   {
      gameplayHUD.SetActive(false);
   }

   public void EnableGameplayHUD()
   {
      gameplayHUD.SetActive(true);
   }

   public void SimulationStarted()
   {
      playIndicator.sprite = PlayIcon;
   }

   public void SimulationEnded()
   {
      playIndicator.sprite = PauseIcon;
   }
}
