using UnityEngine;
using System.Collections;

public class DrawGrid : MonoBehaviour {

   public float gridSize;
   public Color color = Color.white;


   void OnDrawGizmos()
   {
      Gizmos.color = color;

      Vector2 bottomLeft = Camera.current.ViewportToWorldPoint(Vector2.zero);
      Vector2 topRight = Camera.current.ViewportToWorldPoint(new Vector2(1, 1));

      int numberLines = Mathf.CeilToInt((topRight.x - bottomLeft.x) / gridSize);
      int linesOver = Mathf.FloorToInt(bottomLeft.x / gridSize);


      for (int i = linesOver; i < linesOver+numberLines+1; ++i)
      {
         float xloc = i * gridSize;
         Vector3 from = new Vector3(xloc, topRight.y, 1);
         Vector3 to = new Vector3(xloc, bottomLeft.y, 1);

         Gizmos.DrawLine(from, to);
      }

      numberLines = Mathf.CeilToInt((topRight.x - bottomLeft.x) / gridSize);
      linesOver = Mathf.FloorToInt(bottomLeft.y / gridSize);

      for (int i = linesOver; i < linesOver+numberLines+1; ++i)
      {
         float yloc = i * gridSize;
         Vector3 from = new Vector3(topRight.x, yloc, 1);
         Vector3 to = new Vector3(bottomLeft.x, yloc, 1);

         Gizmos.DrawLine(from, to);
      }
   }
}
