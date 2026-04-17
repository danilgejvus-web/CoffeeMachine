using System;
using System.Collections.Generic;

class Program
{
    static DrinkService service = new DrinkService();

    static void Main()
    {
        service.Load();
        while (true)
        {
            Console.WriteLine("\n1. Создать напиток\n2. Список напитков\n3. Показать рецепт\n4. Изменить название\n5. Удалить напиток\n6. Приготовить\n0. Выход");
            string cmd = Console.ReadLine();
            if (cmd == "0") break;
            switch (cmd)
            {
                case "1": CreateDrink(); break;
                case "2": service.ListAll(); break;
                case "3": ShowDrink(); break;
                case "4": UpdateName(); break;
                case "5": DeleteDrink(); break;
                case "6": CookDrink(); break;
                default: Console.WriteLine("Неверный ввод."); break;
            }
        }
    }

    static Ingredient ReadIngredient(string prompt = "Ингредиент")
    {
        while (true)
        {
            Console.Write($"{prompt} (1-Вода, 2-Кофе, 3-Лёд, 4-Сироп, 5-Молоко): ");
            string code = Console.ReadLine();
            Console.Write("Масса (г): ");
            if (!double.TryParse(Console.ReadLine(), out double mass) || mass <= 0)
            {
                Console.WriteLine("Неверная масса."); continue;
            }
            switch (code)
            {
                case "1": return new Water(mass);
                case "2": return new CoffeeBean(mass);
                case "3": return new Ice(mass);
                case "4":
                    Console.Write("Вкус сиропа: ");
                    string flavor = Console.ReadLine() ?? "";
                    return new Syrup(mass, flavor);
                case "5": return new Milk(mass);
                default: Console.WriteLine("Неверный ингредиент."); break;
            }
        }
    }

    static Action? ReadAction()
    {
        Console.Write("Действие (1-Добавить, 2-Вскипятить, 3-Перемолоть, 4-Перемешать, 5-Пролить, 6-Взбить): ");
        string code = Console.ReadLine();

        if (code == "4")
        {
            var ing1 = ReadIngredient("Первый ингредиент");
            var ing2 = ReadIngredient("Второй ингредиент");
            return new Mix(ing1, ing2);
        }

        var ing = ReadIngredient();
        return code switch
        {
            "1" => new Add(ing),
            "2" => new Boil(ing),
            "3" => new Grind(ing),
            "5" => new Pour(ing),
            "6" => new Whisk(ing),
            _   => null
        };
    }

    static void CreateDrink()
    {
        Console.Write("Название напитка: ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Пустое название."); return; }

        Drink d = new Drink(name);
        Console.WriteLine("Добавляйте действия. 0 - закончить.");
        int stepNum = 1;

        while (true)
        {
            Console.WriteLine($"\nДействие {stepNum} (или 0 для завершения):");
            Console.Write("Продолжить? (Enter - да, 0 - нет): ");
            if (Console.ReadLine() == "0") break;

            Action? act = ReadAction();
            if (act == null) { Console.WriteLine("Неверное действие."); continue; }

            d.AddAction(act);
            stepNum++;
        }

        if (d.First != null) service.Create(d);
        else Console.WriteLine("Нет действий, напиток не создан.");
    }

    static void ShowDrink()
    {
        if (service.Count == 0) { Console.WriteLine("Нет напитков."); return; }
        service.ListAll();
        Console.Write("Номер: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= service.Count)
            service.Get(idx - 1)?.Show();
        else Console.WriteLine("Неверный номер.");
    }

    static void UpdateName()
    {
        if (service.Count == 0) { Console.WriteLine("Нет напитков."); return; }
        service.ListAll();
        Console.Write("Номер для изменения: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= service.Count)
        {
            Console.Write("Новое название: ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) service.UpdateName(idx - 1, newName);
            else Console.WriteLine("Пустое название.");
        }
        else Console.WriteLine("Неверный номер.");
    }

    static void DeleteDrink()
    {
        if (service.Count == 0) { Console.WriteLine("Нет напитков."); return; }
        service.ListAll();
        Console.Write("Номер для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= service.Count)
        {
            Console.Write($"Удалить \"{service.Get(idx - 1)?.Name}\"? (д/н): ");
            if (Console.ReadLine()?.ToLower() == "д") service.Delete(idx - 1);
        }
        else Console.WriteLine("Неверный номер.");
    }

    static void CookDrink()
    {
        if (service.Count == 0) { Console.WriteLine("Нет напитков."); return; }
        service.ListAll();
        Console.Write("Номер для приготовления: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= service.Count)
        {
            service.Get(idx - 1)?.Cook();
            service.Load();
        }
        else Console.WriteLine("Неверный номер.");
    }
}
