using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
  protected const float skinWidth = 0.015f;

  [Header("Ray Counts")]
  [SerializeField]
  protected int horizontalRayCount = 4;
  [SerializeField]
  protected int verticalRayCount = 4;

  [Header("LayerMask")]
  [SerializeField]
  protected LayerMask layerMask;

  protected BoxCollider2D boxCollider;

  protected RaycastOrigins raycastOrigins;
  protected float horizontalRaySpacing = 0f;
  protected float verticalRaySpacing = 0f;

  // Start is called before the first frame update
  public virtual void Start()
  {
    boxCollider = GetComponent<BoxCollider2D>();
    CalculateRaySpacing();
  }

  // Update is called once per frame
  protected void UpdateRaycastOrigins()
  {
    Bounds bounds = boxCollider.bounds;
    // Inset the bounds by skin width.
    bounds.Expand(skinWidth * -2);

    raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
    raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
    raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
  }

  protected void CalculateRaySpacing()
  {
    Bounds bounds = boxCollider.bounds;
    // Inset the bounds by skin width.
    bounds.Expand(skinWidth * -2);

    horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
    verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

    horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
    verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
  }

  private void DebugDrawVerticalRays()
  {
    for (int i = 0; i < verticalRayCount; i++)
    {
    }
  }

  protected struct RaycastOrigins
  {
    public Vector2 topLeft, topRight, bottomLeft, bottomRight;
  }
}