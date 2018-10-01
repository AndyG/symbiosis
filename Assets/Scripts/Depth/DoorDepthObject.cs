// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DoorDepthObject : MonoBehaviour
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
//       this.spriteRenderer.color = CopyWithAlpha(this.spriteRenderer.color, 1f);
//       this.spriteRenderer.sortingLayerName = "Default";
//     }
//     else if (this.depth == this.depthManager.GetCameraDepth() - 1)
//     {
//       this.spriteRenderer.sortingLayerName = "DoorForeground";
//       this.spriteRenderer.enabled = true;
//       this.spriteRenderer.color = CopyWithAlpha(this.spriteRenderer.color, 0.3f);
//     }
//     else
//     {
//       this.spriteRenderer.enabled = false;
//     }
//   }

//   private Color CopyWithAlpha(Color color, float alpha)
//   {
//     return new Color(color.r, color.g, color.b, alpha);
//   }
// }
