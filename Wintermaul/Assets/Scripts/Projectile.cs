using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Monster target;

    private float speed;

    private int damage;

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

    public void Initialize(Tower parent)
    {
        speed = parent.ProjectileSpeed;
        target = parent.Target;
        damage = parent.Damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            Monster monster = collision.GetComponent<Monster>();

            if (monster == target)
            {
                monster.Damage(damage);
                GameManager.Instance.Pool.ReleaseObject(gameObject);
            }
        }
    }
}
