class Boil : Action
{
    public Ingredient Ingredient { get; set; }
    public Boil(Ingredient ing) : base("Вскипятить") { Ingredient = ing; }
    public override void Execute() => Console.WriteLine($"  {Ingredient.Name} вскипячена до 95°C");
    public override string ToString() => $"{Name} -> {Ingredient}";
}
