using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleMeshGenerator : MonoBehaviour {

   int pointFrequency, thickness;
   public float circlePercent;
   float currentCirclePercent;

   // Use this for initialization
   void Start()
   {
      CreateMesh();
   }

   [ContextMenu("Create Mesh")]
   void CreateMesh()
   {
      pointFrequency = 50;
      thickness = 2;
      Vector3 loc = transform.position;
      Mesh newMesh = new Mesh();

      List<Vector3> points = new List<Vector3>();

      float variationX = 0;
      float variationY = 0;
      int numberPoints = Mathf.FloorToInt(pointFrequency * circlePercent)+1;
      for (int i = 0; i < numberPoints; ++i)
      {
         float angle = (float)i / numberPoints * Mathf.PI * 2 * circlePercent;
         variationX = Mathf.Cos(angle);
         variationY = Mathf.Sin(angle);
         for (int j = 0; j < thickness; ++j)
         {
            points.Add(new Vector3(variationX * (j * 0.2f + 1), variationY * (j * 0.2f + 1)));

         }
      }

      List<int> triangles = new List<int>();

      for (int i = 0; i < numberPoints-1; ++i)
      {
         for (int j = 0; j < thickness - 1; ++j)
         {
            triangles.Add(i * thickness + j);
            triangles.Add(i * thickness + j + thickness);
            triangles.Add(i * thickness + j + 1);
            triangles.Add(i * thickness + j + thickness);
            triangles.Add(i * thickness + j + thickness + 1);
            triangles.Add(i * thickness + j + 1);
         }
      }

      newMesh.vertices = points.ToArray();
      newMesh.triangles = triangles.ToArray();
      GetComponent<MeshFilter>().mesh = newMesh;

      PolygonCollider2D col = GetComponent<PolygonCollider2D>();
      if (col == null)
         col = gameObject.AddComponent<PolygonCollider2D>();

      List<Vector2> colPoints = new List<Vector2>();

      AddPoints(0, thickness, numberPoints, points, colPoints);
      AddPoints(numberPoints * thickness - thickness + 1, 1, thickness - 1, points, colPoints);
      AddPoints(numberPoints * thickness - thickness - 1, -thickness, numberPoints - 1, points, colPoints);
      AddPoints(thickness - 2, -1, thickness - 2, points, colPoints);

      col.SetPath(0, colPoints.ToArray());

      transform.position = loc;
   }

   void AddPoints(int start, int increment, int number, List<Vector3> source, List<Vector2> toAdd)
   {
      for (int i = 0; i < number; ++i)
      {
         Debug.Log(start + increment * i);
         toAdd.Add(source[start + increment * i]);
      }
   }

   void Update()
   {

      if (circlePercent != currentCirclePercent)
      {
         currentCirclePercent = circlePercent;
         CreateMesh();
      }
   }
}
