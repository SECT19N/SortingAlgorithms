using System;

namespace Sort {
    class Program {
        static void Main() {
            Console.WriteLine("How many elements should the array hold?");
            int indexes = Convert.ToInt32(Console.ReadLine());
            int[] arr = new int[indexes]; int size = arr.Length;
            int left = 0, right = size - 1;
            arr = Functions.GenerateArray(arr, left, right, size);
            while (true) {
                Console.WriteLine("What sorting algorithm you want?");
                Console.WriteLine("0 - Generate New Array\n1 - Selection\n2 - Bubble\n3 - Quick\n4 - Print Current Array");
                string x = Console.ReadLine().ToLower();
                switch (x) {
                    case "0":
                    case "new": arr = Functions.GenerateArray(arr, left, right, size); break;
                    case "1":
                    case "selection":
                        Sorting select = new Selection();
                        select.Sort(arr, left, right);
                        break;
                    case "2":
                    case "bubble":
                        Sorting bubble = new Bubble();
                        bubble.Sort(arr, left, right);
                        break;
                    case "3":
                    case "quick":
                        Sorting quick = new Quick();
                        DateTime start = DateTime.Now;
                        quick.Sort(arr, left, right);
                        DateTime end = DateTime.Now;
                        TimeSpan totTime = (end - start);
                        Console.WriteLine("Done in {0} ms", totTime.TotalMilliseconds);
                        break;
                    case "4":
                    case "array": Functions.PrintArray(arr); break;
                    default: Console.WriteLine("Wrong input"); break;
                }
            }
        }
    }
}