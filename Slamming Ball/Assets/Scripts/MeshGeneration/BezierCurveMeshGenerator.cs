using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierCurveMeshGenerator : MonoBehaviour
{

   public Vector2 a, b, c, d;

   public int pointFrequency = 10;
   public int thickness = 2;

   public float initialU;

   public float defaultUVLength = 1;

   void Start()
   {
      CreateMesh();
   }

   // Update is called once per frame
   void Update()
   {

   }

   public Vector2[] GetBezierPoints()
   {
      return new Vector2[] { transform.TransformPoint(a), 
                             transform.TransformPoint(b), 
                             transform.TransformPoint(c), 
                             transform.TransformPoint(d) };
   }

   public void SetBezierPoint(int point, Vector3 loc)
   {
      switch (point)
      {
         case 0: a = transform.InverseTransformPoint(loc.x, loc.y, 0); break;
         case 1: b = transform.InverseTransformPoint(loc.x, loc.y, 0); break;
         case 2: c = transform.InverseTransformPoint(loc.x, loc.y, 0); break;
         case 3: d = transform.InverseTransformPoint(loc.x, loc.y, 0); break;
      }
   }

   public void UpdateMesh()
   {
      int numberPoints = Mathf.FloorToInt(pointFrequency) + 1;
      List<Vector2> normals;
      List<Vector2> bezierCurve = GenerateBezierPoints(numberPoints, out normals);
      List<Vector2> UVs;
      List<Vector3> meshNormals;
      Bounds meshBounds;
      List<Vector3> points = GenerateVertices(numberPoints, bezierCurve, normals, out UVs, out meshNormals, out meshBounds);
      MeshFilter meshFilter = GetComponent<MeshFilter>();
      meshFilter.mesh.vertices = points.ToArray();
      meshFilter.mesh.uv = UVs.ToArray();
      meshFilter.mesh.bounds = meshBounds;

      UpdateCollider(numberPoints, points);
   }

   List<Vector3> GenerateVertices(int numberPoints, List<Vector2> bezierCurve, List<Vector2> normals, 
                                  out List<Vector2> UVs, out List<Vector3> meshNormals, out Bounds meshBounds)
   {
      UVs = new List<Vector2>();
      meshNormals = new List<Vector3>();
      List<Vector3> points = new List<Vector3>();

      float maxX = 0, minX = 0, maxY = 0, minY = 0;

      for (int i = 0; i < numberPoints; ++i)
      {
         Vector3 curvePoint = new Vector3(bezierCurve[i].x, bezierCurve[i].y);
         Vector3 curveNormal = new Vector3(normals[i].x, normals[i].y);
         float u = i == 0 ? initialU : (bezierCurve[i-1] - bezierCurve[i]).magnitude/defaultUVLength + UVs[(i - 1) * thickness].x;
         for (int j = 0; j < thickness; ++j)
         {
            Vector3 point = curvePoint + j * curveNormal * 0.2f;
            points.Add(point);
            meshNormals.Add(Vector3.back);
            UVs.Add(new Vector2(u, 1.0f - (float)j / (thickness - 1)));
            if (point.x > maxX)
               maxX = point.x;
            if (point.x < minX)
               minX = point.x;
            if (point.y > maxY)
               maxY = point.y;
            if (point.y < minY)
               minY = point.y;
         }
      }

      meshBounds = new Bounds(new Vector3(maxX + minX, maxY + minY, 0) / 2, new Vector3(maxX - minX, maxY - minY, 0));


      return points;
   }

   [ContextMenu("Create Mesh")]
   void CreateMesh() 
   {
      Vector3 loc = transform.position;
      Mesh newMesh = new Mesh();

      int numberPoints = Mathf.FloorToInt(pointFrequency) + 1;
      List<Vector2> normals;
      List<Vector2> bezierCurve = GenerateBezierPoints(numberPoints, out normals);
      List<Vector2> UVs;
      List<Vector3> meshNormals;
      Bounds meshBounds;
      List<Vector3> points = GenerateVertices(numberPoints, bezierCurve, normals, out UVs, out meshNormals, out meshBounds);

      List<int> triangles = new List<int>();

      for (int i = 0; i < numberPoints - 1; ++i)
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
      newMesh.normals = meshNormals.ToArray();
      newMesh.uv = UVs.ToArray();
      newMesh.bounds = meshBounds;
      GetComponent<MeshFilter>().mesh = newMesh;

      UpdateCollider(numberPoints, points);

      transform.position = loc;
   }

   void UpdateCollider(int numberPoints, List<Vector3> points)
   {
      PolygonCollider2D col = GetComponent<PolygonCollider2D>();
      if (col == null)
         col = gameObject.AddComponent<PolygonCollider2D>();

      List<Vector2> colPoints = new List<Vector2>();

      AddPoints(0, thickness, numberPoints, points, colPoints);
      AddPoints(numberPoints * thickness - thickness + 1, 1, thickness - 1, points, colPoints);
      AddPoints(numberPoints * thickness - thickness - 1, -thickness, numberPoints - 1, points, colPoints);
      AddPoints(thickness - 2, -1, thickness - 2, points, colPoints);

      col.SetPath(0, colPoints.ToArray());
   }

   void AddPoints(int start, int increment, int number, List<Vector3> source, List<Vector2> toAdd)
   {
      for (int i = 0; i < number; ++i)
      {
         toAdd.Add(source[start + increment * i]);
      }
   }

   List<Vector2> GenerateBezierPoints(int numberPoints, out List<Vector2> normals)
   {
      List<Vector2> points = new List<Vector2>();
      normals = new List<Vector2>();

      for (int i = 0; i < numberPoints; ++i)
      {
         float t = (float)i / (numberPoints-1);
         Vector2 curvePoint = a * Mathf.Pow((1 - t), 3)
                            + b * 3 * t * Mathf.Pow((1 - t), 2)
                            + c * 3 * Mathf.Pow(t, 2) * (1 - t)
                            + d * Mathf.Pow(t, 3);
         Vector2 tangent = a * -3 * Mathf.Pow(1-t, 2)
                         + b *  3 * (Mathf.Pow(1-t, 2) - 2 * t * (1 - t))
                         + c *  3 * (2 * t * (1-t) - Mathf.Pow(t, 2))
                         + d *  3 * t * t;
         tangent.Normalize();
         Vector2 normal = new Vector2(tangent.y, -tangent.x);
         points.Add(curvePoint);
         normals.Add(normal);
      }

      return points;
   }
}
