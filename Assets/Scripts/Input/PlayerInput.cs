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
  private Vector2 aimDirection;

  private bool didPressJump;
  private bool didReleaseJump;
  private bool didPressAttack;
  private bool didPressGrapple;

  void Awake()
  {
    player = ReInput.players.GetPlayer(playerId);
  }

  public void GatherInput()
  {
    Clear();
    horizInput = _GetHorizInput();
    aimDirection = _GetAimDirection();
    didPressJump = _GetDidPressJump();
    didReleaseJump = _GetDidReleaseJump();
    didPressAttack = _GetDidPressAttack();
    didPressGrapple = _GetDidPressGrapple();
  }

  private void Clear()
  {
    horizInput = 0f;
    aimDirection = Vector2.zero;
    didPressJump = false;
    didReleaseJump = false;
    didPressAttack = false;
    didPressGrapple = false;
  }

  #region Getters
  public float GetHorizInput() => horizInput;
  public Vector2 GetAimDirection() => aimDirection;
  public bool GetDidPressJump() => didPressJump;
  public bool GetDidReleaseJump() => didReleaseJump;
  public bool GetDidPressAttack() => didPressAttack;
  public bool GetDidPressGrapple() => didPressGrapple;
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

  private Vector2 _GetAimDirection()
  {
    float horizInput = player.GetAxis("MoveHoriz"); ;
    float verticalInput = player.GetAxis("MoveVertical");
    Vector2 direction = new Vector2(horizInput, verticalInput);
    return direction.normalized;
  }

  private bool _GetDidPressJump() => player.GetButtonDown("Jump");
  private bool _GetDidReleaseJump() => player.GetButtonUp("Jump");
  private bool _GetDidPressAttack() => player.GetButtonDown("Attack");
  private bool _GetDidPressGrapple() => player.GetButtonDown("Grapple");
  #endregion
}