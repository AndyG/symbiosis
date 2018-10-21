using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGrounded : PlayerState
{

  public override void OnStateEnter(PlayerController context)
  {
    context.animator.SetInteger("PlayerState", PlayerState.STATE_GROUNDED_INT);
  }

  public override PlayerState Tick(PlayerController context)
  {
    LagueController2D.CollisionInfo collisionInfo = context.GetCollisionInfo();
    PlayerInput playerInput = context.GetPlayerInput();

    if (!collisionInfo.below)
    {
      return new StateAirborne();
    }

    if (playerInput.GetDidPressJump())
    {
      context.velocity.y = context.GetMaxJumpVelocity();
      return new StateAirborne();
    }

    if (playerInput.GetDidPressAttack())
    {
      return new StateAttacking();
    }

    PlayerInput input = context.GetPlayerInput();
    if (input.GetDidPressGrapple() && input.GetAimDirection().y > 0) {
      context.GetGrappleHook().SetDirection(input.GetAimDirection());
      return new StateGrappling();
    }

    float horizInput = playerInput.GetHorizInput();
    float targetVelocityX = horizInput * context.GetSpeed();
    context.velocity.x = Mathf.SmoothDamp(context.velocity.x, targetVelocityX, ref context.velocityXSmoothing, context.GetVelocityXSmoothFactorGrounded());
    context.velocity.y = context.GetGravity() * Time.deltaTime;

    context.FaceVelocityX();
    context.GetController().Move(context.velocity * Time.deltaTime);

    return this;
  }
}
