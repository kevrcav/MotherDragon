using UnityEngine;
using System.Collections;

public class CurveMaterialAdder : MonoBehaviour {

   [SerializeField]
   public CurveMaterial material;

   void Start()
   {
      CurveMgr.Instance.AddMaterial(material);
   }
}
