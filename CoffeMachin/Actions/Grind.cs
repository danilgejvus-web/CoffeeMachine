class Grind : Action
{
    public Grind(params Ingredient[] ingredients) : base("Перемолоть")
    {
        Elements.AddRange(ingredients);
    }

    public override void Execute()
    {
        Console.WriteLine(ToString());
        foreach (var elem in Elements)
            if (elem is Ingredient ing)
                ing.Name = "Молотое " + ing.Name;
        Console.WriteLine($"  => {ToString()}");
        foreach (var elem in Elements)
            if (elem is Action act)
                act.Execute();
    }

    public override string ToString() =>
        $"{Name} -> {string.Join(", ", Elements.OfType<Ingredient>())}";
}
