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
                break;

            int allocated = Math.Min(supply[minRow], demand[minCol]);
            allocation[minRow, minCol] = allocated;
            totalCost += allocated * cost[minRow, minCol];

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
        Console.Write("Введите количество поставщиков: ");
        int suppliers = int.Parse(Console.ReadLine());
        Console.Write("Введите количество потребителей: ");
        int consumers = int.Parse(Console.ReadLine());

        int[] supply = new int[suppliers];
        int[] demand = new int[consumers];
        int[,] cost = new int[suppliers, consumers];

        Console.WriteLine("Введите объемы поставок:");
        for (int i = 0; i < suppliers; i++)
            supply[i] = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите объемы спроса:");
        for (int j = 0; j < consumers; j++)
            demand[j] = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите тарифный план:");
        for (int i = 0; i < suppliers; i++)
        {
            for (int j = 0; j < consumers; j++)
            {
                Console.Write($"Тариф от поставщика {i + 1} к потребителю {j + 1}: ");
                cost[i, j] = int.Parse(Console.ReadLine());
            }
        }

        var (allocationNW, totalCostNW) = NorthWestCornerMethod((int[])supply.Clone(), (int[])demand.Clone(), cost);
        Console.WriteLine("Метод северо-западного угла:");
        for (int i = 0; i < allocationNW.GetLength(0); i++)
        {
            for (int j = 0; j < allocationNW.GetLength(1); j++)
            {
                Console.Write(allocationNW[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Общая стоимость (Метод северо-западного угла): " + totalCostNW);

        var (allocationMin, totalCostMin) = MinElementMethod((int[])supply.Clone(), (int[])demand.Clone(), cost);
        Console.WriteLine("Метод минимального элемента:");
        for (int i = 0; i < allocationMin.GetLength(0); i++)
        {
            for (int j = 0; j < allocationMin.GetLength(1); j++)
            {
                Console.Write(allocationMin[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Общая стоимость (Метод минимального элемента): " + totalCostMin);
    }
}