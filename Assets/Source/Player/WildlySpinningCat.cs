using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildlySpinningCat : MonoBehaviour
{
    public void OnSpawn()
    {
        // Activate the cat
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        // Deactivate the cat
    }

    private void Update()
    {
        // Spin the cat on every axis at a random speed
        transform.Rotate(Random.Range(0, 120) * Time.deltaTime, Random.Range(0, 240) * Time.deltaTime, Random.Range(0, 360) * Time.deltaTime);

    }
}
