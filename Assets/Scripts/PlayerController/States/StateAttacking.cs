using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttacking : PlayerState
{

  [SerializeField]
  private float duration = 1f;

  [SerializeField]
  private float cooldown = 0.5f;

  [SerializeField]
  private float attackingMoveSpeedFactor = 0.3f;

  private float timeInState = 0f;

  public override void OnStateEnter(PlayerController context)
  {
    context.animator.SetInteger("PlayerState", PlayerState.STATE_ATTACKING_INT);
    context.meleeHitbox.enabled = true;
    timeInState = 0f;
  }

  public override void OnStateExit(PlayerController context)
  {
    context.meleeHitbox.enabled = false;
  }

  public override PlayerState Tick(PlayerController context)
  {
    if (timeInState >= duration)
    {
      return new StateGrounded();
    }

    timeInState += Time.deltaTime;
    Collider2D[] hurtboxes = context.meleeHitbox.GetHurtboxes(context.enemyLayerMask);
    Debug.Log("Got " + hurtboxes.Length + " hurtboxes");
    for (int i = 0; i < hurtboxes.Length; i++) {
      Hurtable hurtable = hurtboxes[i].GetComponent<Hurtable>();
      if (hurtable != null) {
        hurtable.OnHurt(context.meleeHitbox);
        Debug.Log("Found hurtable");
      } else {
        Debug.Log("Did not find hurtable");
      }
    }

    return this;
  }

  public override void OnAttackAnimationFinished(PlayerController context)
  {
    context.meleeHitbox.enabled = false;
  }
}
