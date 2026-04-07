class Grind : Action
{
    public Ingredient Ingredient { get; set; }
    public Grind(Ingredient ing) : base("Перемолоть") { Ingredient = ing; }
    public override void Execute() => Console.WriteLine($"  {Ingredient.Name} перемолото");
    public override string ToString() => $"{Name} -> {Ingredient}";
}
