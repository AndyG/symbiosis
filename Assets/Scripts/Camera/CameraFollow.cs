using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  [SerializeField]
  private GameObject targetProvider;
  [SerializeField]
  private Vector2 focusAreaSize;
  [SerializeField]
  private float verticalOffset;

  [Header("Lookahead")]
  [SerializeField]
  private float lookaheadDstX;
  [SerializeField]
  private float lookaheadSmoothTimeX;

  [Header("Smoothing")]
  [SerializeField]
  private float verticalSmoothTime;

  [Header("Debug")]
  [SerializeField]
  private bool drawGizmos = false;

  private Target target;
  private FocusArea focusArea;

  private float currentLookaheadX;
  private float targetLookaheadX;
  private float lookaheadDirX;
  private float smoothLookVelocityX;
  private float smoothVelocityY;

  private bool lookaheadStopped;

  void Start()
  {
    target = targetProvider.GetComponent<Target>();
    focusArea = new FocusArea(target.GetCameraTrackingBounds(), focusAreaSize);
  }

  void LateUpdate()
  {
    focusArea.Update(target.GetCameraTrackingBounds());

    Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

    if (focusArea.velocity.x != 0)
    {
      lookaheadDirX = Mathf.Sign(focusArea.velocity.x);
      int influenceDirection = target.GetLookaheadInfluenceDirection();
      if (influenceDirection != 0 && influenceDirection == lookaheadDirX)
      {
        targetLookaheadX = lookaheadDirX * lookaheadDstX;
        lookaheadStopped = false;
      }
    }

    if (!lookaheadStopped && focusArea.velocity.x == 0)
    {
      targetLookaheadX = currentLookaheadX + (lookaheadDirX * lookaheadDstX - currentLookaheadX) / 4f;
      lookaheadStopped = true;
    }

    currentLookaheadX = Mathf.SmoothDamp(currentLookaheadX, targetLookaheadX, ref smoothLookVelocityX, lookaheadSmoothTimeX);
    focusPosition = focusPosition + Vector2.right * currentLookaheadX;
    focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
    transform.position = (Vector3)(focusPosition) + Vector3.forward * -10;
  }

  void OnDrawGizmos()
  {
    if (drawGizmos)
    {
      Gizmos.color = new Color(1, 0, 0, 0.5f);
      Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }
  }

  public interface Target
  {
    Bounds GetCameraTrackingBounds();
    int GetLookaheadInfluenceDirection();
  }

  private struct FocusArea
  {
    public Vector2 center;
    public Vector2 velocity;
    private float left, right;
    private float top, bottom;

    public FocusArea(Bounds targetBounds, Vector2 size)
    {
      left = targetBounds.center.x - size.x / 2;
      right = targetBounds.center.x + size.x / 2;
      bottom = targetBounds.min.y;
      top = targetBounds.min.y + size.y;

      velocity = Vector2.zero;
      center = new Vector2((right + left) / 2, (top + bottom) / 2);
    }

    public void Update(Bounds targetBounds)
    {
      float shiftX = 0;
      if (targetBounds.min.x < left)
      {
        shiftX = targetBounds.min.x - left;
      }
      else if (targetBounds.max.x > right)
      {
        shiftX = targetBounds.max.x - right;
      }

      left += shiftX;
      right += shiftX;

      float shiftY = 0;
      if (targetBounds.min.y < bottom)
      {
        shiftY = targetBounds.min.y - bottom;
      }
      else if (targetBounds.max.y > top)
      {
        shiftY = targetBounds.max.y - top;
      }

      bottom += shiftY;
      top += shiftY;

      velocity = new Vector2(shiftX, shiftY);
      center = new Vector2((right + left) / 2, (top + bottom) / 2);
    }
  }
}
