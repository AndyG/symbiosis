using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAirborne : PlayerState
{

  public override void OnStateEnter(PlayerController context)
  {
    context.animator.SetInteger("PlayerState", PlayerState.STATE_AIRBORNE_INT);
  }

  public override PlayerState Tick(PlayerController context)
  {
    if (context.collisionInfo.below && context.velocity.y <= 0f)
    {
      return new StateGrounded();
    }
    else if ((context.collisionInfo.left || context.collisionInfo.right)
    && (!context.collisionInfo.below && context.velocity.y < 0))
    {
      return new StateWallCling();
    }

    if (context.GetPlayerInput().GetDidPressGrapple()) {
      return new StateGrappling();
    }

    if (context.GetPlayerInput().GetDidReleaseJump())
    {
      if (context.velocity.y > context.GetMinJumpVelocity())
      {
        context.velocity.y = context.GetMinJumpVelocity();
      }
    }

    float horizInput = context.GetPlayerInput().GetHorizInput();
    float targetVelocityX = horizInput * context.GetSpeed();
    context.velocity.x = Mathf.SmoothDamp(context.velocity.x, targetVelocityX, ref context.velocityXSmoothing, context.GetVelocityXSmoothFactorAirborne());

    // Flew into something.
    if (context.collisionInfo.above)
    {
      context.velocity.y = 0f;
    }

    context.velocity.y += context.GetGravity() * Time.deltaTime;
    context.GetController().Move(context.velocity * Time.deltaTime);
    context.FaceVelocityX();

    return this;
  }
}
