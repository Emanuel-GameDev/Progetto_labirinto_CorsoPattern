public abstract class State
{
    public virtual void OnEnter() { }        // Chiamata quando si entra nello stato
    public virtual void OnUpdate() { }       // Chiamata durante l'Update del gioco
    public virtual void OnExit() { }         // Chiamata quando si esce dello stato
}
