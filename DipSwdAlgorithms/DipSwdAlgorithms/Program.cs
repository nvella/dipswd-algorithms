using System;
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

            WriteList($"{basePath}\\insertion_sort.csv", InsertionSort(numbers));
            WriteList($"{basePath}\\shell_sort.csv", ShellSort(numbers));
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
    }
}
