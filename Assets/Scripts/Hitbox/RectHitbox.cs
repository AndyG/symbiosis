using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectHitbox : MonoBehaviour
{
    [SerializeField]
    private float attackRangeX = 5;
    [SerializeField]
    private float attackRangeY = 3;

    void Awake() {
        // Hitboxes should be disabled until someone enables them.
        enabled = false;
    }

    public Collider2D[] GetHurtboxes(LayerMask layerMask) {
        return Physics2D.OverlapBoxAll(transform.position, new Vector2(attackRangeX, attackRangeY), 0f, layerMask);
    }

    void OnDrawGizmosSelected() {
        if (enabled) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(attackRangeX/2, attackRangeY/2));
        }
    }
}
