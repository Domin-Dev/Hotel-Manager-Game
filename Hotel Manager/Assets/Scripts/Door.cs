using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Open()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        stoper = 1f;
    }
    
    private void Close()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }


    float stoper;
    private void Update()
    {
        if(stoper > 0)
        {
            stoper = stoper - Time.deltaTime;

            if(stoper <= 0)
            {
                Close();
            }
        }
    }

}
