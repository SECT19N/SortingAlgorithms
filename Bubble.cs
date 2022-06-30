using System;

namespace Sort {
    public class Bubble : Sorting {
        public override void Sort(int[] arr, int left, int right) {
            int temp;
            DateTime start = DateTime.Now;
            for (int i = 0; i <= arr.Length - 2; i++) {
                for (int j = 0; j <= arr.Length - 2; j++) {
                    if (arr[j] > arr[j + 1]) {
                        temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
            DateTime end = DateTime.Now;
            TimeSpan totTime = (end - start);
            Console.WriteLine("Done in {0} ms", totTime.TotalMilliseconds);
        }
    }
}