using UnityEngine;

public abstract class PlayerState
{
  public virtual void OnStateEnter(PlayerController context)
  {

  }

  public virtual void OnStateExit(PlayerController context)
  {

  }

  public virtual void OnAttackAnimationFinished(PlayerController context)
  {

  }

  public abstract PlayerState Tick(PlayerController context);

  public const int STATE_GROUNDED_INT = 0;
  public const int STATE_AIRBORNE_INT = 1;
  public const int STATE_WALLCLING_INT = 2;
  public const int STATE_ATTACKING_INT = 3;
}
