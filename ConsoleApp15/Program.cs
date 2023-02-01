using System.Text.RegularExpressions;

int[,] desk = {
                          {1,5,1,5,1,5,1,5},
                          {5,1,5,1,5,1,5,1},
                          {1,5,1,5,1,5,1,5},
                          {5,0,5,0,5,0,5,0},
                          {0,5,0,5,0,5,0,5},
                          {5,2,5,2,5,2,5,2},
                          {2,5,2,5,2,5,2,5},
                          {5,2,5,2,5,2,5,2}
                          };


while (true)
{
    Console.Clear();
    ShowDesk(desk);
    Console.Write("\nEnter your move: ");
    var s = Console.ReadLine(); //ввод строки хода в формате wd2-e3   w-white

    //Разбор
    var m = Regex.Match(s ?? "", "([w|b])([a-h])([1-8])-([a-h])([1-8])");
    if (!m.Success)
        continue;

    var fromX = m.Groups[2].Value[0] - 'a';
    var fromY = m.Groups[3].Value[0] - '1';
    var toX = m.Groups[4].Value[0] - 'a';
    var toY = m.Groups[5].Value[0] - '1';
    var color = m.Groups[1].Value[0] == 'w' ? 1 : 2;

    //Проверка хода
    if (desk[fromY, fromX] != color)
    {
        Console.WriteLine("There is not your draught!");
        Console.ReadKey();
        continue;
    }

    if (desk[toY, toX] != 0)
    {
        Console.WriteLine("Target field is not empty!");
        Console.ReadKey();
        continue;
    }

    if (desk[toY, toX] == 5)
    {
        Console.WriteLine("Wrong move!");
        Console.ReadKey();
        continue;
    }
    //Ход
    desk[fromY, fromX] = 0;
    desk[toY, toX] = color;
}

//метод отображения доски
static void ShowDesk(int[,] desk)
{
    char[] chars = new[] { ' ', '☻', '☺', 'w', 'b', '░' };
    for (int i = 7; i >= 0; i--)
    {
        Console.Write((i + 1) + " ");

        for (int j = 0; j < 8; j++)
            Console.Write(chars[desk[i, j]]);

        Console.WriteLine();
    }
    Console.WriteLine("  abcdefgh");
}