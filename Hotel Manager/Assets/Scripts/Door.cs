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
        spriteRenderer.material.SetInt("open", 1);
        stoper = 1f;
    }
    
    private void Close()
    {
        spriteRenderer.material.SetFloat("open", 0);
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
