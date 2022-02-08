using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
   
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                var player = other.gameObject;
                player.GetComponent<HealthManager>().Health += 50;
                Destroy(gameObject);
            }
        }


    
}
