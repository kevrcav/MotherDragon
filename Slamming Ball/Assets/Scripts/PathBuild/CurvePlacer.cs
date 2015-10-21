using UnityEngine;
using System.Collections;

public class CurvePlacer : MonoBehaviour {

   Vector3 mousePosition;
   public BezierCurveMeshGenerator currentCurve;

   enum PlaceState
   {
      kPoint,
      kStartTangent,
      kEndTangent,
      kNone, 
      kDisabled
   }
   PlaceState placeState;

   Plane mousePlane;

   bool newLine;

	// Use this for initialization
	void Start () {
      mousePlane = new Plane(Vector3.forward, new Vector3(0, 0, 0));
      placeState = PlaceState.kNone;
	}
	
	// Update is called once per frame
	void Update () {
      if (placeState == PlaceState.kDisabled) return;


      if (mousePosition != Input.mousePosition)
      {
         if (currentCurve == null && !Input.GetMouseButtonDown(0))
         {
            placeState = PlaceState.kNone;
            return;
         }
         mousePosition = Input.mousePosition;
         Vector3 inWorldMousePos = GetInWorldMousePos();

         switch (placeState)
         {
            case PlaceState.kPoint:
               currentCurve.SetBezierPoint(3, inWorldMousePos);
               currentCurve.SetBezierPoint(2, inWorldMousePos);
               break;
            case PlaceState.kStartTangent:
               currentCurve.SetBezierPoint(1, inWorldMousePos);
               break;
            case PlaceState.kEndTangent:
               currentCurve.SetBezierPoint(2, inWorldMousePos);
               break;
         }

         if (placeState != PlaceState.kNone && placeState != PlaceState.kDisabled)
            currentCurve.UpdateMesh();
      }

      if (Input.GetMouseButtonDown(0))
      {
         switch (placeState)
         {
            case PlaceState.kEndTangent:
               Vector2[] curveInfo = currentCurve.GetBezierPoints();
               Vector2 newPos = curveInfo[3];
               Vector2 normalTangent = (newPos - curveInfo[2]).normalized;
               Vector2 newTangent = newPos + normalTangent;
               Vector2[] uvs = currentCurve.GetComponent<MeshFilter>().mesh.uv;
               float lastU = uvs[uvs.Length-1].x;
               currentCurve = CurveMgr.Instance.NewCurve(newPos, newPos, newTangent, newPos, newPos, true);
               currentCurve.initialU = lastU;
               placeState = PlaceState.kPoint;
               newLine = false;
               break;
            case PlaceState.kPoint:
               placeState = newLine ? PlaceState.kStartTangent : PlaceState.kEndTangent;
               break;
            case PlaceState.kStartTangent:
               placeState = PlaceState.kEndTangent;
               break;
            case PlaceState.kNone:
               Vector3 inWorldMousePos = GetInWorldMousePos();
               currentCurve = CurveMgr.Instance.NewCurve(inWorldMousePos, inWorldMousePos, 
                                                         inWorldMousePos, inWorldMousePos, inWorldMousePos, false);
               placeState = PlaceState.kPoint;
               newLine = true;
               break;

         }
      }
      else if (Input.GetMouseButtonDown(1) && placeState != PlaceState.kNone)
      {
         CurveMgr.Instance.RemoveLatestCurve();
         placeState = PlaceState.kNone;
      }
	}

   Vector3 GetInWorldMousePos()
   {
      //Ray ray = Camera.main.ScreenPointToRay(mousePosition);
      //float distance;
      //mousePlane.Raycast(ray, out distance);
      //return ray.GetPoint(distance)
      Vector3 inWorldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
      inWorldMousePos.z = 0;
      return inWorldMousePos;
   }

   public void EnableEditing(bool b)
   {
      if (!b)
         placeState = PlaceState.kDisabled;
      else if (placeState == PlaceState.kDisabled)
         placeState = PlaceState.kNone;
      else
         Debug.Log("what do you want from me!");
   }

   public void ClearCurrent()
   {
      currentCurve = null;
      placeState = PlaceState.kNone;
   }
}
