using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttacking : State<PlayerController>
{
  public void OnStateEnter(PlayerController context)
  {

  }
  public void OnStateExit(PlayerController context)
  {

  }

  public State<PlayerController> Tick(PlayerController context)
  {
    return this;
  }
}
