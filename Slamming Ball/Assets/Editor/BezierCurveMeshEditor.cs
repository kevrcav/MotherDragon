using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BezierCurveMeshGenerator))]
public class BezierCurveMeshEditor : Editor {

   BezierCurveMeshGenerator generator;
	// Use this for initialization
	void Start () {
      generator = (BezierCurveMeshGenerator)target;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   void OnSceneGUI()
   {
      if (generator == null)
         generator = (BezierCurveMeshGenerator)target;
      Vector2[] points = generator.GetBezierPoints();
      for (int i = 0; i < 4; ++i)
      {
         generator.SetBezierPoint(i, Handles.PositionHandle(points[i], Quaternion.identity));
      }
      Handles.DrawLine(points[0], points[1]);
      Handles.DrawLine(points[2], points[3]);
      Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.white, null, 1);
   }
}
