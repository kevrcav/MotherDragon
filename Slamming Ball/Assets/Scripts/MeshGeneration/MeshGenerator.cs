using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour {

   int x, y;

	// Use this for initialization
	void Start () {
      
	}

   [ContextMenu("Generate Mesh")]
   void GenerateMesh()
   {
      x = 100;
      y = 3;
      Vector3 loc = transform.position;
      Mesh newMesh = new Mesh();

      List<Vector3> points = new List<Vector3>();

      float variation = 0;
      for (int i = 0; i < x; ++i)
      {
         variation = Mathf.Pow(((float)(i % 10) - 5), 2) / 12.5f;
         Debug.Log(variation);
         for (int j = 0; j < y; ++j)
         {
            points.Add(new Vector3(i * 0.5f - x / 4.0f, y / 2.0f - j * 0.5f + variation));

         }
      }

      List<int> triangles = new List<int>();

      for (int i = 0; i < x - 1; ++i)
      {
         for (int j = 0; j < y - 1; ++j)
         {
            triangles.Add(i * y + j);
            triangles.Add(i * y + j + y);
            triangles.Add(i * y + j + 1);
            triangles.Add(i * y + j + y);
            triangles.Add(i * y + j + y + 1);
            triangles.Add(i * y + j + 1);
         }
      }

      newMesh.vertices = points.ToArray();
      newMesh.triangles = triangles.ToArray();
      GetComponent<MeshFilter>().mesh = newMesh;

      PolygonCollider2D col = gameObject.AddComponent<PolygonCollider2D>();

      List<Vector2> colPoints = new List<Vector2>();

      AddPoints(0, y, x, points, colPoints);
      AddPoints(x * y - y + 1, 1, y - 1, points, colPoints);
      AddPoints(x * y - y - 1, -y, x - 1, points, colPoints);
      AddPoints(y - 2, -1, y - 2, points, colPoints);

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

   // Update is called once per frame
   void Update()
   {
	   
	}
}
