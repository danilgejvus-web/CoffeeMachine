using System;
using System.Collections.Generic;
using System.IO;

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
    static void CreateDrink()
    {
        Console.Write("Название напитка: ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Пустое название."); return; }
        Drink d = new Drink(name);
        Console.WriteLine("Добавляйте элементы (действие или ингредиент). 0 - закончить.");
        int stepNum = 1;
        while (true)
        {
            Console.WriteLine($"\nЭлемент {stepNum}:");
            Console.Write("Тип (1-Действие, 2-Ингредиент) или 0: ");
            string typeChoice = Console.ReadLine();
            if (typeChoice == "0") break;
            if (typeChoice == "1")
            {
                Console.Write("Действие (1-Добавить,2-Вскипятить,3-Перемолоть,4-Перемешать,5-Пролить,6-Взбить): ");
                string actCode = Console.ReadLine();
                if (actCode == "4")
                {
                    Console.Write("Первый ингредиент (1-Вода,2-Кофе,3-Лёд,4-Сироп,5-Молоко): ");
                    string ingCode1 = Console.ReadLine();
                    Console.Write("Масса первого (г): ");
                    if (!double.TryParse(Console.ReadLine(), out double mass1) || mass1 <= 0) { Console.WriteLine("Неверная масса."); continue; }
                    Ingredient ing1 = null;
                    switch (ingCode1)
                    {
                        case "1": ing1 = new Water(mass1); break;
                        case "2": ing1 = new CoffeeBean(mass1); break;
                        case "3": ing1 = new Ice(mass1); break;
                        case "4":
                            Console.Write("Вкус сиропа: ");
                            string flavor1 = Console.ReadLine();
                            ing1 = new Syrup(mass1, flavor1);
                            break;
                        case "5": ing1 = new Milk(mass1); break;
                        default: Console.WriteLine("Неверный ингредиент."); continue;
                    }
                    Console.Write("Второй ингредиент (1-Вода,2-Кофе,3-Лёд,4-Сироп,5-Молоко): ");
                    string ingCode2 = Console.ReadLine();
                    Console.Write("Масса второго (г): ");
                    if (!double.TryParse(Console.ReadLine(), out double mass2) || mass2 <= 0) { Console.WriteLine("Неверная масса."); continue; }
                    Ingredient ing2 = null;
                    switch (ingCode2)
                    {
                        case "1": ing2 = new Water(mass2); break;
                        case "2": ing2 = new CoffeeBean(mass2); break;
                        case "3": ing2 = new Ice(mass2); break;
                        case "4":
                            Console.Write("Вкус сиропа: ");
                            string flavor2 = Console.ReadLine();
                            ing2 = new Syrup(mass2, flavor2);
                            break;
                        case "5": ing2 = new Milk(mass2); break;
                        default: Console.WriteLine("Неверный ингредиент."); continue;
                    }
                    d.AddElement(new Mix(ing1, ing2));
                }
                else
                {
                    Console.Write("Ингредиент (1-Вода,2-Кофе,3-Лёд,4-Сироп,5-Молоко): ");
                    string ingCode = Console.ReadLine();
                    Console.Write("Масса (г): ");
                    if (!double.TryParse(Console.ReadLine(), out double mass) || mass <= 0) { Console.WriteLine("Неверная масса."); continue; }
                    Ingredient ing = null;
                    switch (ingCode)
                    {
                        case "1": ing = new Water(mass); break;
                        case "2": ing = new CoffeeBean(mass); break;
                        case "3": ing = new Ice(mass); break;
                        case "4":
                            Console.Write("Вкус сиропа: ");
                            string flavor = Console.ReadLine();
                            ing = new Syrup(mass, flavor);
                            break;
                        case "5": ing = new Milk(mass); break;
                        default: Console.WriteLine("Неверный ингредиент."); continue;
                    }
                    Action act = null;
                    switch (actCode)
                    {
                        case "1": act = new Add(ing); break;
                        case "2": act = new Boil(ing); break;
                        case "3": act = new Grind(ing); break;
                        case "5": act = new Pour(ing); break;
                        case "6": act = new Whisk(ing); break;
                        default: Console.WriteLine("Неверное действие."); continue;
                    }
                    d.AddElement(act);
                }
            }
            else if (typeChoice == "2")
            {
                Console.Write("Ингредиент (1-Вода,2-Кофе,3-Лёд,4-Сироп,5-Молоко): ");
                string ingCode = Console.ReadLine();
                Console.Write("Масса (г): ");
                if (!double.TryParse(Console.ReadLine(), out double mass) || mass <= 0) { Console.WriteLine("Неверная масса."); continue; }
                Ingredient ing = null;
                switch (ingCode)
                {
                    case "1": ing = new Water(mass); break;
                    case "2": ing = new CoffeeBean(mass); break;
                    case "3": ing = new Ice(mass); break;
                    case "4":
                        Console.Write("Вкус сиропа: ");
                        string flavor = Console.ReadLine();
                        ing = new Syrup(mass, flavor);
                        break;
                    case "5": ing = new Milk(mass); break;
                    default: Console.WriteLine("Неверный ингредиент."); continue;
                }
                d.AddElement(ing);
            }
            else
            {
                Console.WriteLine("Неверный тип.");
                continue;
            }
            stepNum++;
        }
        if (d.FirstElement != null) service.Create(d);
        else Console.WriteLine("Нет элементов, напиток не создан.");
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
            service.Get(idx - 1)?.Cook();
        else Console.WriteLine("Неверный номер.");
    }
}