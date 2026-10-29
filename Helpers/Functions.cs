using System;

namespace Sort.Helpers;

public class Functions {
    // Fills arr with random values in 1..maxValue.
    public static int[] GenerateArray(int[] arr, int maxValue) {
        Random r = Random.Shared;

        for (int i = 0; i < arr.Length; i++) {
            arr[i] = r.Next(1, maxValue + 1);
        }

        return arr;
    }
}
