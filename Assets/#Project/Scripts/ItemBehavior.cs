using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public int id;
    public LevelManager manager;

    public bool mouseOver = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0) && mouseOver){
            manager.RevealMaterial(id); // quand on click il revèle le material
        }
    }

    void OnMouseOver(){  // OnMouseOver c'est quand on passe avec la souris, c'est reconnu de base par unity
        mouseOver = true;
        animator.SetBool("MouseOver", true);
    }

    void OnMouseExit(){
        mouseOver = false;
        animator.SetBool("MouseOver", false); // communique avec l'animator de l'item
    }

    public void HasBeenSelected(bool selected){
        animator.SetBool("ItemSelected", selected);
        //animator.SetBool("MouseOver", false);
    }

    public void HasBeenMatched(){
        animator.SetBool("ItemMatch", true);
    }
}
