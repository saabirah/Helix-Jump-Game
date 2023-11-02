using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool isSuperSpeedActive;
    public float impluseForce = 5f;
    public int perfectPass = 0;
    private bool ignoreNextCollission;

    public Rigidbody rb;
    private Vector3 startPos;


    // Utiliser pour les initialisation
    void Awake()
    {
      startPos = transform.position;        
    }



    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ball Touch something");
        if (ignoreNextCollission)
            return;

        if (isSuperSpeedActive)
        {
            if (!collision.transform.GetComponent<Goal>())
            {
                Destroy(collision.transform.parent.gameObject,0.3f);
                Debug.Log("Destorying platform");

                /*foreach (Transform t in other.transform.parent)
                {
                    gameObject.AddComponent<TriangleExplosion>();

                    StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
                    //Destroy(other.gameObject);
                    Debug.Log("exploding - exploding - exploding - exploding");
                }
                Destroy(collision.transform.parent.gameObject);
                */

            }
        } else
            {
                //si la balle touche une part rouge la partie est initialisé
                //adding resetLevel functionality via deathPart- initialized when deathPart is hit
                DeathPart deathPart = collision.transform.GetComponent<DeathPart>();
                if (deathPart)
                {
                    deathPart.HitDeathPart();
                    Debug.Log("hhhhhhhhhh");
                }

        }

        
 
        //Debug.Log("la balle a touchée la plateforme");

        rb.velocity = Vector3.zero; // Remove velocity to not make the ball jump higher after falling done a greater distance
        rb.AddForce(Vector3.up * impluseForce, ForceMode.Impulse);

        // Safety check
        ignoreNextCollission = true;
        Invoke("AllowCollission", 0.2f);

        // Handlig super speed
        perfectPass = 0;
        isSuperSpeedActive = false;

       // GameManager.singleton.AddScore(1); // add score
       // Debug.Log(GameManager.singleton.score);
    }

    // Update is called once per frame
    void Update()
    {
        if(perfectPass >=3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive=true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }

    }

    
    private void AllowCollission()
    {
        ignoreNextCollission = false;
    }


    public void ResetBall()
    {
        transform.position = startPos;
    }

  
}
