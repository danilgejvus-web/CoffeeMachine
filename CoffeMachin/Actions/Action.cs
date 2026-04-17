abstract class Action : IElement
{
    public string Name { get; protected set; }
    public List<IElement> Elements { get; protected set; } = new List<IElement>();

    protected Action(string name) { Name = name; }

    public abstract void Execute();
}
