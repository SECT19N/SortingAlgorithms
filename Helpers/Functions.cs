using System;

namespace Sort;

public class Functions {
    public static int[] GenerateArray(int[] arr, int left, int right, int size) {
        Random r = new Random();
        
        for (int i = 0; i < size; i++) {
            arr[i] = r.Next(10000);
        }

        return arr;
    }
}
