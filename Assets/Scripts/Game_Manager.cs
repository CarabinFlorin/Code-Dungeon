using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    private Animator animator;
    bool destroyed = false;
    //Sound
    private SFX_Script SFX;

    // Start is called before the first frame update
    void Start()
    {
        //Set animator
        animator = GetComponent<Animator>();
        //Set SFX
        SFX = FindObjectOfType<SFX_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(GameObject.FindWithTag("Enemy").transform.position, transform.GetComponentInParent<Transform>().position) < 1.5)
        {
            if (transform.GetComponentInParent<Player_Controller>().attack_bool)
            {
                //Destroy(GameObject.FindGameObjectWithTag("Enemy"));
                GameObject.FindWithTag("Enemy").transform.position = new Vector2(1000,1000);
                destroyed = true;
            }
            else if (!destroyed)
            {
                StartCoroutine(Death());
            }
        }
    }

    //Player hit coroutine
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(.2f);
        if (!destroyed)
        {
            //SFX.Explosion.Play();
            //SFX.Impact.Play();
            SFX.Hurt.Play();
            transform.GetComponentInParent<Player_Controller>().stop = true;
            animator.SetBool("explosion_anim", true);
            yield return new WaitForSeconds(.22f);
            animator.SetBool("explosion_anim", false);
            StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>().Death());
        }
    }
}