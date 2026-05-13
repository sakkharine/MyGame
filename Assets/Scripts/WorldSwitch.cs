using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSwitch : MonoBehaviour
{
    [SerializeField] string normalLevelName;
    [SerializeField] string alternativeLevelName;
    [SerializeField] float changeCooldown = 1f;

    bool canChangeWorld = false;

    private void Awake()
    {
        StartCoroutine(WaitCooldown());
    }

    private void Start()
    {
        MobileInput.Instance.CanSwitchWorld = true;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator WaitCooldown()
    {
        yield return new WaitForSeconds(changeCooldown);
        canChangeWorld = true;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWorld();
        }
        
        if (MobileInput.Instance.SwitchWorldDown) SwitchWorld();
    }

    public void SwitchWorld()
    {
        if (!canChangeWorld)
            return;

        string activeLevel = SceneManager.GetActiveScene().name;

        if (normalLevelName == activeLevel)
        {
            SceneManager.LoadScene(alternativeLevelName);
        }
        else if(alternativeLevelName == activeLevel)
        {
            SceneManager.LoadScene(normalLevelName);
        }
        else
        {
            Debug.LogError($"Can't figure what level to start. You have: {normalLevelName} and {alternativeLevelName}, active is {activeLevel}");
        }
    }
}
