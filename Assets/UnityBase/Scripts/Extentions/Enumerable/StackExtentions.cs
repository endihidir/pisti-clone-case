using System.Collections.Generic;

namespace UnityBase.Extensions
{
    public static class StackExtentions
    {
        public static Stack<T> Shuffle<T>(this Stack<T> stack)
        {
            var tempList = new List<T>(stack);
            
            for (int i = tempList.Count - 1; i > 0; i--)
            {
                var j = UnityEngine.Random.Range(0, i + 1);
                
                (tempList[i], tempList[j]) = (tempList[j], tempList[i]);
            }

            return new Stack<T>(tempList);
        }
    }
}