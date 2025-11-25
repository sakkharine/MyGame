using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDeactivationTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<StoneBehaviour>(out var stone))
        {
            stone.Deactivate();
        }
    }
}
