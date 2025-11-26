using UnityEngine;

public class RingRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;
    public float autoAlignSpeed = 250f;

    public bool IsRotating { get; private set; } = false;
    public bool AutoAligning = false;

    [Header("Animator")]
    public Animator anim;

    [Header("Target for Auto Align")]
    public float targetAngle = 0f; 

    public void StartRotation() => IsRotating = true;
    public void StopRotation() => IsRotating = false;

    void Update()
    {
        if (IsRotating)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (AutoAligning)
        {
            AutoAlignToTarget();
        }
    }

    private void AutoAlignToTarget()
    {
        float currentZ = transform.localEulerAngles.z;
        float angle = Mathf.DeltaAngle(currentZ, targetAngle);

        if (Mathf.Abs(angle) < 0.8f)
        {
            transform.localEulerAngles = new Vector3(0, 0, targetAngle);
            AutoAligning = false;
            Debug.Log(name + " докрутился до " + targetAngle + "°");
            return;
        }

        float step = Mathf.Sign(angle) * autoAlignSpeed * Time.deltaTime;
        transform.Rotate(0, 0, step);
    }

    public bool IsInCorrectPosition(float target, float tolerance = 11f)
    {
        float currentZ = transform.localEulerAngles.z;
        float diff = Mathf.Abs(Mathf.DeltaAngle(currentZ, target));
        return diff <= tolerance;
    }

    public void StartRingAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("StartRing");
        }
    }
}
