using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LagueController2D))]
public class PlayerController : MonoBehaviour, CameraFollow.Target
{

  [Header("State")]
  [SerializeField]
  private PlayerState state = PlayerState.IDLE;

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
    float horizInput = playerInput.GetHorizInput();
    LagueController2D.CollisionInfo collisionInfo = controller.GetCollisions();

    switch (state)
    {
      case PlayerState.IDLE:
        Idle();
        break;
      case PlayerState.RUNNING:
        Running();
        break;
      case PlayerState.JUMPING:
        Jumping();
        break;
      case PlayerState.ATTACKING:
        Attacking();
        break;
      case PlayerState.WALL_CLINGING:
        WallClinging();
        break;
    }

    int wallDirX = collisionInfo.left ? -1 : 1;

    float targetVelocityX = horizInput * speed;
    float smoothFactor = collisionInfo.below ? velocityXSmoothFactorGrounded : velocityXSmoothFactorAirborne;
    velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, smoothFactor);

    wallSliding = false;
    if ((collisionInfo.left || collisionInfo.right) && !collisionInfo.below && velocity.y < 0)
    {
      wallSliding = true;
      if (velocity.y < -wallSlideSpeedMax)
      {
        velocity.y = -wallSlideSpeedMax;
      }

      if (timeUntilWallUnstick > 0)
      {
        velocity.x = 0f;
        velocityXSmoothing = 0f;
        if (playerInput.GetHorizInput() != wallDirX && playerInput.GetHorizInput() != 0)
        {
          timeUntilWallUnstick -= Time.deltaTime;
        }
        else
        {
          timeUntilWallUnstick = wallStickTime;
        }
      }
      else
      {
        timeUntilWallUnstick = wallStickTime;
      }
    }

    if (collisionInfo.below || collisionInfo.above)
    {
      velocity.y = 0;
    }

    if (playerInput.GetDidPressJump() || playerInput.GetDidPressAttack())
    {
      if (wallSliding)
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
      if (collisionInfo.below)
      {
        velocity.y = maxJumpVelocity;
      }
    }

    if (playerInput.GetDidPressAttack())
    {

    }

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
  }

  public Bounds GetCameraTrackingBounds()
  {
    return cameraFollowCollider.bounds;
  }

  public int GetLookaheadInfluenceDirection()
  {
    return (int)playerInput.GetHorizInput();
  }

  private void Idle()
  {

  }

  private void Running()
  {

  }

  private void Jumping()
  {
    if (playerInput.GetDidReleaseJump())
    {
      if (velocity.y > minJumpVelocity)
      {
        velocity.y = minJumpVelocity;
      }
    }
  }

  private void Attacking()
  {

  }

  private void WallClinging()
  {

  }

  public enum PlayerState
  {
    IDLE,
    RUNNING,
    JUMPING,
    ATTACKING,
    WALL_CLINGING
  }
}
