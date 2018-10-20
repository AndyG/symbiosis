using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class StateAttacking : PlayerState
{

  private HashSet<Hurtable> hitHurtables = new HashSet<Hurtable>();

  [SerializeField]
  private float duration = 0.4f;

  [SerializeField]
  private float attackingMoveSpeedFactor = 0.6f;

  private float timeInState = 0f;

  public override void OnStateEnter(PlayerController context)
  {
    hitHurtables.Clear();
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
    for (int i = 0; i < hurtboxes.Length; i++) {
      Hurtable hurtable = hurtboxes[i].GetComponent<Hurtable>();
      if (hurtable != null && !hitHurtables.Contains(hurtable)) {
        hurtable.OnHurt(context.meleeHitbox);
        ShakeCamera();
        context.Hitstop(0.2f);
        // hitHurtables.Add(hurtable);
      }
    }

    float horizInput = context.GetPlayerInput().GetHorizInput();
    float targetVelocityX = horizInput * context.GetSpeed() * attackingMoveSpeedFactor;
    context.velocity.x = Mathf.SmoothDamp(context.velocity.x, targetVelocityX, ref context.velocityXSmoothing, context.GetVelocityXSmoothFactorGrounded());
    context.velocity.y = context.GetGravity() * Time.deltaTime;

    context.FaceVelocityX();
    context.GetController().Move(context.velocity * Time.deltaTime);

    return this;
  }

  private void ShakeCamera() {
    float magn = 1f, rough = 10f, fadeIn = 0.1f, fadeOut = 0.2f;
    CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
  }

  public override void OnAttackAnimationFinished(PlayerController context)
  {
    context.meleeHitbox.enabled = false;
  }
}
