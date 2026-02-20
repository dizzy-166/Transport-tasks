class TransportProblem
{
    
    public sealed class TransportData
    {
        public int[] Supply { get; }
        public int[] Demand { get; }
        public int[,] Costs { get; }

        public int SuppliersCount => Supply.Length;
        public int ConsumersCount => Demand.Length;

        public TransportData(int[] supply, int[] demand, int[,] costs)
        {
            Supply = supply ?? throw new ArgumentNullException(nameof(supply));
            Demand = demand ?? throw new ArgumentNullException(nameof(demand));
            Costs = costs ?? throw new ArgumentNullException(nameof(costs));

            Validate();
        }

        private void Validate()
        {
            if (Costs.GetLength(0) != SuppliersCount ||
                Costs.GetLength(1) != ConsumersCount)
            {
                throw new TransportProblemException("Размерность матрицы тарифов некорректна.");
            }

            if (GetTotal(Supply) != GetTotal(Demand))
            {
                throw new TransportProblemException("Задача не является сбалансированной.");
            }
        }

        private int GetTotal(int[] array)
        {
            int sum = 0;

            foreach (int value in array)
            {
                sum += value;
            }

            return sum;
        }
    }

    
    // Результат решения транспортной задачи.
    
    public sealed class TransportResult
    {
        public int[,] Allocation { get; }
        public int TotalCost { get; }

        public TransportResult(int[,] allocation, int totalCost)
        {
            Allocation = allocation;
            TotalCost = totalCost;
        }
    }
   
    // Пользовательское исключение транспортной задачи.
    public sealed class TransportProblemException : Exception
    {
        public TransportProblemException(string message)
            : base(message)
        {
        }
    }

    // Интерфейс стратегии решения транспортной задачи.
    public interface ITransportStrategy
    {
        TransportResult Solve(TransportData data);
    }

    // Метод северо-западного угла.
    public sealed class NorthWestCornerStrategy : ITransportStrategy
    {
        public TransportResult Solve(TransportData data)
        {
            int[] supply = (int[])data.Supply.Clone();
            int[] demand = (int[])data.Demand.Clone();

            int[,] allocation = new int[data.SuppliersCount, data.ConsumersCount];
            int totalCost = 0;

            int i = 0;
            int j = 0;

            while (i < data.SuppliersCount && j < data.ConsumersCount)
            {
                int allocated = Math.Min(supply[i], demand[j]);

                allocation[i, j] = allocated;
                totalCost += allocated * data.Costs[i, j];

                supply[i] -= allocated;
                demand[j] -= allocated;

                if (supply[i] == 0)
                {
                    i++;
                }

                if (j < data.ConsumersCount && demand[j] == 0)
                {
                    j++;
                }
            }

            return new TransportResult(allocation, totalCost);
        }
    }

    // Метод минимального элемента.
    public sealed class MinCostStrategy : ITransportStrategy
    {
        public TransportResult Solve(TransportData data)
        {
            int[] supply = (int[])data.Supply.Clone();
            int[] demand = (int[])data.Demand.Clone();

            int[,] allocation = new int[data.SuppliersCount, data.ConsumersCount];
            int totalCost = 0;

            while (HasOpenCells(supply, demand))
            {
                (int row, int col) = FindMinCostCell(data, supply, demand);

                int allocated = Math.Min(supply[row], demand[col]);

                allocation[row, col] = allocated;
                totalCost += allocated * data.Costs[row, col];

                supply[row] -= allocated;
                demand[col] -= allocated;
            }

            return new TransportResult(allocation, totalCost);
        }

        private bool HasOpenCells(int[] supply, int[] demand)
        {
            foreach (int s in supply)
            {
                if (s > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private (int row, int col) FindMinCostCell(
            TransportData data,
            int[] supply,
            int[] demand)
        {
            int minCost = int.MaxValue;
            int minRow = -1;
            int minCol = -1;

            for (int i = 0; i < data.SuppliersCount; i++)
            {
                if (supply[i] == 0)
                {
                    continue;
                }

                for (int j = 0; j < data.ConsumersCount; j++)
                {
                    if (demand[j] == 0)
                    {
                        continue;
                    }

                    if (data.Costs[i, j] < minCost)
                    {
                        minCost = data.Costs[i, j];
                        minRow = i;
                        minCol = j;
                    }
                }
            }

            if (minRow == -1)
            {
                throw new TransportProblemException("Невозможно найти минимальный тариф.");
            }

            return (minRow, minCol);
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            try
            {
                TransportData data = ReadInput();

                ExecuteStrategy("Метод северо-западного угла",
                    new NorthWestCornerStrategy(),
                    data);

                ExecuteStrategy("Метод минимального элемента",
                    new MinCostStrategy(),
                    data);
            }
            catch (TransportProblemException ex)
            {
                Console.WriteLine($"Ошибка транспортной задачи: {ex.Message}");
            }
        }

        private static TransportData ReadInput()
        {
            Console.Write("Введите количество поставщиков: ");
            int suppliers = int.Parse(Console.ReadLine());

            Console.Write("Введите количество потребителей: ");
            int consumers = int.Parse(Console.ReadLine());

            int[] supply = new int[suppliers];
            int[] demand = new int[consumers];
            int[,] costs = new int[suppliers, consumers];

            Console.WriteLine("Введите объемы поставок:");
            for (int i = 0; i < suppliers; i++)
            {
                supply[i] = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("Введите объемы спроса:");
            for (int j = 0; j < consumers; j++)
            {
                demand[j] = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("Введите тарифный план:");
            for (int i = 0; i < suppliers; i++)
            {
                for (int j = 0; j < consumers; j++)
                {
                    Console.Write($"Тариф [{i + 1},{j + 1}]: ");
                    costs[i, j] = int.Parse(Console.ReadLine());
                }
            }

            return new TransportData(supply, demand, costs);
        }

        private static void ExecuteStrategy(
            string title,
            ITransportStrategy strategy,
            TransportData data)
        {
            TransportResult result = strategy.Solve(data);

            Console.WriteLine(title);

            for (int i = 0; i < result.Allocation.GetLength(0); i++)
            {
                for (int j = 0; j < result.Allocation.GetLength(1); j++)
                {
                    Console.Write(result.Allocation[i, j] + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine($"Общая стоимость: {result.TotalCost}");
            Console.WriteLine();
        }
    }
}
