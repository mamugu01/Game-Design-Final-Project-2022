using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
 
    private Vector3 startPos;
    public Vector3 endPos;
 
    private Vector3 targetPos;
 
    public float speed;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        targetPos = endPos;
    }
 
    void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
 
        if (currentPos == endPos)
        {
            targetPos = startPos;
            Debug.Log("Reached end, switching target position");
        }
        else if (currentPos == startPos)
        {
            targetPos = endPos;
            Debug.Log("Reached beginning, switching target position");
        }
 
        Vector3 targetDirection = (targetPos - currentPos).normalized;
        rb.MovePosition(currentPos + targetDirection * speed * Time.deltaTime);
    }
}
 