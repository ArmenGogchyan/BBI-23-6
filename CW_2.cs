using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

abstract class Task
{
    protected string text = "";
    public string Text
    {
        get => text;
        protected set => text = value;
    }

    public virtual void Resh() { }
    public Task(string text)
    {
        this.text = text;
    }
}

class Task1 : Task
{
    public Task1(string text) : base(text)
    {
    }

    public override void Resh()
    {
        HashSet<char> uniqueLetters = new HashSet<char>();

        foreach (char c in Text)
        {
            if (char.IsLetter(c))
            {
                uniqueLetters.Add(char.ToLower(c));
            }
        }

        Console.WriteLine($"Количество различных букв в слове: {uniqueLetters.Count}");
    }
}

class Task2 : Task
{
    public Task2(string text) : base(text)
    {
    }

    public override void Resh()
    {
        string[] numberStrings = Regex.Split(Text, @"\D+");
        double sum = 0;
        int count = 0;

        foreach (string numberString in numberStrings)
        {
            if (!string.IsNullOrEmpty(numberString))
            {
                if (double.TryParse(numberString, out double number))
                {
                    sum += number;
                    count++;
                }
            }
        }

        double average = count > 0 ? sum / count : 0;

        Console.WriteLine($"Среднее арифметическое всех чисел в тексте: {average}");
    }
}

class JsonIO
{
    public static void Write<T>(T obj, string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            JsonSerializer.Serialize(fs, obj);
        }
    }

    public static T Read<T>(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            return JsonSerializer.Deserialize<T>(fs);
        }
    }
}

class Program
{
    static void Main()
    {
        string text = "Названо по имени Р. Броуна, который открыл явление в 1827 году. Видимые только под микроскопом взвешенные частицы движутся независимо друг от друга и описывают сложные зигзагообразные траектории.";
        Task[] tasks = {
            new Task1(text),
            new Task2(text)
        };

        foreach (var task in tasks)
        {
            task.Resh();
        }

        string path = @"C:\Users\m2308388";
        string folderName = "Answer";
        path = Path.Combine(path, folderName);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName1 = "task_1.json";
        string fileName2 = "task_2.json";

        fileName1 = Path.Combine(path, fileName1);
        fileName2 = Path.Combine(path, fileName2);

        if (!File.Exists(fileName1))
        {
            JsonIO.Write<Task1>(tasks[0] as Task1, fileName1);
        }
        else
        {
            var t1 = JsonIO.Read<Task1>(fileName1);
            Console.WriteLine($"Количество различных букв: {t1}");
        }

        if (!File.Exists(fileName2))
        {
            JsonIO.Write<Task2>(tasks[1] as Task2, fileName2);
        }
        else
        {
            var t2 = JsonIO.Read<Task2>(fileName2);
            Console.WriteLine($"{t2}");
        }
    }
}
