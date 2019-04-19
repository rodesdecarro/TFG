using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private int range = 0;

    private SpriteRenderer spriteRenderer;

    public int Range { get => range; }

    public float ProjectileSpeed { get => projectileSpeed; }

    public Monster Target { get; private set; }

    public int Damage { get => damage; }

    public int Price { get => price; }
    public Tower Upgrade { get => upgrade?.transform.GetChild(0).GetComponent<Tower>(); }
    public GameObject UpgradePrefab { get => upgrade; }

    private List<Monster> targets = new List<Monster>();

    private bool canAttack = true;

    private float attackTimer = 0f;

    [SerializeField]
    private float attackCooldown = 0f;

    [SerializeField]
    private string projectileType = "";

    [SerializeField]
    private float projectileSpeed = 0f;

    [SerializeField]
    private int damage = 0;

    [SerializeField]
    private int price = 0;

    [SerializeField]
    private GameObject upgrade = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    public void Select()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.transform.localScale = new Vector3(range, range, range);
        }

        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void Attack()
    {
        UpdateAttackTimer();

        if (canAttack)
        {
            // If target is not active or is outside radius, remove it
            if (Target != null && (!Target.IsActive || !GetComponent<Collider2D>().IsTouching(Target.GetComponent<Collider2D>())))
            {
                Target = null;
            }

            if (Target == null)
            {
                // Select the next active target closest to goal
                Target = GameManager.Instance.ActiveMonsters.Where(x => GetComponent<Collider2D>().IsTouching(x.GetComponent<Collider2D>())).OrderBy(x => x.PathLength).FirstOrDefault();
            }

            if (Target != null)
            {
                if (canAttack)
                {
                    Shoot();
                    canAttack = false;
                }
            }
        }
    }

    private void UpdateAttackTimer()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
    }

    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }
}
