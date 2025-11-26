using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    [Header("Rings")]
    public RingRotator ring1;
    public RingRotator ring2;
    public RingRotator ring3;

    [Header("Target Angles")]
    public float targetAngle1;
    public float targetAngle2;
    public float targetAngle3;
    public float tolerance = 25f;

    [Header("Second Object")]
    public Animator secondObjectAnimator; // ссылка на Animator второго объекта
    public string secondObjectTrigger = "StartSecondAnim"; // триггер второй анимации

    private bool puzzleSolved = false;
    private bool alignPhase = false;
    private bool firstAnimationStarted = false;
    private bool secondAnimationStarted = false;

    void Update()
    {
        if (!puzzleSolved)
        {
            CheckPuzzleSolved();
        }
        else if (alignPhase)
        {
            CheckAlignCompleted();
        }
    }

    private void CheckPuzzleSolved()
    {
        bool allStopped = !ring1.IsRotating && !ring2.IsRotating && !ring3.IsRotating;
        bool allCorrect =
            ring1.IsInCorrectPosition(targetAngle1, tolerance) &&
            ring2.IsInCorrectPosition(targetAngle2, tolerance) &&
            ring3.IsInCorrectPosition(targetAngle3, tolerance);

        if (allStopped && allCorrect)
        {
            puzzleSolved = true;

            ring1.targetAngle = targetAngle1;
            ring2.targetAngle = targetAngle2;
            ring3.targetAngle = targetAngle3;

            ring1.AutoAligning = true;
            ring2.AutoAligning = true;
            ring3.AutoAligning = true;

            alignPhase = true;
        }
    }

    private void CheckAlignCompleted()
    {
        bool allAligned = !ring1.AutoAligning && !ring2.AutoAligning && !ring3.AutoAligning;

        if (allAligned && !firstAnimationStarted)
        {
            firstAnimationStarted = true;
            ring1.StartRingAnimation();
            ring2.StartRingAnimation();
            ring3.StartRingAnimation();
        }
    }

    // Этот метод вызывается через Animation Event в конце первой анимации кольца
    public void FirstAnimationFinished()
    {
        if (!secondAnimationStarted && secondObjectAnimator != null)
        {
            secondAnimationStarted = true;
            secondObjectAnimator.SetTrigger(secondObjectTrigger);
        }
    }
}
