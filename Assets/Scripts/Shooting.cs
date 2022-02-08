﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;

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

    public float bulletForce = 200f;
    
    // Update is called once per frame
    void Update()
    {

        if (reloading && (DateTime.Now - reloadStart).TotalSeconds > reloadTime)
        {
            
            reloading = false;
            animator.SetBool("reloading", false);
        }

        if (!reloading && PlayerScore.bulletsInClip != 30 & ( PlayerScore.bulletsInClip == 0 || Input.GetKeyDown("r")))
        {
            
            PlayerScore.ammoAmount -= (30-PlayerScore.bulletsInClip);
            PlayerScore.bulletsInClip = 30;
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