class Pour : Action
{
    public Ingredient Ingredient { get; set; }
    public Pour(Ingredient ing) : base("Пролить") { Ingredient = ing; }
    public override void Execute() => Console.WriteLine($"  Вода пролита через спрессованный кофе");
    public override string ToString() => $"{Name} -> {Ingredient}";
}
