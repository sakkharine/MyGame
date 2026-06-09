using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionIndent = 1f;
    [SerializeField] private float interactionWidth = 1f;
    [SerializeField] private float interactionHeight = 1f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    
    [SerializeField] private characterController characterController;
    
    public bool IsInteracting => currentInteractable != null; 
    
    private InteractableWithTimer currentInteractable;
    
    private void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if(MobileInput.Instance == null)
            return;
        MobileInput.Instance.CanInteract = CheckForInteractable();
        
        if (Input.GetKeyDown(interactionKey))
        {
            TryStartInteraction();
        }
        else if (Input.GetKeyUp(interactionKey))
        {
            CancelInteraction();
        }
        
        if (MobileInput.Instance.IsInteracting) TryStartInteraction();
        else CancelInteraction();
    }

    private bool CheckForInteractable()
    {
        Vector2 boxCenter = transform.position + characterController.FaceDirection * interactionIndent;
        Vector2 boxSize = new Vector2(interactionWidth, interactionHeight);
        
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, interactableLayer);

        foreach (Collider2D collider in hitColliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                return true;
            }
        }
        return false;
    }
    
    private void TryStartInteraction()
    {
        if (IsInteracting) return;
        Vector2 boxCenter = transform.position + characterController.FaceDirection * interactionIndent;
        Vector2 boxSize = new Vector2(interactionWidth, interactionHeight);
        
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, interactableLayer);

        foreach (Collider2D collider in hitColliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                StartInteraction(interactable);
                break;
            }
        }
    }

    private void StartInteraction(Interactable interactable)
    {
        if (interactable is InteractableWithTimer cancelableInteraction)
        {
            CancelInteraction();
            currentInteractable = cancelableInteraction;
        
            characterController.Stop();
            characterController.enabled = false;
        
            currentInteractable.OnFinishInteract.AddListener(CancelInteraction);
        }
        
        interactable.Activate();
    }

    private void CancelInteraction()
    {
        if (!IsInteracting) return;

        currentInteractable.Cancel();
        
        if (IsInteracting)
        {
            currentInteractable.OnFinishInteract.RemoveListener(CancelInteraction);
            currentInteractable = null;
            
            characterController.enabled = true;
        }
    }

    private void OnDestroy()
    {
        CancelInteraction();
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 boxCenter = transform.position + characterController.FaceDirection * interactionIndent;
        Vector2 boxSize = new Vector2(interactionWidth, interactionHeight);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
