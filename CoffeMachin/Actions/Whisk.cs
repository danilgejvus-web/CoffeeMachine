class Whisk : Action
{
    public Whisk(params Ingredient[] ingredients) : base("Взбить")
    {
        Elements.AddRange(ingredients);
    }

    public override void Execute()
    {
        Console.WriteLine(ToString());
        foreach (var elem in Elements)
            if (elem is Ingredient ing)
                ing.Name = "Взбитое " + ing.Name;
        Console.WriteLine($"  => {ToString()}");
        foreach (var elem in Elements)
            if (elem is Action act)
                act.Execute();
    }

    public override string ToString() =>
        $"{Name} -> {string.Join(", ", Elements.OfType<Ingredient>())}";
}
