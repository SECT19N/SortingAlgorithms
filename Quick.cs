using System;

namespace Sort {
    public class Quick : Sorting {
        public override void Sort(int[] arr, int left, int right) {
            if (left < right) {
                int pivot = Partition(arr, left, right);
                if (pivot > 1) {
                    Sort(arr, left, pivot - 1);
                }
                if (pivot + 1 < right) {
                    Sort(arr, pivot + 1, right);
                }
            }
        }
        public int Partition(int[] arr, int left, int right) {
            int pivot = arr[right], i = left - 1, temp;
            for (int j = left; j <= right - 1; j++) {
                if (arr[j] <= pivot) {
                    i++;
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
            temp = arr[i + 1];
            arr[i + 1] = arr[right];
            arr[right] = temp;
            return i + 1;
        }
    }
}