using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapestCombinationOfTickets
{
    /*
    http://stackoverflow.com/questions/34340395/how-to-calculate-the-cheapest-combination-of-tickets-for-travel-in-a-given-month?lq=1

You want to buy public transport tickets for the upcoming month. You know exactly the days on which you will be traveling. The month has 30 days and there are three types of ticket:

1-day ticket, costs 2, valid for one day;
7-day ticket, costs 7, valid for seven consecutive days (e.g. if the first valid day is X, then the last valid day is X+6);
30-day ticket, costs 25, valid for all thirty days of the month.
You want to pay as little as possible.

You are given a sorted (in increasing order) array A of dates when you will be traveling. For example, given:

A[0] = 1
A[1] = 2
A[2] = 4
A[3] = 5
A[4] = 7
A[5] = 29
A[6] = 30 
You can buy one 7-day ticket and two 1-day tickets. The two 1-day tickets should be used on days 29 and 30. The 7-day ticket should be used on the first seven days of the month. The total cost is 11 and there is no possible way of paying less.

Write a function:

class Solution { public int solution(int[] a); }
that, given a zero-indexed array A consisting of N integers that specifies days on which you will be traveling, returns the minimum amount of money that you have to spend on tickets for the month.

For example, given the above data, the function should return 11, as explained above.

Assume that:

N is an integer within the range [0..30];
each element of array A is an integer within the range [1..30];
array A is sorted in increasing order;
the elements of A are all distinct.

    */

    public class CheapestCombinationOfTickets
    {
        private const int OneDayTicket = 2;
        private const int WeeklyTicket = 7;
        private const int MonthlyTicket = 25;
        private const int Threshold = 4;//Number of travels within 7 days where it starts to make sense to buy a weekly ticket

        public int Solution(int[] a)
        {
            var resultCache = new Dictionary<string, int>();
            return FindMinAmount(a, 0, a.Length, ref resultCache);
        }

        private int FindMinAmount(int[] days, int start, int end, ref Dictionary<string, int> resultCache)
        {
            if (resultCache.ContainsKey(start + "_" + end))//If there is a cache hit, no need to recalculate
                return resultCache[start + "_" + end];

            if (end - start < Threshold) //If number of travels are smaller than the Threshold then just buy single tickets, skipped caching here...
                return (end - start) * OneDayTicket;

            //Options: All single tickets, a monthly ticket or some 7 day tickets. We will pick the smallest one.
            int minTotal = (end - start) * OneDayTicket;
            if (MonthlyTicket < minTotal)
                minTotal = MonthlyTicket;

            //This is where the recursion happens. We will take each of the travel days in order and buy a 7 day ticket on that day and calculate
            //[minimum amount for the previous days] + [7 day ticket amount] + [minimum amount for the days after the 7 day ticket expires]
            //Finally, take the min of these
            for (int i = 0; i < end - start; i++)
            {
                int total = 0;

                total += FindMinAmount(days, start, start + i, ref resultCache);//[minimum amount for the previous days]

                total += WeeklyTicket; //[7 day ticket amount]
                int countToSeven = 0; //itarete the travel days until the 7 day ticket expires
                while (start + i + countToSeven < end && days[start + i + countToSeven] - days[start + i] < 7 && countToSeven < 7)
                {
                    countToSeven++;
                }

                total += FindMinAmount(days, start + i + countToSeven, end, ref resultCache);//[minimum amount for the days after the 7 day ticket expires]

                if (total < minTotal)
                    minTotal = total;
            }

            resultCache.Add(start + "_" + end, minTotal);//Cache the result
            return minTotal;
        }

        public void TestRun()
        {
            Console.WriteLine(Solution(new[] { 1, 3, 5, 7, 8, 9, 10 }));
            Console.WriteLine(Solution(new[] { 1, 2, 3, 4, 5, 6, 7, 15, 16, 17, 18, 19, 20, 21 }));
            Console.WriteLine(Solution(Enumerable.Range(1, 30).ToArray()));
            Console.WriteLine(Solution(Enumerable.Range(1, 23).ToArray()));
            Console.WriteLine(Solution(Enumerable.Range(1, 22).ToArray()));
            Console.WriteLine(Solution(Enumerable.Range(1, 21).ToArray()));
            Console.WriteLine(Solution(Enumerable.Range(1, 30).Where(i => i % 2 == 0).ToArray()));

            Console.WriteLine(Solution(new[] { 1, 2, 20, 21, 23, 26 }));
        }
    }

}
