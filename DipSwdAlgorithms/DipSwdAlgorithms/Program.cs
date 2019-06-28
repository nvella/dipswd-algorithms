using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DipSwdAlgorithms
{
    class Program
    {
        static string basePath = "C:\\proj\\dipswd-algorithms";

        static void Main(string[] args)
        {
            var numbers = ReadList($"{basePath}\\unsorted_numbers.csv");

            var insertionSorted = InsertionSort(numbers);
            var shellSorted = ShellSort(numbers);

            WriteList($"{basePath}\\insertion_sort.csv", insertionSorted);
            WriteList($"{basePath}\\shell_sort.csv", shellSorted);

            TestSearch("Linear", insertionSorted, LinearSearch);
            TestSearch("Binary", insertionSorted, BinarySearch);
        }

        static int[] ReadList(string filePath) => File.ReadAllLines(filePath).Select(row => int.Parse(row)).ToArray();

        static void WriteList(string filePath, int[] list) => File.WriteAllLines(filePath, list.Select(row => row.ToString()));

        /// <summary>
        /// Insertion sort, implemented from pseudo  at https://en.wikipedia.org/wiki/Insertion_sort
        /// </summary>
        /// <param name="input">Input array</param>
        /// <returns></returns>
        static int[] InsertionSort(int[] input)
        {
            var arr = input.ToArray();
            int i = 1;
            while(i < arr.Length)
            {
                int j = i;
                while(j > 0 && arr[j-1] > arr[j])
                {
                    int a = arr[j];
                    int b = arr[j - 1];

                    arr[j - 1] = a;
                    arr[j] = b;

                    j--;
                }
                i++;
            }
            return arr;
        }

        /// <summary>
        /// Shell sort, implemented from pseudo at https://en.wikipedia.org/wiki/Shellsort
        /// </summary>
        /// <param name="input">Input array</param>
        /// <returns></returns>
        static int[] ShellSort(int[] input)
        {
            // ShellSort relies on a static 'gap' sequence.
            // This one was copied from the sample pseudocode on Wikipedia.
            // Different gap sequences yield different performances based on the dataset.

            int[] gaps = new int[] { 701, 301, 132, 57, 23, 10, 4, 1 };
            var a = input.ToArray();

            foreach (int gap in gaps)
            {
                for(int i = gap; i < a.Length; i++)
                {
                    int temp = a[i];
                    int j;
                    for (j = i; j >= gap && a[j - gap] > temp; j -= gap)
                        a[j] = a[j - gap];
                    a[j] = temp;
                }
            }
            return a;
        }

        static void TestSearch(string searchName, int[] sortedNumbers, Func<int[], int, int> searchFn)
        {
            Console.Write($"Testing {searchName}...");
            var times = new List<double>();
            for(int iteration = 0; iteration < 3; iteration++)
            {
                var startTime = DateTime.Now;

                // Search the largest number
                int res = searchFn(sortedNumbers, sortedNumbers[sortedNumbers.Length - 1]);
                if (res == -1) throw new Exception("search failed");

                // Search every 1500th number
                for (int i = 0; i < sortedNumbers.Length; i += 1500)
                {
                    res = searchFn(sortedNumbers, sortedNumbers[i]);
                    if (res == -1) throw new Exception("search failed");
                }

                var endTime = DateTime.Now;
                times.Add((endTime - startTime).TotalMilliseconds);
            }
            Console.WriteLine($" Avg Time: {times.Average()}ms");
        }

        static int LinearSearch(int[] input, int val)
        {
            for(int i = 0; i < input.Length; i++)
            {
                if (input[i] == val) return i;
            }
            return -1;
        }

        static int BinarySearch(int[] sortedInput, int val)
        {
            // Nick's super cool binary search
            int win = sortedInput.Length;
            int i   = win / 2;
            while(i < sortedInput.Length && sortedInput[i] != val && win > 1)
            {
                win /= 2; // Halve the window
                if (val < sortedInput[i]) i -= win;
                if (sortedInput[i] < val) i += win - 1;
            }

            if (i >= sortedInput.Length || sortedInput[i] != val)
                return -1;
            return i;
        }
    }
}
