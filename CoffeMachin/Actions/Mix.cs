class Mix : Action
{
    public Mix(Ingredient ing1, Ingredient ing2) : base("Перемешать")
    {
        Elements.Add(ing1);
        Elements.Add(ing2);
    }

    public override void Execute()
    {
        Console.WriteLine(ToString());
        var ingredients = Elements.OfType<Ingredient>().ToList();
        double totalMass = ingredients.Sum(i => i.NetMass);
        var syrup = ingredients.OfType<Syrup>().FirstOrDefault();
        var result = ingredients.FirstOrDefault(i => i is not Syrup) ?? ingredients[0];
        result.Name = syrup != null
            ? $"{result.Name} {syrup.Flavor}"
            : string.Join(" + ", ingredients.Select(i => i.Name));
        result.NetMass = totalMass;

        var actions = Elements.OfType<Action>().Cast<IElement>().ToList();
        Elements.Clear();
        Elements.Add(result);
        Elements.AddRange(actions);

        var res = Elements.OfType<Ingredient>().FirstOrDefault();
        if (res != null)
            Console.WriteLine($"  => {res}");
        foreach (var elem in Elements)
            if (elem is Action act)
                act.Execute();
    }

    public override string ToString() =>
        $"{Name} -> {string.Join(", ", Elements.OfType<Ingredient>())}";
}
