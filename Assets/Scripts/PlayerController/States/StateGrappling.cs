using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGrappling : PlayerState
{

  private bool hasCompleted = false;

  public override void OnStateEnter(PlayerController context)
  {
    hasCompleted = false;
  }

  public override PlayerState Tick(PlayerController context)
  {
    GrappleHook grappleHook = context.GetGrappleHook();
    GrappleHook.State grappleHookState = grappleHook.GetState;
    if (grappleHookState == GrappleHook.State.NONE) {
      if (!hasCompleted) {
        // Just entered the state.
        grappleHook.Extend();
      } else {
        return context.GetDefaultState();
      }
    }

    if (grappleHookState == GrappleHook.State.EXTENDING) {
      if (context.GetPlayerInput().GetDidPressGrapple()) {
        hasCompleted = true;
        grappleHook.Retract();
      }
    }

    if (grappleHookState == GrappleHook.State.HITTING) {
      hasCompleted = true;
      if (context.GetPlayerInput().GetDidPressGrapple()) {
        grappleHook.Retract();
      }

      if (context.GetPlayerInput().GetDidPressJump()) {
        return new StateReeling();
      }
    }

    return this;
  }
}
