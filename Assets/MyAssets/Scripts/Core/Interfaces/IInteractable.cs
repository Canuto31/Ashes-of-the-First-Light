/// <summary>
/// Defines the contract implemented by iinteractable objects.
/// </summary>
public interface IInteractable
{

    #region Contract

    void Interact();
    string GetInteractionText();

    #endregion
}
