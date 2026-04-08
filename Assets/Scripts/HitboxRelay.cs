using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxRelay : MonoBehaviour
{
    [SerializeField] private SwordHitbox swordHitbox;

    public void Activate() => swordHitbox.Activate();
    public void Deactivate() => swordHitbox.Deactivate();
}
