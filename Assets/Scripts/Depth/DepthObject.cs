// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DepthObject : MonoBehaviour
// {

//   private DepthManager depthManager;
//   private SpriteRenderer spriteRenderer;

//   [SerializeField]
//   private int depth = 0;

//   // Use this for initialization
//   void Start()
//   {
//     this.spriteRenderer = GetComponent<SpriteRenderer>();
//     this.depthManager = FindObjectOfType<DepthManager>();
//     this.depthManager.OnCameraDepthChangedEvent += this.OnCameraDepthChanged;
//     UpdateDepth();
//   }

//   public void SetDepth(int depth)
//   {
//     this.depth = depth;
//   }

//   private void OnCameraDepthChanged(int cameraDepth)
//   {
//     UpdateDepth();
//   }

//   private void UpdateDepth()
//   {
//     this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, depth);
//     if (this.depth == this.depthManager.GetCameraDepth())
//     {
//       this.spriteRenderer.enabled = true;
//     }
//     else
//     {
//       this.spriteRenderer.enabled = false;
//     }
//   }
// }
