using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void PlayHit()
    {
        anim.SetTrigger("Hurt");
    }

    public void PlayDeath()   
    {
        anim.SetTrigger("Death");
    }
}
