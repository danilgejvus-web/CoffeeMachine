class Drink : IElement
{
    public string Name { get; set; }
    public Action? First { get; set; }

    public Drink(string name) { Name = name; }

    public void AddAction(Action action)
    {
        if (First == null)
        {
            First = action;
        }
        else
        {
            Action last = First;
            while (last.Elements.OfType<Action>().Any())
                last = last.Elements.OfType<Action>().Last();
            last.Elements.Add(action);
        }
    }

    public void Show()
    {
        Console.WriteLine($"\nНапиток: {Name}");
        Console.WriteLine("Последовательность действий:");
        ShowActions(First, 1);
    }

    private void ShowActions(Action? action, int index)
    {
        if (action == null) return;
        Console.WriteLine($"  {index}. {action}");
        foreach (var elem in action.Elements)
            if (elem is Action next)
                ShowActions(next, index + 1);
    }

    public void Cook()
    {
        Console.WriteLine($"\nПриготовление {Name}:");
        First?.Execute();
        Console.WriteLine($"\n{Name} готов!");
    }

}
