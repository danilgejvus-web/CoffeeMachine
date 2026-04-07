class Drink
{
    public string Name { get; set; }
    public IElement FirstElement { get; set; }
    public Drink(string name) { Name = name; FirstElement = null; }
    public void AddElement(IElement newElement)
    {
        if (FirstElement == null)
            FirstElement = newElement;
        else
        {
            IElement current = FirstElement;
            while (current.Next != null)
                current = current.Next;
            current.Next = newElement;
        }
    }
    public void Show()
    {
        Console.WriteLine($"\nНапиток {Name}:");
        int index = 1;
        IElement current = FirstElement;
        while (current != null)
        {
            Console.WriteLine($"  {index++}. {current}");
            current = current.Next;
        }
    }
    public void Cook()
    {
        Console.WriteLine($"\nПриготовление {Name}:");
        IElement current = FirstElement;
        while (current != null)
        {
            if (current is Action act)
                act.Execute();
            current = current.Next;
        }
        Console.WriteLine($"{Name} готов!\n");
    }
    public List<IElement> GetAllElements()
    {
        List<IElement> list = new List<IElement>();
        IElement current = FirstElement;
        while (current != null)
        {
            list.Add(current);
            current = current.Next;
        }
        return list;
    }
}
