abstract class Ingredient : IElement
{
    public string Name { get; protected set; }
    public double NetMass { get; set; }
    public IElement Next { get; set; }
    protected Ingredient(string name, double mass) { Name = name; NetMass = mass; Next = null; }
    public override string ToString() => $"{Name} ({NetMass}г)";
}
