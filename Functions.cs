using System;

namespace Sort {
    public class Functions {
        public static void PrintArray(int[] arr) {
            Console.WriteLine("Output:");
            foreach (int p in arr) {
                Console.Write("{0}\t", p.ToString("N0"));
            }
            Console.WriteLine();
        }
        public static int[] GenerateArray(int[] arr, int left, int right, int size) {
            Random r = new Random();
            for (int i = 0; i < size; i++) {
                arr[i] = r.Next(10000);
            }
            return arr;
        }
    }
}