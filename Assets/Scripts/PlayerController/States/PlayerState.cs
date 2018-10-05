using UnityEngine;

public abstract class PlayerState : ScriptableObject
{
  public virtual void OnStateEnter(PlayerController context)
  {

  }

  public virtual void OnStateExit(PlayerController context)
  {

  }

  public abstract PlayerState Tick(PlayerController context);
}
