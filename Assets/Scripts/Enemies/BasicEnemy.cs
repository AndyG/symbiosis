using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LagueController2D))]
public class BasicEnemy : MonoBehaviour, Hurtable
{

  [SerializeField]
  private float speed = 1f;

  [SerializeField]
  private float gravity = 1f;

  private LagueController2D controller2D;

  // Start is called before the first frame update
  void Start()
  {
    controller2D = GetComponent<LagueController2D>();
  }

  // Update is called once per frame
  void Update()
  {
    Vector2 velocity = Vector2.left * speed;
    AddGravity(ref velocity);
    controller2D.Move(velocity);
  }

  private void AddGravity(ref Vector2 velocity)
  {
    velocity.y = velocity.y - gravity;
  }

  public void OnHurt(RectHitbox hitbox) {
    GameObject.Destroy(this.transform.gameObject);
  }
}
