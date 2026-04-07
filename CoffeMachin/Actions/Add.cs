class Add : Action
{
    public Ingredient Ingredient { get; set; }
    public Add(Ingredient ing) : base("Добавить") { Ingredient = ing; }
    public override void Execute() => Console.WriteLine($"  Добавлен {Ingredient}");
    public override string ToString() => $"{Name} -> {Ingredient}";
}
