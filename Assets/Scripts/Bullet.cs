using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

public class Bullet : MonoBehaviour
{
    public GameObject deathSplash;
    public GameObject bloodSplash;
    public GameObject explosion;
    public float damage = 50;

    private Transform textLoc;
    public Text addScoreText;

    public string origin = "enemy";

    private AudioManager audioManager;

    private PauseMenu pauseMenu;

    private DateTime insTime;

    private HealthManager health;

    private Shooting shooting;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        textLoc = GameObject.FindGameObjectWithTag("TextLoc").transform;
        insTime = DateTime.Now;

        shooting = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //GameObject col = collision.gameObject;
        GameObject col = collision.collider.gameObject;
        var instPos = col.transform.position;

        int mult = 1;
        
        if (col.tag == "Wall" || col.tag == "PlayerWall")
        {
            audioManager.Play(name: "Wall", soundPos: transform.position);
            GameObject effect = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(effect, .75f);
            Destroy(gameObject);
        }
        else if (col.tag == "Gewis" || col.tag == "Player" || col.tag == "Boomer")
        {
            try
            {
                audioManager.Play("Body", col.transform.position);
            }
            catch
            {
                audioManager.Play("Body");
            }
            health = col.GetComponent<HealthManager>();
            health.Health -= damage;

            if (health.Health <= 0)
            {
                if (col.tag == "Player")
                {
                    health.DeathHealth();
                    pauseMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<PauseMenu>();
                    pauseMenu.DeathMenu();
                }
                if (origin == "Player")
                    mult = shooting.multFactor;
                if (col.tag == "Gewis" && col.name != "Boss")
                {

                    PlayerScore.Score += mult*50;
                    var txt = Instantiate(addScoreText, textLoc);
                    txt.text = string.Format("Enemy killed: +{0}", mult*50);
                    Destroy(txt, 10f);
                    shooting.AddKill();
                }
                else if (col.name == "Boss")
                {
                    PlayerScore.Score += mult*300;
                    var txt = Instantiate(addScoreText, textLoc);
                    txt.text = string.Format("Boss killed: +{0}", mult*300);
                    Destroy(txt, 10f);

                    shooting.AddKill();
                }
                if (col.tag == "Boomer")
                {
                    PlayerScore.Score += mult*100;
                    var txt = Instantiate(addScoreText, textLoc);
                    txt.text = string.Format("Suicider killed: +{0}", mult*100);

                    try
                    {
                        col.GetComponent<SuiciderMovement>().Boom();
                    }
                    catch
                    {
                        col.GetComponent<EnigmaMine>().Explode();
                    }

                    shooting.AddKill();

                }
                else
                {
                    GameObject effect = Instantiate(deathSplash, col.transform.position, Quaternion.identity);

                    Destroy(effect, .75f);
                    Destroy(col);
                }
                Destroy(gameObject);

            }
            else
            {
                GameObject effect = Instantiate(bloodSplash, col.transform.position, Quaternion.identity);
                Destroy(effect, .75f);
                Destroy(gameObject);
            }
        }
         
        else if (col.tag == "Destrucitble")
        {
            audioManager.Play("Wall", transform.position);
            PlayerScore.Score -= 15;
            var txt = Instantiate(addScoreText, textLoc);
            txt.text = "Computer Destroyed: -15";
            txt.color = Color.cyan;
            Destroy(col);
            Destroy(txt, 10f);

            GameObject effect = Instantiate(explosion, instPos, transform.rotation);
            Destroy(effect, .75f);
            Destroy(gameObject);
        }
        else if (col.tag == "EnigmaPC")
        {
            audioManager.Play("Wall", transform.position);
            health = col.GetComponent<HealthManager>();
            health.Health -= damage;

            if (health.Health <= 0)
            {
                audioManager.Play("Boom", transform.position);
                GameObject effect = Instantiate(explosion, instPos, transform.rotation);


                PlayerScore.Score += 500;
                var txt = Instantiate(addScoreText, textLoc);
                txt.text = "Pinder Saved: + 500";
                txt.color = Color.green;

                Destroy(col);
                Destroy(txt, 10f);

                Destroy(effect, 5f);
               
            }
            Destroy(gameObject);
        }
        else if ((DateTime.Now - insTime).TotalSeconds > .05f)
        {
            audioManager.Play("Wall", transform.position);
            GameObject effect = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(effect, .75f);
            Destroy(gameObject);
        }
    }
}
