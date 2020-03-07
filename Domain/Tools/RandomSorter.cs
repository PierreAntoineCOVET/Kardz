using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Tools
{
    public static class RandomSorter
    {
        public static IEnumerable<T> Randomize<T>(IEnumerable<T> list)
        {
            var buffer = list.ToList();
            var shuffledItems = new List<T>(buffer.Count);
            var random = new Random();

            for (int i = 0; i < list.Count(); i++)
            {
                var randomCardIndex = random.Next(0, buffer.Count);
                shuffledItems.Add(buffer.ElementAt(randomCardIndex));
                buffer.RemoveAt(randomCardIndex);
            }

            return shuffledItems;
        }
    }
}
