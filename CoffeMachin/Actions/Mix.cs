class Mix : Action
{
    public Ingredient Ingredient1 { get; set; }
    public Ingredient Ingredient2 { get; set; }
    public Mix(Ingredient ing1, Ingredient ing2) : base("Перемешать")
    {
        Ingredient1 = ing1;
        Ingredient2 = ing2;
    }
    public override void Execute() => Console.WriteLine($"  {Ingredient1} и {Ingredient2} перемешаны");
    public override string ToString() => $"{Name} -> {Ingredient1} + {Ingredient2}";
}
