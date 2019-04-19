using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;

    private SpriteRenderer rangeSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
    }

    public void Activate(Sprite sprite, int range)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;

        rangeSpriteRenderer.enabled = true;
        rangeSpriteRenderer.transform.localScale = new Vector3(range, range);
    }

    internal void Deactivate()
    {
        spriteRenderer.enabled = false;
        rangeSpriteRenderer.enabled = false;
    }
}
