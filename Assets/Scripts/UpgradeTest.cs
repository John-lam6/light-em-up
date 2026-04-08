using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTest : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Upgrade.Instance.UpgradePlayer();
        }
    }
}
