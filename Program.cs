using System;

class TransportProblem
{
    public static (int[,], int) MinElementMethod(int[] supply, int[] demand, int[,] cost)
    {
        int suppliers = supply.Length;
        int consumers = demand.Length;
        int[,] allocation = new int[suppliers, consumers];
        int totalCost = 0;

        while (true)
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
                break; // Если не нашли минимальный элемент, завершаем

            int allocated = Math.Min(supply[minRow], demand[minCol]);
            allocation[minRow, minCol] = allocated;
            totalCost += allocated * cost[minRow, minCol];

            supply[minRow] -= allocated;
            demand[minCol] -= allocated;
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

        var (allocation, totalCost) = MinElementMethod(supply, demand, cost);

        //Вывод матрицы перевозок и общей стоимости
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
