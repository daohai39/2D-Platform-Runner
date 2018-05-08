using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaGirl : Enemy
{   
    [SerializeField] private Transform knifePos;
    [SerializeField] private GameObject knifePrefab;

    public override void Move()
    {
        if (!Attack) {
            Animator.SetFloat("speed", 1);
            transform.Translate(speed * GetDirection() * Time.deltaTime);
        }
    }

    public void ThrowKnife() 
    {
        if (isFacingRight) {
            GameObject tmp = (GameObject) Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(0,0,-90));
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        } else {
            GameObject tmp = (GameObject) Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(0,0,90));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }
}
