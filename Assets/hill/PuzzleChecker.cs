using UnityEngine;
using UnityEngine.Events;

public class PuzzleChecker : MonoBehaviour
{
    public RingRotator ring1;
    public RingRotator ring2;
    public RingRotator ring3;

    public float targetAngle1;
    public float targetAngle2;
    public float targetAngle3;
    public float tolerance = 5f;

    private bool puzzleSolved = false;

    public UnityEvent OnSolve = new();

    void Update()
    {
        // Проверяем только если головоломка ещё не решена
        if (puzzleSolved) return;

        // Проверяем, что все кольца остановлены
        bool allStopped = !ring1.IsRotating && !ring2.IsRotating && !ring3.IsRotating;

        // Проверяем, что все кольца в нужных позициях
        bool allCorrect = ring1.IsInCorrectPosition(targetAngle1, tolerance) &&
                          ring2.IsInCorrectPosition(targetAngle2, tolerance) &&
                          ring3.IsInCorrectPosition(targetAngle3, tolerance);

        if (allStopped && allCorrect)
        {
            OnSolve.Invoke();
            Debug.Log("✅ Картинка собрана! Все кольца стоят на месте и составляют правильное изображение.");
            // Здесь можно вызвать событие, включить анимацию, открыть дверь и т.д.
        }
    }
}
