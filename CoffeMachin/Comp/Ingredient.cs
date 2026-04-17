abstract class Ingredient : IElement
{
    public string Name { get; set; }
    public double NetMass { get; set; }
    protected Ingredient(string name, double mass) { Name = name; NetMass = mass; }
    public override string ToString() => $"{Name} ({NetMass}г)";
}
