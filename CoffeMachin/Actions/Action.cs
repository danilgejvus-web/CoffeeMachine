abstract class Action : IElement
{
    public string Name { get; protected set; }
    public IElement Next { get; set; }
    protected Action(string name) { Name = name; Next = null; }
    public abstract void Execute();
}
