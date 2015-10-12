using UnityEngine;
using System.Collections;

public class BoofyCamera : MonoBehaviour {

   float editCameraSize;

   public Camera editCam;
   public Camera boofyCam;

   enum CameraMode
   {
      kEdit,
      kSimulation
   }
   CameraMode cameraMode;

   public void SetEditCamSize(float size)
   {
      editCameraSize = size;
      if (cameraMode == CameraMode.kEdit)
      {
         editCam.orthographicSize = editCameraSize;
      }
   }

   public void UseEditCamera()
   {
      editCam.gameObject.SetActive(true);
      boofyCam.gameObject.SetActive(false);
      editCam.orthographicSize = editCameraSize;
      cameraMode = CameraMode.kEdit;
   }

   public void UseBoofyCamera()
   {
      editCam.gameObject.SetActive(false);
      boofyCam.gameObject.SetActive(true);
      cameraMode = CameraMode.kSimulation;
   }

   public void EnableFollowCam(bool b)
   {
      boofyCam.GetComponent<CameraFollow>().enabled = b;
   }
}
