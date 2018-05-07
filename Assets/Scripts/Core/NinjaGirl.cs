using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaGirl : Enemy
{   

    public override void Move()
    {
        if (!Attack) {
            Animator.SetFloat("speed", 1);
            transform.Translate(speed * GetDirection() * Time.deltaTime);
        }
    }


}
