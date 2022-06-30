using System;

namespace Sort {
    public class Selection : Sorting {
        public override void Sort(int[] arr, int left, int right) {
            DateTime start = DateTime.Now;
            for (int i = 0; i < arr.Length - 1; i++) {
                int minIndex = i;
                for (int j = i + 1; j < arr.Length; j++) {
                    if (arr[j] < arr[minIndex]) {
                        minIndex = j;
                    }
                }
                (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
            }
            DateTime end = DateTime.Now;
            TimeSpan totTime = (end - start);
            Console.WriteLine("Done in {0} ms", totTime.TotalMilliseconds);
        }
    }
}