using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LagueController2D))]
public class PlayerController : MonoBehaviour, CameraFollow.Target
{

  [Header("State")]
  [SerializeField]
  private PlayerState state;

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

  [HideInInspector]
  public float velocityXSmoothing;
  [SerializeField]
  private float velocityXSmoothFactorGrounded = 0.2f;
  [SerializeField]
  private float velocityXSmoothFactorAirborne = 0.1f;

  private int dirFacing = 1;

  private PlayerInput playerInput;

  [HideInInspector]
  public Vector3 velocity;

  [Header("Wall Jumping")]
  [SerializeField]
  private float wallSlideSpeedMax = 3;
  [SerializeField]
  private float wallStickTime = 0.125f;

  [SerializeField]
  private Vector2 towardWallForce;
  [SerializeField]
  private Vector2 neutralForce;
  [SerializeField]
  private Vector2 awayFromWallForce;

  private BoxCollider2D cameraFollowCollider;
  private LagueController2D controller;
  [HideInInspector]
  public LagueController2D.CollisionInfo collisionInfo;

  [Header("Melee")]
  public Collider2D meleeHitbox;

  [Header("Animation")]
  public Animator animator;

  // Start is called before the first frame update
  void Awake()
  {
    controller = GetComponent<LagueController2D>();
    playerInput = GetComponent<PlayerInput>();
    cameraFollowCollider = GetComponent<BoxCollider2D>();
    animator = GetComponent<Animator>();

    // See Sebastian Lague videos for explanations here.
    gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    maxJumpVelocity = -gravity * timeToJumpApex;

    state = new StateAirborne();
    state.OnStateEnter(this);
  }

  // Update is called once per frame
  void Update()
  {
    playerInput.GatherInput();
    collisionInfo = controller.GetCollisions();
    PlayerState prevState = state;
    state = state.Tick(this);
    if (state != prevState)
    {
      prevState.OnStateExit(this);
      state.OnStateEnter(this);
    }
  }

  public void OnAttackAnimationFinished()
  {
    state.OnAttackAnimationFinished(this);
  }

  public Bounds GetCameraTrackingBounds()
  {
    return cameraFollowCollider.bounds;
  }

  public int GetLookaheadInfluenceDirection()
  {
    return (int)playerInput.GetHorizInput();
  }

  public LagueController2D.CollisionInfo GetCollisionInfo()
  {
    return collisionInfo;
  }

  public PlayerInput GetPlayerInput()
  {
    return playerInput;
  }

  public float GetSpeed()
  {
    return speed;
  }

  public float GetMaxJumpVelocity()
  {
    return maxJumpVelocity;
  }

  public float GetMinJumpVelocity()
  {
    return minJumpVelocity;
  }

  public float GetVelocityXSmoothFactorGrounded()
  {
    return velocityXSmoothFactorGrounded;
  }

  public float GetVelocityXSmoothFactorAirborne()
  {
    return velocityXSmoothFactorAirborne;
  }

  public float GetGravity()
  {
    return gravity;
  }

  public LagueController2D GetController()
  {
    return controller;
  }

  public float GetWallStickTime()
  {
    return wallStickTime;
  }

  public float GetWallSlideSpeedMax()
  {
    return wallSlideSpeedMax;
  }

  public Vector2 GetWallLeapForce()
  {
    return awayFromWallForce;
  }

  public Vector2 GetWallHopForce()
  {
    return neutralForce;
  }

  public Vector2 GetWallClimbForce()
  {
    return towardWallForce;
  }
}
