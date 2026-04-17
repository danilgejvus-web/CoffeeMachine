using System.Text.Json;

class DrinkService
{
    private List<Drink> drinks = new List<Drink>();
    private const string SaveFile = "drinks.json";

    public void Create(Drink d) { drinks.Add(d); Save(); Console.WriteLine($"Создан напиток: {d.Name}"); }
    public void ListAll()
    {
        if (drinks.Count == 0) Console.WriteLine("Нет напитков.");
        else for (int i = 0; i < drinks.Count; i++) Console.WriteLine($"{i + 1}. {drinks[i].Name}");
    }
    public Drink? Get(int idx) => (idx >= 0 && idx < drinks.Count) ? drinks[idx] : null;
    public void UpdateName(int idx, string newName)
    {
        if (idx >= 0 && idx < drinks.Count) { drinks[idx].Name = newName; Save(); Console.WriteLine("Название обновлено."); }
        else Console.WriteLine("Неверный индекс.");
    }
    public void Delete(int idx)
    {
        if (idx >= 0 && idx < drinks.Count) { drinks.RemoveAt(idx); Save(); Console.WriteLine("Напиток удалён."); }
        else Console.WriteLine("Неверный индекс.");
    }
    public int Count => drinks.Count;

    private void Save()
    {
        var saveData = drinks.ConvertAll(DrinkToDto);
        string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SaveFile, json);
    }

    public void Load()
    {
        if (!File.Exists(SaveFile)) return;
        try
        {
            string json = File.ReadAllText(SaveFile);
            var loaded = JsonSerializer.Deserialize<List<DrinkDto>>(json);
            if (loaded == null) return;
            drinks.Clear();
            foreach (var dto in loaded)
                drinks.Add(DrinkFromDto(dto));
        }
        catch (Exception ex) { Console.WriteLine($"Ошибка загрузки: {ex.Message}"); }
    }

    private class IngredientDto
    {
        public string? Type { get; set; }
        public double NetMass { get; set; }
        public string? Flavor { get; set; }
    }

    private class ActionDto
    {
        public string? Type { get; set; }
        public List<IngredientDto> Elements { get; set; } = new List<IngredientDto>();
        public List<ActionDto> Actions { get; set; } = new List<ActionDto>();
    }

    private class DrinkDto
    {
        public string? Name { get; set; }
        public List<ActionDto> Actions { get; set; } = new List<ActionDto>();
    }

    private DrinkDto DrinkToDto(Drink d)
    {
        var dto = new DrinkDto { Name = d.Name };
        if (d.First != null)
            dto.Actions.Add(ActionToDto(d.First));
        return dto;
    }

    private ActionDto ActionToDto(Action act)
    {
        var actDto = new ActionDto { Type = act.GetType().Name };
        foreach (var elem in act.Elements)
        {
            if (elem is Ingredient ing)
                actDto.Elements.Add(IngredientToDto(ing));
            else if (elem is Action nested)
                actDto.Actions.Add(ActionToDto(nested));
        }
        return actDto;
    }

    private Drink DrinkFromDto(DrinkDto dto)
    {
        Drink d = new Drink(dto.Name);
        if (dto.Actions.Count > 0)
            d.First = ActionFromDto(dto.Actions[0]);
        return d;
    }

    private Action? ActionFromDto(ActionDto actDto)
    {
        var ingredients = actDto.Elements
            .Select(IngredientFromDto)
            .Where(i => i != null)
            .Cast<Ingredient>()
            .ToList();
        Action? act = actDto.Type switch
        {
            "Add"   => new Add(ingredients.ToArray()),
            "Boil"  => new Boil(ingredients.ToArray()),
            "Grind" => new Grind(ingredients.ToArray()),
            "Mix"   => new Mix(ingredients.ToArray()),
            "Pour"  => new Pour(ingredients.ToArray()),
            "Whisk" => new Whisk(ingredients.ToArray()),
            _       => null
        };
        if (act != null)
            foreach (var nested in actDto.Actions)
            {
                var nestedAct = ActionFromDto(nested);
                if (nestedAct != null) act.Elements.Add(nestedAct);
            }
        return act;
    }

    private IngredientDto IngredientToDto(Ingredient ing)
    {
        var dto = new IngredientDto { Type = ing.GetType().Name, NetMass = ing.NetMass };
        if (ing is Syrup s) dto.Flavor = s.Flavor;
        return dto;
    }

    private Ingredient? IngredientFromDto(IngredientDto dto) => dto.Type switch
    {
        "Water"      => new Water(dto.NetMass),
        "CoffeeBean" => new CoffeeBean(dto.NetMass),
        "Ice"        => new Ice(dto.NetMass),
        "Milk"       => new Milk(dto.NetMass),
        "Syrup"      => new Syrup(dto.NetMass, dto.Flavor ?? ""),
        _            => null
    };
}
