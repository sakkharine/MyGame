using UnityEngine;

public static class KarmaCounter
{
    private const string KeyAltWorld  = "Karma_AltWorld";
    private const string KeyRealWorld = "Karma_RealWorld";

    static KarmaCounter()
    {
        ResetAll();
    }

    public static int AltWorldScore  => PlayerPrefs.GetInt(KeyAltWorld,  0);
    public static int RealWorldScore => PlayerPrefs.GetInt(KeyRealWorld, 0);

    public static void AddToAltWorld(int amount = 1)
    {
        int current = PlayerPrefs.GetInt(KeyAltWorld, 0);
        PlayerPrefs.SetInt(KeyAltWorld, current + amount);
        PlayerPrefs.Save();
        Debug.Log($"[KarmaCounter] AltWorld +{amount} → {current + amount}");
    }

    public static void AddToRealWorld(int amount = 1)
    {
        int current = PlayerPrefs.GetInt(KeyRealWorld, 0);
        PlayerPrefs.SetInt(KeyRealWorld, current + amount);
        PlayerPrefs.Save();
        Debug.Log($"[KarmaCounter] RealWorld +{amount} → {current + amount}");
    }

    public static void ResetAll()
    {
        PlayerPrefs.DeleteKey(KeyAltWorld);
        PlayerPrefs.DeleteKey(KeyRealWorld);
        PlayerPrefs.Save();
    }
}
