using System;

class TransportTask
{
   static void Main()
    {
        Console.WriteLine("Введите количество поставщиков");
        int supplier = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите количество поставщиков");
        int consumer = int.Parse(Console.ReadLine());

        int[] supple = new int[supplier];
        int[] consum = new int[consumer];

        for(int i = 0; i < supplier; i++)
        {
            Console.WriteLine("Введите объем поставки:");
            supple[i] = int.Parse(Console.ReadLine());
        }
        for (int i = 0; i < consumer; i++)
        {
            Console.WriteLine("Введите объем поставки:");
            consum[i] = int.Parse(Console.ReadLine());
        }
    }
}
