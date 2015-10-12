using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForegroundGenerator : MonoBehaviour {

   public GameObject[] foregroundObjects;

   GameObject[] objects;

   public float objectDensity;
   public float depth;
   public float depthVariation;

   void Start()
   {
      objects = new GameObject[1];
   }

   public void GenerateObjects()
   {
      Camera cam = Camera.main;

      Vector3 BottomLeft = cam.ViewportToWorldPoint(new Vector3(0.2f, 0.2f));
      Vector3 TopRight = cam.ViewportToWorldPoint(new Vector3(0.8f, 0.8f));

      Debug.Log(TopRight + " " + BottomLeft);

      float xDist = TopRight.x - BottomLeft.x;
      float yDist = TopRight.y - BottomLeft.y;

      int numberX = Mathf.FloorToInt(xDist / objectDensity);
      int numberY = Mathf.FloorToInt(yDist / objectDensity);

      List<Vector3> spawnPoints = new List<Vector3>();

      for (int i = 0; i < numberX; ++i)
      {
         for (int j = 0; j < numberY; ++j)
         {
            spawnPoints.Add(new Vector3(BottomLeft.x + xDist * ((float)i / numberX), 
                                        BottomLeft.y + yDist * ((float)j / numberY), depth));
         }
      }

      float xVariation = (TopRight.x - BottomLeft.x) / numberX / 5;
      float yVariation = (TopRight.y - BottomLeft.y) / numberY / 5;

      for (int i = 0; i < spawnPoints.Count; ++i)
      {
         spawnPoints[i] = spawnPoints[i] + new Vector3(Random.Range(-xVariation, xVariation),
                                                       Random.Range(-yVariation, yVariation),
                                                       Random.Range(-depthVariation / 2, depthVariation / 2));
      }

      objects = new GameObject[spawnPoints.Count];



      for (int i = 0; i < spawnPoints.Count; ++i)
      {
         GameObject foregroundObject = foregroundObjects[Random.Range(0, foregroundObjects.Length)];
         GameObject newObject = (GameObject)Instantiate(foregroundObject, spawnPoints[i], Quaternion.identity);
         objects[i] = newObject;
      }
   }

   public void CleanupForeground()
   {
      for (int i = 0; i < objects.Length; ++i)
      {
         Destroy(objects[i]);
      }
   }
}
