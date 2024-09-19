using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static void Shuffle<T>(this List<T> inputList)
    {
        for (int i = 0; i < inputList.Count - 1; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }


    public static void GenerateUniqueRandomInt(this List<int> inputList, int maxNumber, int desiredRandomNumberCount)
    {
        inputList.Clear();

        while (inputList.Count < desiredRandomNumberCount)
        {
            // range is 1 to maxNumber because 0 is the default value
            int intToAdd = Random.Range(0, maxNumber + 1);
            
            if(!inputList.Contains(intToAdd))
                inputList.Add(intToAdd);
        }
        
    }
}