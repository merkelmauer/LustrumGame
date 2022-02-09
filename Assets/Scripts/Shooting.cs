﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;


    public GameObject extraGunVisual;
    public Image timeBar;


    public Transform extraGun1;
    public Transform extraGun2;
    public Transform extraGun3;
    public Transform extraGun4;
    public Transform extraGun5;
    public Animator extraGunsAnim;
    public bool extraGuns = false;


    public Animator wallAnimator;
    public bool extraWalls = false;
    public GameObject wallVisual;
    public Image wallBar;

    public float shieldWaitTime = 7.5f;
    private DateTime shieldTime;



    //public int bulletsInClip = 30;
    //public int ammoAmount = 90;
    public bool reloading;
    public float reloadTime = 1.2f;

    public Animator animator;

    [SerializeField]
    public List<Sprite> gunList;
    public int gunLevel=0;
    public int maxGunLevel=3;
    public SpriteRenderer currentGun;


    public Text clip;
    public Text ammo;

    private DateTime reloadStart = DateTime.Now.AddSeconds(-10d);
    private List<float> damageMultiplier = new List<float> { 0.5f, 1f, 1.5f, 2f };
    public float gunWaitTime = 5f;
    private DateTime gunTime;
    public float bulletForce = 200f;
    
    // Update is called once per frame
    void Update()
    {
        if (extraGuns)
        {
            float prop = (float)(1 - (DateTime.Now - gunTime).TotalSeconds/ gunWaitTime);
       
            var rectt = timeBar.GetComponent<RectTransform>();
            rectt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, prop*400f);
        }

        if (extraWalls)
        {
            float prop = (float)(1 - (DateTime.Now - shieldTime).TotalSeconds / shieldWaitTime);

            var rectt = wallBar.GetComponent<RectTransform>();
            rectt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, prop * 400f);

        }


        if (reloading && (DateTime.Now - reloadStart).TotalSeconds > reloadTime)
        {
            
            reloading = false;
            animator.SetBool("reloading", false);
        }

        if (!reloading && PlayerScore.bulletsInClip != 30 && PlayerScore.ammoAmount > 0 && ( PlayerScore.bulletsInClip == 0 || Input.GetKeyDown("r")))
        {
            if (PlayerScore.ammoAmount + PlayerScore.bulletsInClip >= 30)
            {
                PlayerScore.ammoAmount -= (30 - PlayerScore.bulletsInClip);
                PlayerScore.bulletsInClip = 30;
            }
            else 
            {
                PlayerScore.bulletsInClip += PlayerScore.ammoAmount; 
                PlayerScore.ammoAmount = 0;

            }
            reloading = true;
            animator.SetBool("reloading", true);
            reloadStart = DateTime.Now;
        }
        if (Input.GetButtonDown("Fire1") && !PauseMenu.GameIsPaused && PlayerScore.bulletsInClip > 0 && !reloading)
        {
            
            Shoot();
            PlayerScore.bulletsInClip -= 1;
        }

        clip.text = PlayerScore.bulletsInClip.ToString();
        ammo.text = PlayerScore.ammoAmount.ToString();
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().damage *= damageMultiplier[gunLevel];
        //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        if (extraGuns)
        {
            ExtraShoot(extraGun1);
            ExtraShoot(extraGun2);
            ExtraShoot(extraGun3);
            ExtraShoot(extraGun4);
            ExtraShoot(extraGun5);
            //GameObject bullet1 = Instantiate(bulletPrefab, extraGun1.position, extraGun1.rotation);
            //bullet1.GetComponent<Bullet>().damage *= damageMultiplier[gunLevel];
            ////GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
            //rb.AddForce(extraGun1.up * bulletForce, ForceMode2D.Impulse);
        }
    }

    void ExtraShoot(Transform extraGun)
    {
        var bullet1 = Instantiate(bulletPrefab, extraGun.position, extraGun.rotation);
        bullet1.GetComponent<Bullet>().damage *= damageMultiplier[gunLevel];
        //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
        rb1.AddForce(extraGun.up * bulletForce, ForceMode2D.Impulse);
    }

    public void ActivateExtraGuns()
    {
        extraGuns = true;
        gunTime = DateTime.Now;
        extraGunsAnim.SetBool("ExtraGuns", true);
        extraGunVisual.SetActive(true);
        StartCoroutine(DeActivateGuns());
        
    }

    IEnumerator DeActivateGuns()
    {
        yield return new WaitForSeconds(gunWaitTime);
        extraGunVisual.SetActive(false);
        extraGunsAnim.SetBool("ExtraGuns", false);
        extraGuns = false;
    }

    public void ActivateExtraShield()
    {
        extraWalls = true;
        shieldTime = DateTime.Now;
        wallAnimator.SetBool("Wall", true);
        wallVisual.SetActive(true);
        StartCoroutine(DeActivateShield());
        
    }

    IEnumerator DeActivateShield()
    {
        yield return new WaitForSeconds(shieldWaitTime);
        wallAnimator.SetBool("Wall", false);
        wallVisual.SetActive(false);
        extraWalls = false;
    }

    public void UpgradeGun()
    {
        if (gunLevel != maxGunLevel)
        {
            gunLevel++;
            currentGun.sprite = gunList[gunLevel];
        }
    }
        
}
