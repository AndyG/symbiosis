// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class StateShootingGrapple : PlayerState
// {

//   private LineRenderer
//   private float grappleDistance = 0f;

//   public override void OnStateEnter(PlayerController context)
//   {
//   }

//   public override PlayerState Tick(PlayerController context)
//   {
//     RaycastHit2D hit = Physics2D.Raycast(context.transform.position, Vector3.forward);
//     if (hit) {
//       lineRenderer.SetPosition(0, this.transform.position);
//       lineRenderer.SetPosition(1, hit.point);
//       ConfigureEndingHit(hit);
//     } else {
//       lineRenderer.SetPosition(0, this.transform.position);
//       lineRenderer.SetPosition(1, this.transform.position + transform.up * 1000);
//       ConfigueEndingMiss();
//     }
//     return this;
//   }
// }
