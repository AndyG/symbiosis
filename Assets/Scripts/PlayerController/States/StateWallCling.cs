using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateWallCling", menuName = "PlayerState/StateWallCling")]
public class StateWallCling : PlayerState
{

  private float timeUntilWallUnstick = 0f;

  public override void OnStateEnter(PlayerController context)
  {
    timeUntilWallUnstick = context.GetWallStickTime();
  }

  public override PlayerState Tick(PlayerController context)
  {
    if (context.collisionInfo.below)
    {
      return ScriptableObject.CreateInstance<StateGrounded>();
    }
    else if (!context.collisionInfo.left && !context.collisionInfo.right)
    {
      return ScriptableObject.CreateInstance<StateAirborne>();
    }

    int wallDirX = context.collisionInfo.left ? -1 : 1;
    float horizInput = context.GetPlayerInput().GetHorizInput();

    if (horizInput == wallDirX)
    {
      timeUntilWallUnstick = context.GetWallStickTime();
    }
    else if (timeUntilWallUnstick <= 0)
    {
      float targetVelocityX = horizInput * context.GetSpeed();
      context.velocity.x = Mathf.SmoothDamp(context.velocity.x, targetVelocityX, ref context.velocityXSmoothing, context.GetVelocityXSmoothFactorGrounded());
    }
    else
    {
      timeUntilWallUnstick -= Time.deltaTime;
    }

    if (Mathf.Abs(context.velocity.y) > context.GetWallSlideSpeedMax())
    {
      context.velocity.y = -context.GetWallSlideSpeedMax();
    }

    if (context.GetPlayerInput().GetDidPressJump())
    {
      Vector2 wallJumpForce;
      if (wallDirX == horizInput)
      {
        wallJumpForce = context.GetWallClimbForce();
      }
      else if (horizInput == 0)
      {
        wallJumpForce = context.GetWallHopForce();
      }
      else
      {
        wallJumpForce = context.GetWallLeapForce();
      }

      context.velocity.x = -wallDirX * wallJumpForce.x;
      context.velocity.y = wallJumpForce.y;
    }

    context.velocity.y += context.GetGravity() * Time.deltaTime;
    context.GetController().Move(context.velocity * Time.deltaTime);

    return this;
  }
}
