using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimTarget;
    float speed = 5f;
    float force = 12;

    bool hitting;
    
    Animator animator;
    public Transform ball;

    Vector3 aimTargetInitialPosition;

    ShotManager shotManager;
    Shot currentshot;

    [SerializeField] Transform serveRight;
    [SerializeField] Transform serveLeft;

    bool servedRight = true;
    private void Start()
    {
        animator = GetComponent<Animator>();
        aimTargetInitialPosition = aimTarget.position;
        shotManager = GetComponent<ShotManager>();
        currentshot = shotManager.topspin;
    }
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        if(Input.GetKeyDown(KeyCode.F))
        {
            hitting = true;
            currentshot = shotManager.topspin;
        }
        else if(Input.GetKeyUp(KeyCode.F))
        {
            hitting= false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true;
            currentshot = shotManager.flat;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            hitting = true;
            currentshot = shotManager.flatServe;
            GetComponent<BoxCollider>().enabled = false;
            animator.Play("serve-prepare");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            hitting = true;
            currentshot = shotManager.kickServe;
            GetComponent<BoxCollider>().enabled = false;
            animator.Play("serve-prepare");
        }

        if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.T))
        {
            hitting = false;
            GetComponent<BoxCollider>().enabled = true;
            ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);
            Vector3 dir = aimTarget.position - transform.position;
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentshot.hitforce + new Vector3(0, currentshot.upforce, 0);
            animator.Play("serve");
            ball.GetComponent<Ball>().hitter = "player";
            ball.GetComponent<Ball>().playing = true;
        }


        
        




        if (hitting )
        {
            aimTarget.Translate(new Vector3(h,0,0) * speed * 2 * Time.deltaTime);
        }
        if( (h != 0 || v != 0) && !hitting)
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);
        }
                
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Vector3 dir = aimTarget.position - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentshot.hitforce + new Vector3(0 ,currentshot.upforce, 0);
            
            Vector3 ballDir = ball.position - transform.position;
            if(ballDir.x >= 0 ) 
            {
                animator.Play("forehand"); 
            }
            else
            {
                animator.Play("backhand");
                
            }

            ball.GetComponent<Ball>().hitter = "player";
            aimTarget.position = aimTargetInitialPosition;
        }
    }

    public void Reset()
    {
        if(servedRight)
            transform.position = serveLeft.position;
        else
            transform.position = serveRight.position;

        servedRight = !servedRight;
    }
}
