class TransportProblem
{
    // Метод минимального элемента для решения транспортной задачи
    public static void MinElementMethod(int[] supply, int[] demand, int[,] cost)
    {
        int suppliers = supply.Length;   // Количество поставщиков
        int consumers = demand.Length;   // Количество потребителей
        int[,] allocation = new int[suppliers, consumers]; // Матрица распределения
        int totalCost = 0; // Общая стоимость перевозки

        while (true)
        {
            int minCost = int.MaxValue; // Минимальная стоимость среди доступных тарифов
            int minRow = -1, minCol = -1; // Координаты минимального элемента

            // Поиск минимального тарифа среди всех доступных (где еще есть запасы и спрос)
            for (int i = 0; i < suppliers; i++)
            {
                for (int j = 0; j < consumers; j++)
                {
                    if (cost[i, j] < minCost && supply[i] > 0 && demand[j] > 0)
                    {
                        minCost = cost[i, j];
                        minRow = i;
                        minCol = j;
                    }
                }
            }

            // Если не найден минимальный тариф (все поставки распределены) - цикл завершится
            if (minRow == -1 || minCol == -1)
                break;

            // Вычисляем объем поставки (минимум из доступного запаса и спроса)
            int allocated = Math.Min(supply[minRow], demand[minCol]);
            allocation[minRow, minCol] = allocated;
            totalCost += allocated * cost[minRow, minCol]; // Учитываем стоимость перевозки

            // Уменьшаем доступный запас и спрос
            supply[minRow] -= allocated;
            demand[minCol] -= allocated;
        }

        // Вывод результата
        Console.WriteLine("Метод минимального элемента:");
        for (int i = 0; i < allocation.GetLength(0); i++)
        {
            for (int j = 0; j < allocation.GetLength(1); j++)
            {
                Console.Write(allocation[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Общая стоимость: " + totalCost);
    }

    // Метод северо-западного угла для решения транспортной задачи
    public static void NorthWestCornerMethod(int[] supply, int[] demand, int[,] cost)
    {
        int suppliers = supply.Length;
        int consumers = demand.Length;
        int[,] allocation = new int[suppliers, consumers]; // Матрица распределения
        int totalCost = 0; // Общая стоимость перевозки

        int i = 0, j = 0; // С верхнего левого угла
        while (i < suppliers && j < consumers)
        {
            // Определяем, сколько можно поставить в текущую ячейку
            int allocated = Math.Min(supply[i], demand[j]);
            allocation[i, j] = allocated;
            totalCost += allocated * cost[i, j]; // Учитываем стоимость

            // Уменьшаем доступный запас и спрос
            supply[i] -= allocated;
            demand[j] -= allocated;

            // Если запас исчерпан — переходим к следующему поставщику
            if (supply[i] == 0) i++;
            // Если спрос исчерпан — переходим к следующему потребителю
            if (demand[j] == 0) j++;
        }

        // Вывод результата
        Console.WriteLine("Метод северо-западного угла:");
        for (int i2 = 0; i2 < allocation.GetLength(0); i2++)
        {
            for (int j2 = 0; j2 < allocation.GetLength(1); j2++)
            {
                Console.Write(allocation[i2, j2] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Общая стоимость: " + totalCost);
    }

    static void Main()
    {
        // Ввод данных
        Console.Write("Введите количество поставщиков: ");
        int suppliers = int.Parse(Console.ReadLine());
        Console.Write("Введите количество потребителей: ");
        int consumers = int.Parse(Console.ReadLine());

        int[] supply = new int[suppliers];  // Поставки
        int[] demand = new int[consumers];  // Спрос
        int[,] cost = new int[suppliers, consumers]; // Матрица тарифов

        // Ввод объемов поставок
        Console.WriteLine("Введите объемы поставок:");
        for (int i = 0; i < suppliers; i++)
            supply[i] = int.Parse(Console.ReadLine());

        // Ввод объемов спроса
        Console.WriteLine("Введите объемы спроса:");
        for (int j = 0; j < consumers; j++)
            demand[j] = int.Parse(Console.ReadLine());

        // Ввод тарифного плана
        Console.WriteLine("Введите тарифный план:");
        for (int i = 0; i < suppliers; i++)
        {
            for (int j = 0; j < consumers; j++)
            {
                Console.Write($"Тариф от поставщика {i + 1} к потребителю {j + 1}: ");
                cost[i, j] = int.Parse(Console.ReadLine());
            }
        }

        // Решение методом северо-западного угла
        NorthWestCornerMethod((int[])supply.Clone(), (int[])demand.Clone(), cost);

        // Решение методом минимального элемента
        MinElementMethod((int[])supply.Clone(), (int[])demand.Clone(), cost);
    }
}
