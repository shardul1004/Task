using System;

namespace Palindrome
{
    internal class Program
    {
        public static bool IsPalindrome(char[] str, int start, int end)
        {
            while (start < end)
            {
                if (str[start] != str[end])
                {
                    return false;
                }
                start++;
                end--;
            }
            return true;
        }

        public static void ReverseString(char[] str, int start, int end)
        {
            while (start < end)
            {
                char temp = str[start];
                str[start] = str[end];
                str[end] = temp;
                start++;
                end--;
            }
        }

        static void Main(string[] args)
        {
            char[] s = Console.ReadLine().Trim().ToCharArray();
            int q = int.Parse(Console.ReadLine());

            for (int query = 0; query < q; query++)
            {
                string[] inputs = Console.ReadLine().Split();
                int i = int.Parse(inputs[0]) - 1;
                int j = int.Parse(inputs[1]) - 1;
                int k = int.Parse(inputs[2]) - 1;
                int l = int.Parse(inputs[3]) - 1;

                char[] tempArray = (char[])s.Clone();

                ReverseString(tempArray, i, j);

                if (IsPalindrome(tempArray, k, l))
                {
                    Console.WriteLine("Yes");
                }
                else
                {
                    Console.WriteLine("No");
                }
            }

            Console.ReadKey();
        }
    }
}