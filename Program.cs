using System;

class TransportProblem
{
    public static (int[,], int) MinElementMethod(int[] supply, int[] demand, int[,] cost)
    {
        int suppliers = supply.Length; //размер таблицы
        int consumers = demand.Length; //Размер таблицы
        int[,] allocation = new int[suppliers, consumers]; //матрица распределения перевозок
        int totalCost = 0; //общая стоимость перевозок

        while (true) //Цикл выполняется пока есть незаполненные потребности и остатки товаров
        {
            int minCost = int.MaxValue;
            int minRow = -1, minCol = -1;

            // Найти минимальный элемент в таблице тарифов
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

            if (minRow == -1 || minCol == -1)
                break; //При отсутствии доступных ячеек алгоритм завершает работу

            int allocated = Math.Min(supply[minRow], demand[minCol]);  //принимает минимум из поставок и спроса
            allocation[minRow, minCol] = allocated; //Обновление матрицу распределения
            totalCost += allocated * cost[minRow, minCol]; //стоимость перевозки

            supply[minRow] -= allocated;
            demand[minCol] -= allocated;
        }

        return (allocation, totalCost);
    }

    public static (int[,], int) NorthWestCornerMethod(int[] supply, int[] demand, int[,] cost)
    {
        int suppliers = supply.Length;
        int consumers = demand.Length;
        int[,] allocation = new int[suppliers, consumers];
        int totalCost = 0;

        int i = 0, j = 0;
        while (i < suppliers && j < consumers)
        {
            int allocated = Math.Min(supply[i], demand[j]);
            allocation[i, j] = allocated;
            totalCost += allocated * cost[i, j];

            supply[i] -= allocated;
            demand[j] -= allocated;

            if (supply[i] == 0) i++;
            if (demand[j] == 0) j++;
        }

        return (allocation, totalCost);
    }


    static void Main()
    {
        //Количество поставщиков и потребителей
        Console.Write("Введите количество поставщиков: ");
        int suppliers = int.Parse(Console.ReadLine());
        Console.Write("Введите количество потребителей: ");
        int consumers = int.Parse(Console.ReadLine());

        int[] supply = new int[suppliers]; //Поставки
        int[] demand = new int[consumers]; //Спрос

        //Заполнение массива поставок
        Console.WriteLine("Введите объемы поставок:");
        for (int i = 0; i < suppliers; i++)
            supply[i] = int.Parse(Console.ReadLine());

        //Заполнение массива спроса
        Console.WriteLine("Введите объемы спроса:");
        for (int j = 0; j < consumers; j++)
            demand[j] = int.Parse(Console.ReadLine());

        //Ввод тарифного плана
        int[,] cost = new int[suppliers, consumers];
        Console.WriteLine("Введите тарифный план:");
        for (int i = 0; i < suppliers; i++)
        {
            for (int j = 0; j < consumers; j++)
            {
                Console.Write($"Тариф от поставщика {i + 1} к потребителю {j + 1}: ");
                cost[i, j] = int.Parse(Console.ReadLine());
            }
        }

        Console.WriteLine("Выберите метод решения:");
        Console.WriteLine("1 - Метод минимального элемента");
        Console.WriteLine("2 - Метод северо-западного угла");
        int choice = int.Parse(Console.ReadLine());

        (int[,], int) result;
        if (choice == 1)
            result = MinElementMethod(supply, demand, cost);
        else
            result = NorthWestCornerMethod(supply, demand, cost);

        var (allocation, totalCost) = result;

        Console.WriteLine("Распределение ресурсов:");
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
}
