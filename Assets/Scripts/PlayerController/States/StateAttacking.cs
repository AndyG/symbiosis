using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateAttacking", menuName = "PlayerState/StateAttacking")]
public class StateAttacking : PlayerState
{

  [SerializeField]
  private float cooldown = 0.5f;

  public override PlayerState Tick(PlayerController context)
  {
    return this;
  }
}
