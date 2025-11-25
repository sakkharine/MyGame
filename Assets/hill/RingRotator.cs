using UnityEngine;

public class RingRotator : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private bool isRotating = false;
    
    public bool IsRotating => isRotating; // 👈 публичное свойство для проверки

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating)
        {
            Debug.Log("ROtating");
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    public bool IsInCorrectPosition(float targetAngle, float tolerance = 11f)
    {
        float currentZ = transform.localEulerAngles.z;
        float diff = Mathf.Abs(Mathf.DeltaAngle(currentZ, targetAngle));
        return diff <= tolerance;
    }
}
