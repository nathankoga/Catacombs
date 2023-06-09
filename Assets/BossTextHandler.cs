using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTextHandler : MonoBehaviour
{
    public GameObject objBossKilled;
    public GameObject objMustKillBoss;

    float timer = 0.0f;

    public void BossKilled()
    {
        print("BossKilled");
        objBossKilled.SetActive(true);
        objMustKillBoss.SetActive(false);
        timer = 3.0f;
    }

    public void MustKillBoss()
    {
        print("MustKillBoss");
        objBossKilled.SetActive(false);
        objMustKillBoss.SetActive(true);
        timer = 3.0f;
    }

    private void Update()
    {
        if (timer <= 0)
        {
            objBossKilled.SetActive(false);
            objMustKillBoss.SetActive(false);
            return;
        }
        timer -= Time.deltaTime;
    }
}
