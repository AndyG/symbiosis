using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

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

    float directionY = Mathf.Sign(grappleEndPos.y - curPosition.y);
    if (directionY == 1 && collisionInfo.above || directionY == -1 && collisionInfo.below) {
        EndGrapple(context);
        return context.GetDefaultState();
    }

    if (directionX == 1 && collisionInfo.right || directionX == -1 && collisionInfo.left) {
        EndGrapple(context);
        return new StateWallCling();
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

  private void EndGrapple(PlayerController context) {
    ShakeCamera();
    context.Hitstop(0.3f);
    context.GetGrappleHook().RetractInstant();
  }

  private void ShakeCamera() {
    float magn = 3f, rough = 10f, fadeIn = 0.1f, fadeOut = 0.2f;
    CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
  }
}
