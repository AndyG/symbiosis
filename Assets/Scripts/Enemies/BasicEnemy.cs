using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LagueController2D))]
[RequireComponent(typeof(Health))]
public class BasicEnemy : MonoBehaviour, Hurtable
{

  [Header("Movement")]
  [SerializeField] private float speed = 1f;
  [SerializeField] private float gravity = 1f;
  [SerializeField] private Vector2 velocity;

  [Header("Knockback")]
  [SerializeField] private Vector2 knockbackVelocity;
  [SerializeField] private float hitFlashDuration;
  [SerializeField] private Material hitFlashMaterial;
  [SerializeField] private float hitstopDuration = 0.2f;

  [Header("Attacking")]
  [SerializeField]
  private LayerMask attackLayerMask;

  private float hitstopTime = 0f;

  [Header("State")]
  [SerializeField]
  private State state;

  private Material defaultMaterial;

  private Animator animator;
  private SpriteRenderer spriteRenderer;
  private SpawnOnHit onHitObject;
  private LagueController2D controller2D;
  private Health health;
  private RectHitbox hitbox;

  // Start is called before the first frame update
  void Awake()
  {
    controller2D = GetComponent<LagueController2D>();
    onHitObject = GetComponent<SpawnOnHit>();
    animator = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    health = GetComponent<Health>();
    hitbox = GetComponent<RectHitbox>();
    defaultMaterial = spriteRenderer.material;

    health.OnDiedEvent += this.OnDied;
  }

  // Update is called once per frame
  void Update()
  {
    if (hitstopTime >= 0f) {
      hitstopTime -= Time.deltaTime;
      return;
    }

    this.animator.enabled = true;
    switch (state) {
      case State.IDLE:
        IdleTick();
        break;
      case State.KNOCKBACK:
        KnockbackTick();
        break;
    }
    AddGravity(ref velocity);
    controller2D.Move(velocity * Time.deltaTime);
  }

  private void IdleTick() {
    velocity = Vector2.left * speed;
    Attack();
  }

  private void KnockbackTick() {
    if (controller2D.GetCollisions().below && velocity.y <= 0) {
      this.state = State.IDLE;
    }
  }

  private void Attack() {
    Collider2D[] hurtboxes = hitbox.GetHurtboxes(attackLayerMask);
    for (int i = 0; i < hurtboxes.Length; i++) {
      Hurtable hurtable = hurtboxes[i].GetComponent<Hurtable>();
      if (hurtable != null) {
        hurtable.OnHurt(hitbox);
        Hitstop();
      }
    }
  }

  public void OnHurt(RectHitbox hitbox) {
    this.state = State.KNOCKBACK;
    this.velocity = knockbackVelocity;

    Hitstop();
    StopAllCoroutines();
    StartCoroutine(HitFlash());

    if (onHitObject != null) {
      onHitObject.Spawn();
    }

    health.subtract(1);
  }

  private void OnDied() {
    StopAllCoroutines();
    GameObject.Destroy(this.transform.gameObject);
  }

  private IEnumerator HitFlash() {
    spriteRenderer.material = hitFlashMaterial;
    yield return new WaitForSeconds(hitFlashDuration);
    spriteRenderer.material = defaultMaterial;
  }

  private void AddGravity(ref Vector2 velocity)
  {
    velocity.y = velocity.y - gravity * Time.deltaTime;
  }

  private enum State {
    IDLE,
    KNOCKBACK
  }

  private void Hitstop() {
    this.hitstopTime = hitstopDuration;
    this.animator.enabled = false;
  }
}
