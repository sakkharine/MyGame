using System;
using System.Collections.Generic;
using QuestGame;
using UnityEngine;

public class LockedByCollectibles : MonoBehaviour, ILock
{
    [SerializeField] private List<Collectible> collectibles;

    public bool IsLocked
    {
        get
        {
            foreach (var collectible in collectibles)
            {
                if (!collectible.IsCollected) return true;
            }
            return false;
        }
    }
}
