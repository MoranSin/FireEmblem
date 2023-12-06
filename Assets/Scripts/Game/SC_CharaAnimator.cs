using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_CharaAnimator : MonoBehaviour
{
    Animator animator;

    [SerializeField] bool move;
    [SerializeField] bool attack;
    [SerializeField] bool defeated;

    private float attackDuration = 0.5f;
    private float attackStartTime = 0.0f;
   // private float fadeOutTime = 1.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartMoving()
    {
        move = true;
    }

    public void StopMoving()
    {
        move = false;
    }

    public void Attack()
    {
        attackStartTime = Time.time;
        attack = true;

    }

   /* public void Defeated()
    {
        defeated = true;
        if (defeated)
        {
            StartCoroutine(DoFadeIn(GetComponent<SpriteRenderer>()));
        }
    }
   */

    private void Update()
    {
        animator.SetBool("Move", move);
        animator.SetBool("Attack", attack);
    }

    private void LateUpdate()
    {
        if(attack == true)
        {
            float elapsedTime = Time.time - attackStartTime;
            if (elapsedTime >= attackDuration)
            {
                attack = false;
            }
        }
    }

   /* IEnumerator DoFadeIn(SpriteRenderer character)
    {
        Color Alive = character.color;
        while (Alive.a >= 0)
        {
            Alive.a -= Time.deltaTime/ fadeOutTime;
            character.color = Alive;
        }
        yield return null;
    } */
}
