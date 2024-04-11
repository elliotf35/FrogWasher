using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int lives = 3;
    public bool canTakeDamage = true;
    BoxCollider2D bc;
    void Start(){
        bc = GetComponent<BoxCollider2D>();
        
    }
    void Update(){
        if (lives <= 0){
            bc.enabled = false;
            transform.DetachChildren();
            StartCoroutine(ResetScene());
        }
    }
   
    IEnumerator DamageCooldown(){
        canTakeDamage = false;
        yield return new WaitForSeconds(3);
        canTakeDamage = true;
    }

    IEnumerator ResetScene(){
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }

    public void PlayerHit()
    {
        if (canTakeDamage){
            StartCoroutine(DamageCooldown());
            lives -= 1;
        }
        
    }
}
