using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    private float timeToDestroy = 1f;

    private void Update()
    {
        timeToDestroy -= Time.deltaTime;
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Terrain")
        {
            Destroy(this.gameObject);
        }
    }

}
