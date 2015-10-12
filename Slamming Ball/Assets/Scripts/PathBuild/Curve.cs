using UnityEngine;
using System.Collections;

public class Curve : MonoBehaviour {

   CurveMaterial material;

   public void SetMaterial(CurveMaterial mat)
   {
      material = mat;
      GetComponent<MeshRenderer>().material.mainTexture = material.texture;
   }

   public float GetBallSpeed()
   {
      return material.ball_speed;
   }
}
