using System;
using System.Collections.Generic;
using System.IO;
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
    public Drink Get(int idx) => (idx >= 0 && idx < drinks.Count) ? drinks[idx] : null;
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
        var saveData = new List<DrinkDto>();
        foreach (var d in drinks)
            saveData.Add(DrinkToDto(d));

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
        public string Type { get; set; }
        public double NetMass { get; set; }
        public string Flavor { get; set; }
    }

    private class ActionDto
    {
        public string Type { get; set; }
        public IngredientDto Ingredient { get; set; }
        public IngredientDto Ingredient1 { get; set; }
        public IngredientDto Ingredient2 { get; set; }
    }

    private class ElementDto
    {
        public string Kind { get; set; }
        public ActionDto Action { get; set; }
        public IngredientDto Ingredient { get; set; }
    }

    private class DrinkDto
    {
        public string Name { get; set; }
        public List<ElementDto> Elements { get; set; }
    }

    private DrinkDto DrinkToDto(Drink d)
    {
        var elements = new List<ElementDto>();
        var current = d.FirstElement;
        while (current != null)
        {
            if (current is Action act)
            {
                var actDto = new ActionDto { Type = act.GetType().Name };
                if (act is Add a) actDto.Ingredient = IngredientToDto(a.Ingredient);
                else if (act is Boil b) actDto.Ingredient = IngredientToDto(b.Ingredient);
                else if (act is Grind g) actDto.Ingredient = IngredientToDto(g.Ingredient);
                else if (act is Mix m)
                {
                    actDto.Ingredient1 = IngredientToDto(m.Ingredient1);
                    actDto.Ingredient2 = IngredientToDto(m.Ingredient2);
                }
                else if (act is Pour p) actDto.Ingredient = IngredientToDto(p.Ingredient);
                else if (act is Whisk w) actDto.Ingredient = IngredientToDto(w.Ingredient);
                elements.Add(new ElementDto { Kind = "Action", Action = actDto });
            }
            else if (current is Ingredient ing)
            {
                elements.Add(new ElementDto { Kind = "Ingredient", Ingredient = IngredientToDto(ing) });
            }
            current = current.Next;
        }
        return new DrinkDto { Name = d.Name, Elements = elements };
    }

    private Drink DrinkFromDto(DrinkDto dto)
    {
        Drink d = new Drink(dto.Name);
        IElement prev = null;
        foreach (var elemDto in dto.Elements)
        {
            IElement elem = null;
            if (elemDto.Kind == "Action")
            {
                var act = elemDto.Action;
                switch (act.Type)
                {
                    case "Add": elem = new Add(IngredientFromDto(act.Ingredient)); break;
                    case "Boil": elem = new Boil(IngredientFromDto(act.Ingredient)); break;
                    case "Grind": elem = new Grind(IngredientFromDto(act.Ingredient)); break;
                    case "Mix": elem = new Mix(IngredientFromDto(act.Ingredient1), IngredientFromDto(act.Ingredient2)); break;
                    case "Pour": elem = new Pour(IngredientFromDto(act.Ingredient)); break;
                    case "Whisk": elem = new Whisk(IngredientFromDto(act.Ingredient)); break;
                }
            }
            else if (elemDto.Kind == "Ingredient")
            {
                elem = IngredientFromDto(elemDto.Ingredient);
            }
            if (elem != null)
            {
                if (prev == null) d.FirstElement = elem;
                else prev.Next = elem;
                prev = elem;
            }
        }
        return d;
    }

    private IngredientDto IngredientToDto(Ingredient ing)
    {
        var dto = new IngredientDto { Type = ing.GetType().Name, NetMass = ing.NetMass };
        if (ing is Syrup s) dto.Flavor = s.Flavor;
        return dto;
    }

    private Ingredient IngredientFromDto(IngredientDto dto)
    {
        switch (dto.Type)
        {
            case "Water": return new Water(dto.NetMass);
            case "CoffeeBean": return new CoffeeBean(dto.NetMass);
            case "Ice": return new Ice(dto.NetMass);
            case "Milk": return new Milk(dto.NetMass);
            case "Syrup": return new Syrup(dto.NetMass, dto.Flavor);
            default: return null;
        }
    }
}