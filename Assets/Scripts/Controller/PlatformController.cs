using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{

  [SerializeField]
  private LayerMask passengerMask;

  [SerializeField]
  private Vector3 move;

  [SerializeField]
  private bool drawDebugRays = true;

  private List<PassengerMovement> passengerMovement;
  private Dictionary<Transform, LagueController2D> passengerDictionary = new Dictionary<Transform, LagueController2D>();

  // Start is called before the first frame update
  public override void Start()
  {
    base.Start();
  }

  // Update is called once per frame
  void Update()
  {
    UpdateRaycastOrigins();
    Vector3 velocity = move * Time.deltaTime;
    CalculatePassengerMovement(velocity);
    MovePassengers(true);
    transform.Translate(velocity);
    MovePassengers(false);
  }

  private struct PassengerMovement
  {
    public Transform transform;
    public Vector3 moveDistance;
    public bool isStandingOnPlatform;
    public bool moveBeforePlatform;

    public PassengerMovement(Transform transform, Vector3 moveDistance, bool isStandingOnPlatform, bool moveBeforePlatform)
    {
      this.transform = transform;
      this.moveDistance = moveDistance;
      this.isStandingOnPlatform = isStandingOnPlatform;
      this.moveBeforePlatform = moveBeforePlatform;
    }
  }

  private void MovePassengers(bool beforeMovePlatform)
  {
    foreach (PassengerMovement passenger in passengerMovement)
    {
      if (passenger.moveBeforePlatform == beforeMovePlatform)
      {
        if (!passengerDictionary.ContainsKey(passenger.transform))
        {
          passengerDictionary[passenger.transform] = passenger.transform.GetComponent<LagueController2D>();
        }
        passengerDictionary[passenger.transform].Move(passenger.moveDistance, passenger.isStandingOnPlatform);
      }
    }
  }

  private void CalculatePassengerMovement(Vector3 moveDistance)
  {
    passengerMovement = new List<PassengerMovement>();
    float directionX = Mathf.Sign(moveDistance.x);
    float directionY = Mathf.Sign(moveDistance.y);

    HashSet<Transform> movedPassengers = new HashSet<Transform>();

    // Vertically moving platform
    if (moveDistance.y != 0)
    {
      float rayLength = Mathf.Abs(moveDistance.y) + skinWidth;
      for (int i = 0; i < verticalRayCount; i++)
      {
        Vector2 baseRayOrigin = directionY == -1f
        ? raycastOrigins.bottomLeft
        : raycastOrigins.topLeft;

        // Add move distance X since vertical collisions happen after horizontal collisions.
        float rayOriginOffset = verticalRaySpacing * i;
        Vector2 rayOrigin = baseRayOrigin + Vector2.right * rayOriginOffset;
        Vector2 rayDirection = Vector2.up * directionY;
        if (drawDebugRays)
        {
          Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
        }

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, passengerMask);
        if (hit)
        {
          if (movedPassengers.Contains(hit.transform))
          {
            break;
          }
          float pushX = (directionY == 1) ? moveDistance.x : 0;
          float pushY = moveDistance.y - (hit.distance - skinWidth) * directionY;
          passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
          movedPassengers.Add(hit.transform);
        }
      }
    }

    // Horizontally moving platform
    if (moveDistance.x != 0)
    {
      float rayLength = Mathf.Abs(moveDistance.x) + skinWidth;
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
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, passengerMask);
        if (hit)
        {
          if (movedPassengers.Contains(hit.transform))
          {
            break;
          }
          float pushX = moveDistance.x - (hit.distance - skinWidth) * directionX;
          // Add some small downward force to make the player check for below collisions (and thus be able to jump).
          float pushY = -skinWidth;
          passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
          movedPassengers.Add(hit.transform);
        }
      }
    }

    // Check for passengers on top of a horizontally or downward moving platform
    if (directionY == -1 || (moveDistance.y == 0 && moveDistance.x != 0))
    {
      // Just enough to get out of the collider.
      float rayLength = skinWidth * 2;
      for (int i = 0; i < horizontalRayCount; i++)
      {

        float rayOriginOffset = verticalRaySpacing * i;
        Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * rayOriginOffset;
        Vector2 rayDirection = Vector2.up;
        if (drawDebugRays)
        {
          Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);
        }
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, passengerMask);
        if (hit)
        {
          if (movedPassengers.Contains(hit.transform))
          {
            break;
          }
          float pushX = moveDistance.x;
          float pushY = moveDistance.y - (hit.distance - skinWidth) * directionY;
          passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
          movedPassengers.Add(hit.transform);
        }
      }
    }
  }
}
