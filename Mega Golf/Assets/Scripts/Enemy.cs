using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
 
    private Vector3 startPos;
    public Vector3 endPos;
 
    private Vector3 targetPos;
 
    public float speed;
    private int sign = 0;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        targetPos = endPos;
    }
 
    void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        
 
        if (Math.Abs(currentPos.x - endPos.x) < 0.3 && Math.Abs(currentPos.y - endPos.y) < 0.3)
        {
            targetPos = startPos;
            Debug.Log("Reached end, switching target position");
            if (this.tag == "tumbleweed") sign = -1;
            if (this.tag == "taxi") transform.localScale = new Vector3 (-1,1,1);;
        }
        else if (Math.Abs(currentPos.x - startPos.x) < 0.3 && Math.Abs(currentPos.y - startPos.y) < 0.3)
        {
            targetPos = endPos;
            Debug.Log("Reached beginning, switching target position");
            if (this.tag == "tumbleweed") sign = 1;
            if (this.tag == "taxi") transform.localScale = new Vector3 (1,1,1);
        }
 
        Vector3 targetDirection = (targetPos - currentPos).normalized;
        rb.MovePosition(currentPos + targetDirection * speed * Time.deltaTime);
        rb.angularVelocity = 500f * sign;
        
    }
}
 