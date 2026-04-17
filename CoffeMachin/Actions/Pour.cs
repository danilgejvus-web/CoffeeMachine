class Pour : Action
{
    public Pour(params Ingredient[] ingredients) : base("Пролить")
    {
        Elements.AddRange(ingredients);
    }

    public override void Execute()
    {
        Console.WriteLine(ToString());
        foreach (var elem in Elements)
            if (elem is Ingredient ing)
                ing.Name = "Эспрессо";
        Console.WriteLine($"  => {ToString()}");
        foreach (var elem in Elements)
            if (elem is Action act)
                act.Execute();
    }

    public override string ToString() =>
        $"{Name} -> {string.Join(", ", Elements.OfType<Ingredient>())}";
}
