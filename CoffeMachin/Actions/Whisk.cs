class Whisk : Action
{
    public Ingredient Ingredient { get; set; }
    public Whisk(Ingredient ing) : base("Взбить") { Ingredient = ing; }
    public override void Execute() => Console.WriteLine($"  {Ingredient.Name} взбито в пену");
    public override string ToString() => $"{Name} -> {Ingredient}";
}
