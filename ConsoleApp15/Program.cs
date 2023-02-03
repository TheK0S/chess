using System.Text.RegularExpressions;

int[,] desk = 
{
    {1,5,1,5,1,5,1,5},
    {5,1,5,1,5,1,5,1},
    {1,5,1,5,1,5,1,5},
    {5,0,5,0,5,0,5,0},
    {0,5,0,5,0,5,0,5},
    {5,2,5,2,5,2,5,2},
    {2,5,2,5,2,5,2,5},
    {5,2,5,2,5,2,5,2}
};

int counter = 0;

while (true)
{
    Console.Clear();
    ShowDesk(desk);
    if(counter < 2)
        Console.WriteLine("\nMove example: a3-b4");
    
    if(counter % 2 == 0)
        Console.Write("\nWhite move: ");
    else
        Console.Write("\nBlack move: ");

    var s = Console.ReadLine(); //ввод строки хода в формате d2-e3
    s = counter % 2 == 0 ? s?.Insert(0, "w") : s?.Insert(0, "b");
        //Разбор
        var m = Regex.Match(s ?? "", "([w|b])([a-h])([1-8])-([a-h])([1-8])");
    if (!m.Success)
        continue;

    var fromX = m.Groups[2].Value[0] - 'a';
    var fromY = m.Groups[3].Value[0] - '1';
    var toX = m.Groups[4].Value[0] - 'a';
    var toY = m.Groups[5].Value[0] - '1';
    var color = m.Groups[1].Value[0] == 'w' ? 1 : 2;

    //Предварительная проверка хода
    if (desk[fromY, fromX] != color && desk[fromY, fromX] != color + 2)
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
        Console.WriteLine("Target field is wrong!");
        Console.ReadKey();
        continue;
    }

    //Ход с проверкой
    if (MoveCheck(desk, fromY, fromX, toY, toX, desk[fromY, fromX] > 2))
    {
        if (IsBecomeQueen(toY, desk[fromY, fromX]))//Проверка на переход в дамки
            desk[toY, toX] = desk[fromY, fromX] + 2;
        else
            desk[toY, toX] = desk[fromY, fromX];

        desk[fromY, fromX] = 0;
    }
    else
    {
        Console.WriteLine("Wrong move!");
        Console.ReadKey();
        continue;
    }

    int GameStatus = IsGameOver(desk);

    if(GameStatus == 1)
    {
        Console.WriteLine("Game over! White win!");
        break;
    }
    else if(GameStatus == 2)
    {
        Console.WriteLine("Game over! Black win!");
        break;
    }

    counter++;
}

static bool IsBecomeQueen(int row, int value)//Проверка на переход в дамки
{
    if (value == 1 && row == 7)
        return true;
    else if (value == 2 && row == 0)
        return true;
    else
        return false;
}

static bool MoveCheck(int[,] arr, int fromY, int fromX, int toY, int toX, bool isQueen = false)
{
    int diffX = fromX > toX ? fromX - toX : toX - fromX;
    int diffY = fromY > toY ? fromY - toY : toY - fromY;

    int killX = fromX > toX ? fromX - 1 : toX - 1;
    int killY = fromY > toY ? fromY - 1 : toY - 1;

    if (diffX == 1 && diffY == 1)//Если ход со смещением на 1 клетку
    {
        if (!isQueen && arr[fromY, fromX] == 1 && fromY > toY)//Проверка на ход назад для белых
            return false;

        if (!isQueen && arr[fromY, fromX] == 2 && fromY < toY)//Проверка на ход назад для черных
            return false;

        return true;
    } 
    //Проверка боя для обычной шашки
    else if (!isQueen && diffX == 2 && diffY == 2 && arr[killY, killX] != arr[fromY, fromX] && arr[killY, killX] != arr[fromY, fromX] + 2)
    {
        arr[killY, killX] = 0;
        return true;
    }
    //Проверка боя и перемещения для дамки
    else if (isQueen && diffX == diffY)
    {
        if (fromY < toY && fromX < toX)
        {
            for (int y = fromY + 1, x = fromX + 1; y < toY && x < toX; y++, x++)
            {
                if (arr[y, x] == arr[fromY, fromX] || arr[y, x] == arr[fromY, fromX] - 2)
                    return false;
                else
                    arr[y, x] = 0;
            }
        }

        if (fromY > toY && fromX > toX)
        {
            for (int y = fromY - 1, x = fromX - 1; y > toY && x > toX; y--, x--)
            {
                if (arr[y, x] == arr[fromY, fromX] || arr[y, x] == arr[fromY, fromX] - 2)
                    return false;
                else
                    arr[y, x] = 0;
            }
        }

        if (fromY > toY && fromX < toX)
        {
            for (int y = fromY - 1, x = fromX + 1; y > toY && x < toX; y--, x++)
            {
                if (arr[y, x] == arr[fromY, fromX] || arr[y, x] == arr[fromY, fromX] - 2)
                    return false;
                else
                    arr[y, x] = 0;
            }
        }

        if (fromY < toY && fromX > toX)
        {
            for (int y = fromY + 1, x = fromX - 1; y < toY && x > toX; y++, x--)
            {
                if (arr[y, x] == arr[fromY, fromX] || arr[y, x] == arr[fromY, fromX] - 2)
                    return false;
                else
                    arr[y, x] = 0;
            }
        }

        return true;
    }

    else
        return false;
}

static int IsGameOver(int[,] arr)
{
    int whiteCount = 0;
    int blackCount = 0;

    foreach (var item in arr)
    {
        if (item == 1 || item == 3)
            whiteCount++;

        if(item == 2 || item == 4)
            blackCount++;
    }
    if (whiteCount > 0 && blackCount == 0)
        return 1;

    else if (whiteCount == 0 && blackCount > 0)
        return 2;

    else 
        return 0;
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
