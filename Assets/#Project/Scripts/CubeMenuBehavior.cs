using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMenuBehavior : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnMouseOver()
    {
        animator.SetBool("mouse over", true);
        
    }
    void OnMouseExit()
    {
        animator.SetBool("mouse over", false);

    }
}
