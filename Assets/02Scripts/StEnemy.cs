using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StEnemy : MonoBehaviour
{
    public Transform bulletPos;
    public GameObject bullet;
    public float bulletSpeed;

    float delay;

    private void FixedUpdate()
    {
        delay += Time.fixedDeltaTime;

        if(delay > 1f)
        {
            GameObject instantBullet = Instantiate(bullet, bulletPos.transform.position, bulletPos.transform.rotation);
            Rigidbody2D rigidBullet = instantBullet.GetComponent<Rigidbody2D>();
            rigidBullet.velocity = bulletPos.right * -bulletSpeed;
            delay = 0f;
        }
    }
}
