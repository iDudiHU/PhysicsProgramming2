public interface IInteractable
{
    public InteractionType Type { get; }
    public string InteractionPrompt { get; }
    public bool Interact(Interactor interactor);
}

public enum InteractionType
{
    None,
    Press,
    Hold
}

