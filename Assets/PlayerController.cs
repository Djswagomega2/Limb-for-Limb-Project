using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Recovery,FootStool
    #region Movement Based Values
    [Header("Movement Based Values")]
    public Vector2 movementDir,jumpForce;
    public float movementSpeed;
    public Rigidbody2D rb;
    #endregion

    #region Jumping
    [Header("Jumping")]
    public LayerMask groundLayer;
    public Transform groundPoint;
    public float gravityscale;
    public float gravityscalemultiplier;
    public float rayLength;
    public int maxJumps = 2;
    [SerializeField] private int currentJumpCount;
    #endregion

    #region Weapon
    [Header("Weapon")]
    public GameObject[] weapon;
    public GameObject heldWeapon;
    [SerializeField] private Transform weaponInst;
    #endregion

    #region Death and Respawn
    [Header("Death and Respawn")]
    [SerializeField] private Transform stageRespawner;
    [SerializeField] private float deathSeconds;
    
    #endregion
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       stageRespawner = GameObject.Find("Stage Respawner").GetComponent<Transform>();
       weaponInst = GameObject.Find("WeaponInst").GetComponent<Transform>();
       heldWeapon = weapon[0];
       currentJumpCount = maxJumps;
       
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jumping();
        //AttackTest();
        Debug.Log(Input.GetAxisRaw("Vertical"));
    }

    public void Movement()
    {
       float x = Input.GetAxisRaw("Horizontal");
       rb.velocity = new Vector2(x * movementSpeed, rb.velocity.y);
    }

    public void Jumping()
    {
        float y = Input.GetAxisRaw("Vertical");

        bool isGrounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(groundPoint.position,Vector2.down,rayLength,groundLayer);
            return hit.collider != null;
        }

        if(isGrounded())
        {
            currentJumpCount = maxJumps;
        }  

        if(Input.GetButton("Jump") && currentJumpCount > 0)
        {
            //rb.gravityScale = gravityscale;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            currentJumpCount--;
           
        }

        IEnumerator FastFall()
        {
            rb.gravityScale = gravityscale * gravityscalemultiplier;
            yield return new WaitUntil (() => isGrounded());
            rb.gravityScale = gravityscale;
        }

        if(rb.velocity.y > 0f && y < 0) 
        {
            StartCoroutine(FastFall());
        }
    }

   
    private void OnTriggerEnter2D(Collider2D other) {//For level 3 death valley
        switch (other.gameObject.tag)
		{
            case "DeathValley":
                Debug.Log("dead");
                StartCoroutine(Death());
                rb.velocity = Vector3.zero;
                //rb.angularVelocity = 0;
            break;
		}
    }

    private IEnumerator Death()
    {
        this.enabled = false;
        this.transform.position = stageRespawner.transform.position;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(deathSeconds);
        this.enabled = true;
        rb.gravityScale = gravityscale;
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(groundPoint.position, Vector2.down * rayLength, Color.red);
    }
}
