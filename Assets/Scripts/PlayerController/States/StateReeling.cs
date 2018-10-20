using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReeling : PlayerState
{

  public override void OnStateEnter(PlayerController context)
  {

  }

  public override PlayerState Tick(PlayerController context)
  {
    GrappleHook grappleHook = context.GetGrappleHook();
    GrappleHook.State grappleHookState = grappleHook.GetState;

    Vector2 grappleEndPos = grappleHook.GetEndingPosition();
    Vector2 curPosition = context.transform.position;
    LagueController2D.CollisionInfo collisionInfo = context.collisionInfo;
    float directionX = Mathf.Sign(grappleEndPos.x - curPosition.x);
    if (directionX == 1 && collisionInfo.right || directionX == -1 && collisionInfo.left) {
        grappleHook.RetractInstant();
        return context.GetDefaultState();
    }

    float directionY = Mathf.Sign(grappleEndPos.y - curPosition.y);
    if (directionY == 1 && collisionInfo.above || directionY == -1 && collisionInfo.below) {
        grappleHook.RetractInstant();
        return context.GetDefaultState();
    }

    float targetVelocity = context.GetReelSpeed();
    Vector2 direction = (grappleEndPos - curPosition) * targetVelocity;
    // context.velocity.x = Mathf.SmoothDamp(context.velocity.x, targetVelocityX, ref context.velocityXSmoothing, context.GetVelocityXSmoothFactorGrounded());
    // context.velocity.y = context.GetGravity() * Time.deltaTime;
    context.velocity.x = direction.x;
    context.velocity.y = direction.y;

    context.FaceVelocityX();
    context.GetController().Move(context.velocity * Time.deltaTime);

    return this;
  }
}
