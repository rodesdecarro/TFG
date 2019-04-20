using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Monster target;

    private float speed;

    private int damage;

    private float splashArea;

    private float slowDuration;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (target != null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        }
        else
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    public void Initialize(Tower tower)
    {
        speed = tower.ProjectileSpeed;
        target = tower.Target;
        damage = Random.Range(tower.MinDamage, tower.MaxDamage + 1);

        if (tower.CritChance > Random.Range(0f, 1f))
        {
            damage *= 2;
        }

        splashArea = tower.SplashArea;
        slowDuration = tower.SlowDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            Monster monster = collision.GetComponent<Monster>();

            if (monster == target)
            {
                if (splashArea > 0)
                {
                    Explode(monster.transform.position);
                }
                else
                {
                    monster.Damage(damage, slowDuration);
                }

                GameManager.Instance.Pool.ReleaseObject(gameObject);
            }
        }
    }

    private void Explode(Vector3 center)
    {
        foreach (Monster monster in GameManager.Instance.ActiveMonsters)
        {
            float distance = Vector3.Distance(center, monster.transform.position);

            if (distance <= splashArea)
            {
                // Deals damage to the monster proportionally to the distance, from 100% (center of the explosion) to 50% (border of the explosion)
                monster.Damage((int)(((splashArea - distance) * 0.5 + 0.5) * damage), slowDuration);
            }
        }
    }
}
