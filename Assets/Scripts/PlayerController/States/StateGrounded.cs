using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGrounded : PlayerState
{
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

    float horizInput = playerInput.GetHorizInput();
    float targetVelocityX = horizInput * context.GetSpeed();
    context.velocity.x = Mathf.SmoothDamp(context.velocity.x, targetVelocityX, ref context.velocityXSmoothing, context.GetVelocityXSmoothFactorGrounded());

    context.velocity.y = context.GetGravity() * Time.deltaTime;
    context.GetController().Move(context.velocity * Time.deltaTime);

    return this;
  }
}
