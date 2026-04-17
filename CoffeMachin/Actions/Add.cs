class Add : Action
{
    public Add(params Ingredient[] ingredients) : base("Добавить")
    {
        Elements.AddRange(ingredients);
    }

    public override void Execute()
    {
        Console.WriteLine(ToString());
        foreach (var elem in Elements)
            if (elem is Action act)
                act.Execute();
    }

    public override string ToString() =>
        $"{Name} -> {string.Join(", ", Elements.OfType<Ingredient>())}";
}
