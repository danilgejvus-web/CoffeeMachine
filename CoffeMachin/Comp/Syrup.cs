class Syrup : Ingredient
{
    public string Flavor { get; set; }
    public Syrup(double m, string f) : base("Сироп", m) { Flavor = f; }
    public override string ToString() => $"{Name} {Flavor} ({NetMass}г)";
}
