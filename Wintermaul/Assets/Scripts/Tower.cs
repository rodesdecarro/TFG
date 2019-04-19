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


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.transform.localScale = new Vector3(range, range, range);
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    public void Select()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            targets.Add(collision.GetComponent<Monster>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            Monster monster = collision.GetComponent<Monster>();
            targets.Remove(monster);

            if (Target == monster)
            {
                Target = null;
            }
        }
    }

    private void Attack()
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

        if (Target == null || !Target.IsActive)
        {
            // Select the next active target closest to goal
            Target = targets.Where(x => x.IsActive).OrderBy(x => x.PathLength).FirstOrDefault();
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

    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }
}
