// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DepthManager : MonoBehaviour
// {

//   public delegate void OnCameraDepthChanged(int depth);
//   public OnCameraDepthChanged OnCameraDepthChangedEvent;

//   [SerializeField]
//   [Range(0, 2)]
//   private int cameraDepth = 0;

//   // Use this for initialization
//   void Start()
//   {

//   }

//   // Update is called once per frame
//   void Update()
//   {

//   }

//   void OnValidate()
//   {
//     if (OnCameraDepthChangedEvent != null)
//     {
//       OnCameraDepthChangedEvent(this.cameraDepth);
//     }
//   }

//   public void SetCameraDepth(int depth)
//   {
//     this.cameraDepth = depth;
//     OnCameraDepthChangedEvent(depth);
//   }

//   public int GetCameraDepth()
//   {
//     return this.cameraDepth;
//   }
// }
