using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentSounds", menuName = "Audio/Environment Sounds")]
public class EnvironmentSounds : ScriptableObject
{
    [Header("Single Sounds")]
    public AudioClip jump;
    public AudioClip hit;
    public AudioClip pickup;

    [Header("Random Groups")]
    public AudioClip[] footsteps;
    public AudioClip[] grassSteps;
    public AudioClip[] rockSteps;
}
