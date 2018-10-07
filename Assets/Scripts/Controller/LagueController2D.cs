using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagueController2D : RaycastController
{
  [SerializeField]
  private float maxClimbAngle = 80;
  [SerializeField]
  private float maxDescendAngle = 80;

  [Header("Debug")]
  [SerializeField]
  private bool drawDebugRays;

  private BoxCollider2D collider;
  private CollisionInfo collisionInfo;

  public override void Start()
  {
    base.Start();
  }

  public void Move(Vector3 moveDistance, bool isStandingOnPlatform = false)
  {
    UpdateRaycastOrigins();
    collisionInfo.Reset();
    collisionInfo.velocityOld = moveDistance;

    if (moveDistance.y < 0)
    {
      DescendSlope(ref moveDistance);
    }

    HorizontalCollisions(ref moveDistance);

    if (moveDistance.y != 0)
    {
      VerticalCollisions(ref moveDistance);
    }

    if (isStandingOnPlatform)
    {
      collisionInfo.below = true;
    }

    CheckForSlopeChange(ref moveDistance);
    transform.Translate(moveDistance, Space.World);
  }

  public CollisionInfo GetCollisions()
  {
    return collisionInfo;
  }

  private void HorizontalCollisions(ref Vector3 moveDistance)
  {
    float directionX = (int)Mathf.Sign(moveDistance.x);
    float rayLength = Mathf.Abs(moveDistance.x) + skinWidth;

    if (Mathf.Abs(moveDistance.x) < skinWidth)
    {
      rayLength = 2 * skinWidth;
    }

    for (int i = 0; i < horizontalRayCount; i++)
    {
      Vector2 baseRayOrigin = directionX == -1f
      ? raycastOrigins.bottomLeft
      : raycastOrigins.bottomRight;

      float rayOriginOffset = horizontalRaySpacing * i;
      Vector2 rayOrigin = baseRayOrigin + Vector2.up * rayOriginOffset;
      Vector2 rayDirection = Vector2.right * directionX;
      if (drawDebugRays)
      {
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
      }
      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);

      if (hit.distance == 0)
      {
        // Inside the collider, let's just ignore...
        continue;
      }

      if (hit)
      {
        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

        if (i == 0 && slopeAngle <= maxClimbAngle)
        {
          float distanceToSlopeStart = 0f;
          if (slopeAngle != collisionInfo.slopeAngleOld)
          {
            distanceToSlopeStart = hit.distance - skinWidth;
            moveDistance.x -= distanceToSlopeStart * directionX;
          }
          ClimbSlope(ref moveDistance, slopeAngle);
          moveDistance.x += distanceToSlopeStart * directionX;
        }

        if (!collisionInfo.climbingSlope || slopeAngle > maxClimbAngle)
        {
          moveDistance.x = (hit.distance - skinWidth) * directionX;
          rayLength = hit.distance;

          if (collisionInfo.climbingSlope)
          {
            moveDistance.y = Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveDistance.x);
          }

          collisionInfo.left = directionX == -1;
          collisionInfo.right = directionX == 1;
        }
      }
    }
  }

  private void VerticalCollisions(ref Vector3 moveDistance)
  {
    float directionY = Mathf.Sign(moveDistance.y);
    float rayLength = Mathf.Abs(moveDistance.y) + skinWidth;
    for (int i = 0; i < verticalRayCount; i++)
    {
      Vector2 baseRayOrigin = directionY == -1f
      ? raycastOrigins.bottomLeft
      : raycastOrigins.topLeft;

      // Add move distance X since vertical collisions happen after horizontal collisions.
      float rayOriginOffset = verticalRaySpacing * i + moveDistance.x;
      Vector2 rayOrigin = baseRayOrigin + Vector2.right * rayOriginOffset;
      Vector2 rayDirection = Vector2.up * directionY;
      if (drawDebugRays)
      {
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
      }
      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, layerMask);

      if (hit)
      {
        moveDistance.y = (hit.distance - skinWidth) * directionY;
        rayLength = hit.distance;

        if (collisionInfo.climbingSlope)
        {
          moveDistance.x = moveDistance.y / Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveDistance.x);
        }

        collisionInfo.below = directionY == -1;
        collisionInfo.above = directionY == 1;
      }
    }
  }

  private void ClimbSlope(ref Vector3 moveDistance, float slopeAngle)
  {
    float targetMoveDistanceX = Mathf.Abs(moveDistance.x);
    float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * targetMoveDistanceX;
    if (moveDistance.y <= climbVelocityY)
    {
      moveDistance.y = climbVelocityY;
      moveDistance.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * targetMoveDistanceX * Mathf.Sign(moveDistance.x);
      collisionInfo.below = true;
      collisionInfo.climbingSlope = true;
      collisionInfo.slopeAngle = slopeAngle;
    }
  }

  private void DescendSlope(ref Vector3 moveDistance)
  {
    float directionX = Mathf.Sign(moveDistance.x);
    Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, layerMask);

    if (hit)
    {
      float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
      if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
      {
        if (Mathf.Sign(hit.normal.x) == directionX)
        {
          if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveDistance.x))
          {
            float targetMoveDistanceX = Mathf.Abs(moveDistance.x);
            float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * targetMoveDistanceX;
            moveDistance.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * targetMoveDistanceX * Mathf.Sign(moveDistance.x);
            moveDistance.y -= descendVelocityY;
            collisionInfo.slopeAngle = slopeAngle;
            collisionInfo.below = true;
            collisionInfo.descendingSlope = true;
          }
        }
      }
    }
  }

  private void CheckForSlopeChange(ref Vector3 moveDistance)
  {
    if (collisionInfo.climbingSlope)
    {
      float directionX = Mathf.Sign(moveDistance.x);
      float rayLength = Mathf.Abs(moveDistance.x + skinWidth);
      Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveDistance.y;
      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, layerMask);
      if (hit)
      {
        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
        if (slopeAngle != collisionInfo.slopeAngle)
        {
          moveDistance.x = (hit.distance - skinWidth) * directionX;
          collisionInfo.slopeAngle = slopeAngle;
        }
      }
    }
  }

  public struct CollisionInfo
  {
    public bool above, below, left, right;
    public bool climbingSlope;
    public bool descendingSlope;
    public float slopeAngle, slopeAngleOld;
    public Vector3 velocityOld;

    public void Reset()
    {
      above = false;
      below = false;
      left = false;
      right = false;
      climbingSlope = false;
      descendingSlope = false;
      slopeAngleOld = slopeAngle;
      slopeAngle = 0;
    }
  }
}
