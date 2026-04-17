class Boil : Action
{
    public Boil(params Ingredient[] ingredients) : base("Вскипятить")
    {
        Elements.AddRange(ingredients);
    }

    public override void Execute()
    {
        Console.WriteLine(ToString());
        foreach (var elem in Elements)
            if (elem is Ingredient ing)
                ing.Name = "Кипяченое " + ing.Name;
        foreach (var elem in Elements)
            if (elem is Action act)
                act.Execute();
    }

    public override string ToString() =>
        $"{Name} -> {string.Join(", ", Elements.OfType<Ingredient>())}";
}
