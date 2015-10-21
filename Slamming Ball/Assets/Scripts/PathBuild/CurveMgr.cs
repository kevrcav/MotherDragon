using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public struct CurveMaterial
{
   public float ball_speed;
   public Texture texture;
   public Sprite UITexture;
   public string matType;
   
   public bool magnetic;
}

public enum CurveProperties
{
   kMagnetic = 1
}


public class CurveMgr : MonoBehaviour {

   public static CurveMgr Instance;

   public BezierCurveMeshGenerator defaultCurve;

   List<Curve> curves;

   public CurveMaterial[] materials;

   public int currentMaterial;

   public Curve currentCurve;

   public CurvePlacer placer;

   public RawImage currentMaterialIndicator;

   bool editEnabled = true;

   void Awake()
   {
      Instance = this;
      curves = new List<Curve>();
   }

   public BezierCurveMeshGenerator NewCurve(Vector3 pos, Vector2 startPoint, Vector2 startTangent, Vector2 endPoint, Vector2 endTangent, bool continuation)
   {
      curves.Add(currentCurve);
      BezierCurveMeshGenerator newCurve = (BezierCurveMeshGenerator)Instantiate(defaultCurve, pos, Quaternion.identity);
      newCurve.SetBezierPoint(0, startPoint);
      newCurve.SetBezierPoint(1, startTangent);
      newCurve.SetBezierPoint(2, endTangent);
      newCurve.SetBezierPoint(3, endPoint);
      if (continuation)
      {
         currentCurve.GetComponent<BezierCurveMeshGenerator>().SetUseHead(false);
         newCurve.SetUseTail(false);
      }
      else
         newCurve.SetUseTail(true);
      newCurve.SetUseHead(true);
      currentCurve = newCurve.GetComponent<Curve>();
      
      UpdateMaterial();
      return newCurve;
   }

   void Update()
   {
      if (!editEnabled) return;
      if (Input.GetKeyDown(KeyCode.E))
      {
         if (--currentMaterial < 0) currentMaterial = materials.Length-1;
         Debug.Log(currentMaterial);
         UpdateMaterial();
      }
      else if (Input.GetKeyDown(KeyCode.R))
      {
         currentMaterial = (currentMaterial + 1) % materials.Length;
         UpdateMaterial();
      }
      else if (Input.GetKeyDown(KeyCode.Z))
      {
         RemoveLatestCurve();
      }

   }

   public void RemoveLatestCurve()
   {
      if (currentCurve != null)
         Destroy(currentCurve.gameObject);
      else if (curves.Count > 0)
      {
         Curve lastCurve = curves[curves.Count - 1];
         curves.Remove(lastCurve);
         if (lastCurve != null)
            Destroy(lastCurve.gameObject);
      }
      Debug.Log(curves.Count);
      if (curves.Count > 0)
      {
         curves[curves.Count - 1].GetComponent<BezierCurveMeshGenerator>().SetUseHead(true);
      }
   }

   public void SetCreationEnabled(bool b)
   {
      if (editEnabled == b) return;

      editEnabled = b;

      if (!editEnabled && currentCurve != null)
         RemoveLatestCurve();

     placer.EnableEditing(b);
   }

   public void ClearCurves()
   {
      if (currentCurve != null)
         Destroy(currentCurve.gameObject);
      foreach (Curve c in curves)
      {
         if (c != null && c.gameObject != null)
            Destroy(c.gameObject);
      }
      curves.Clear();
      placer.ClearCurrent();
   }

   public void AddMaterial(CurveMaterial curveMaterial)
   {
      CurveMaterial[] newCurveArray = new CurveMaterial[materials.Length + 1];
      materials.CopyTo(newCurveArray, 0);
      newCurveArray[materials.Length] = curveMaterial;
      materials = newCurveArray;
   }

   void UpdateMaterial()
   {
      if (currentCurve != null)
      {
         currentCurve.SetMaterial(materials[currentMaterial]);
         BezierCurveMeshGenerator generator = currentCurve.GetComponent<BezierCurveMeshGenerator>();
         if (generator.head != null)
            currentCurve.SetPartMaterial(generator.head);
         if (generator.tail != null)
            currentCurve.SetPartMaterial(generator.tail);
      }
      HUDMgr.Instance.ChangeCurrentMaterial(materials[currentMaterial].UITexture);
   }
}

