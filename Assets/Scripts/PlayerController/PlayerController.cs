using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LagueController2D))]
public class PlayerController : MonoBehaviour, CameraFollow.Target
{

  [Header("State")]
  [SerializeField]
  private PlayerState state = PlayerState.AIRBORNE;

  [Header("Jumping")]
  [SerializeField]
  private float maxJumpHeight = 4;
  [SerializeField]
  private float minJumpHeight = 1;
  [SerializeField]
  private float timeToJumpApex = 0.4f;

  [Header("Movement")]
  [SerializeField]
  private float speed = 5f;

  private float gravity;
  private float maxJumpVelocity;
  private float minJumpVelocity;

  private float velocityXSmoothing;
  [SerializeField]
  private float velocityXSmoothFactorGrounded = 0.2f;
  [SerializeField]
  private float velocityXSmoothFactorAirborne = 0.1f;

  private PlayerInput playerInput;
  private Vector3 velocity;

  [Header("Wall Jumping")]
  [SerializeField]
  private float wallSlideSpeedMax = 3;
  [SerializeField]
  private float wallStickTime = 0.125f;
  private float timeUntilWallUnstick;

  [SerializeField]
  private Vector2 towardWallForce;
  [SerializeField]
  private Vector2 neutralForce;
  [SerializeField]
  private Vector2 awayFromWallForce;

  private bool wallSliding = false;

  private BoxCollider2D cameraFollowCollider;
  private LagueController2D controller;
  private LagueController2D.CollisionInfo collisionInfo;

  // Start is called before the first frame update
  void Awake()
  {
    controller = GetComponent<LagueController2D>();
    playerInput = GetComponent<PlayerInput>();
    cameraFollowCollider = GetComponent<BoxCollider2D>();

    // See Sebastian Lague videos for explanations here.
    gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    maxJumpVelocity = -gravity * timeToJumpApex;
  }

  // Update is called once per frame
  void Update()
  {
    playerInput.GatherInput();
    collisionInfo = controller.GetCollisions();
    // See if we need to change our state before processing input.
    CheckForStateUpdates();

    switch (state)
    {
      case PlayerState.GROUNDED:
        Grounded();
        break;
      case PlayerState.AIRBORNE:
        Airborne();
        break;
      case PlayerState.ATTACKING:
        Attacking();
        break;
      case PlayerState.WALL_CLINGING:
        WallClinging();
        break;
    }
  }

  private void CheckForStateUpdates()
  {
    switch (state)
    {
      case PlayerState.GROUNDED:
        if (!collisionInfo.below)
        {
          state = PlayerState.AIRBORNE;
        }
        break;
      case PlayerState.AIRBORNE:
        if (collisionInfo.below && velocity.y <= 0f)
        {
          state = PlayerState.GROUNDED;
        }
        else if (collisionInfo.left || collisionInfo.right && !collisionInfo.below && velocity.y < 0)
        {
          state = PlayerState.WALL_CLINGING;
          timeUntilWallUnstick = wallStickTime;
        }
        break;
      case PlayerState.WALL_CLINGING:
        if (collisionInfo.below)
        {
          state = PlayerState.GROUNDED;
        }
        else if (!collisionInfo.left && !collisionInfo.right)
        {
          state = PlayerState.AIRBORNE;
        }
        break;
      default:
        break;
    }
  }

  public Bounds GetCameraTrackingBounds()
  {
    return cameraFollowCollider.bounds;
  }

  public int GetLookaheadInfluenceDirection()
  {
    return (int)playerInput.GetHorizInput();
  }

  private void Grounded()
  {
    if (playerInput.GetDidPressJump())
    {
      velocity.y = maxJumpVelocity;
      state = PlayerState.AIRBORNE;
      return;
    }

    float horizInput = playerInput.GetHorizInput();
    float targetVelocityX = horizInput * speed;
    velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, velocityXSmoothFactorGrounded);

    velocity.y = gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
  }

  private void Airborne()
  {
    if (playerInput.GetDidReleaseJump())
    {
      if (velocity.y > minJumpVelocity)
      {
        velocity.y = minJumpVelocity;
      }
    }

    float horizInput = playerInput.GetHorizInput();
    float targetVelocityX = horizInput * speed;
    velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, velocityXSmoothFactorAirborne);

    // Flew into something.
    if (collisionInfo.above)
    {
      velocity.y = 0f;
    }

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
  }

  private void Attacking()
  {

  }

  private void WallClinging()
  {
    int wallDirX = collisionInfo.left ? -1 : 1;
    float horizInput = playerInput.GetHorizInput();

    if (horizInput == wallDirX)
    {
      timeUntilWallUnstick = wallStickTime;
    }
    else if (timeUntilWallUnstick <= 0)
    {
      float targetVelocityX = horizInput * speed;
      velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, velocityXSmoothFactorGrounded);
    }
    else
    {
      timeUntilWallUnstick -= Time.deltaTime;
    }

    if (Mathf.Abs(velocity.y) > wallSlideSpeedMax)
    {
      velocity.y = -wallSlideSpeedMax;
    }

    if (playerInput.GetDidPressJump())
    {
      if (wallDirX == horizInput)
      {
        velocity.x = -wallDirX * towardWallForce.x;
        velocity.y = towardWallForce.y;
      }
      else if (horizInput == 0)
      {
        velocity.x = -wallDirX * neutralForce.x;
        velocity.y = neutralForce.y;
      }
      else
      {
        velocity.x = -wallDirX * awayFromWallForce.x;
        velocity.y = awayFromWallForce.y;
      }
    }

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
  }

  public enum PlayerState
  {
    GROUNDED,
    AIRBORNE,
    ATTACKING,
    WALL_CLINGING
  }
}
