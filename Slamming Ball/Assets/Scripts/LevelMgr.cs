using UnityEngine;
using System.Collections;

public class LevelMgr : MonoBehaviour {

   public string[] levels;
   Level currentLevel;
   int levelIndex;

   public Boofy player;
   public BoofyCamera cam;
   public ForegroundGenerator fGenerator;

   public static LevelMgr Instance;

   bool simulating;

   void Awake()
   {
      Instance = this;
   }

   void Start()
   {
      levelIndex = -1;
      LoadNextLevel();
   }

   void Update()
   {
      if (Input.GetKeyDown(KeyCode.S))
      {
         if (simulating)
         {
            simulating = false;
            EndSimulation();
         }
         else
         {
            simulating = true;
            StartSimulation();
         }
      }
   }

   void LoadNextLevel()
   {
      if (++levelIndex >= levels.Length)
      {
         Win();
      }
      else
      {
         if (currentLevel != null)
            Destroy(currentLevel.gameObject);
         fGenerator.CleanupForeground();
         StartCoroutine(LoadNextLevelCoroutine());
      }

   }

   IEnumerator LoadNextLevelCoroutine()
   {
      yield return Application.LoadLevelAdditiveAsync(levels[levelIndex]);
      Level[] activeLevels = FindObjectsOfType<Level>();
      foreach (Level l in activeLevels)
      {
         if (l.name == levels[levelIndex])
         {
            currentLevel = l;
            break;
         }
      }
      player.EndMovement(currentLevel.GetStartLocation());
      cam.SetEditCamSize(currentLevel.cameraSize);
      cam.UseEditCamera();
      cam.EnableFollowCam(true);
      CurveMgr.Instance.ClearCurves();
      CurveMgr.Instance.SetCreationEnabled(true);
      HUDMgr.Instance.SimulationEnded();
      fGenerator.GenerateObjects();
   }

   public void StartSimulation()
   {
      CurveMgr.Instance.SetCreationEnabled(false);
      player.StartMovement();
      cam.UseBoofyCamera();
      HUDMgr.Instance.SimulationStarted();
   }

   public void EndSimulation()
   {
      CurveMgr.Instance.SetCreationEnabled(true);
      player.EndMovement(currentLevel.GetStartLocation());
      cam.UseEditCamera();
      HUDMgr.Instance.SimulationEnded();
   }

   public void LevelWon(bool instant = false)
   {
      if (instant)
         LoadNextLevel();
      else
         StartCoroutine(StartNextLevelAfterWait());
      cam.EnableFollowCam(false);
   }

   IEnumerator StartNextLevelAfterWait()
   {
      yield return new WaitForSeconds(2);
      LoadNextLevel();
   }

   void Win()
   {

   }
}
