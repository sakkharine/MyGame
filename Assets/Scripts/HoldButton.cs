using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : Button
{
    public bool IsHeld { get; private set; }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        IsHeld = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        IsHeld = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        IsHeld = false;
    }
}
