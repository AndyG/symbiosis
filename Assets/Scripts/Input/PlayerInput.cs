using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInput : MonoBehaviour
{

  [SerializeField]
  private int playerId;

  [SerializeField]
  private float horizDeadzone = 0.02f;

  private Player player;

  private float horizInput;

  private bool didPressJump;
  private bool didReleaseJump;
  private bool didPressAttack;

  void Awake()
  {
    player = ReInput.players.GetPlayer(playerId);
  }

  public void GatherInput()
  {
    Clear();
    horizInput = _GetHorizInput();
    didPressJump = _GetDidPressJump();
    didReleaseJump = _GetDidReleaseJump();
    didPressAttack = _GetDidPressAttack();
  }

  private void Clear()
  {
    horizInput = 0f;
    didPressJump = false;
    didReleaseJump = false;
    didPressAttack = false;
  }

  #region Getters
  public float GetHorizInput()
  {
    return horizInput;
  }

  public bool GetDidPressJump()
  {
    return didPressJump;
  }

  public bool GetDidReleaseJump()
  {
    return didReleaseJump;
  }

  public bool GetDidPressAttack()
  {
    return didPressAttack;
  }
  #endregion

  #region Get raw input

  private float _GetHorizInput()
  {
    float h = player.GetAxis("MoveHoriz");
    if (Mathf.Abs(h) < horizDeadzone)
    {
      return 0;
    }
    else
    {
      return Mathf.Sign(h);
    }
  }

  private bool _GetDidPressJump()
  {
    return player.GetButtonDown("Jump");
  }

  private bool _GetDidReleaseJump()
  {
    return player.GetButtonUp("Jump");
  }

  private bool _GetDidPressAttack()
  {
    return player.GetButtonDown("Attack");
  }
  #endregion
}