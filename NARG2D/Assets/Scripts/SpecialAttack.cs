using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private List<KeyCode> attack = new List<KeyCode>();
    private Dictionary<string, List<string>> attackDictionary = new Dictionary<string, List<string>>();
    private MainCharacterController main;
    private int count = 0;
    private int maxCombo = 4;
    void Start()
    {
        List<string> move1 = new List<string>() {" ","A","A"};
        List<string> move2 = new List<string>() {"A","S","A","A"};
        List<string> move3 = new List<string>() {"D","S","A"};
        attackDictionary.Add("Special1", move1);
        attackDictionary.Add("Special2", move2);
        attackDictionary.Add("Special3", move3);
    }

    // Update is called once per frame
    public String CheckSpecial(List<String> list)
    {
        int index = 0;
        bool flag = false;
        if (list.Count > 0)
        {
            foreach (var move in attackDictionary.Keys)
            {
                if (list.Count < attackDictionary[move].Count)
                {
                    flag = false;
                    continue;
                }
                foreach (var key in attackDictionary[move])
                {
                    if (!arePermutation(list[index], key))
                    {
                        Debug.Log("move: " + key + " Input: " + list[index]);
                        flag = false;
                        break;
                    }
                    flag = true;
                    index++;
                }
                if (flag == true)
                {
                    while (index > 0)
                    {
                        list.RemoveAt(0);
                        index--;
                    }
                    return move;
                }
                index = 0;
            }
        }
        index = 1;
        flag = false;
        if (list.Count > 0)
        {
            foreach (var move in attackDictionary.Keys)
            {
                if (list.Count < attackDictionary[move].Count)
                {
                    flag = false;
                    continue;
                }
                foreach (var key in attackDictionary[move])
                {
                    if (index < list.Count)
                    {
                        if (!arePermutation(list[index], key))
                        {
                            Debug.Log("move: " + key + " Input: " + list[index]);
                            flag = false;
                            break;
                        }
                        flag = true;
                        index++;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag == true)
                {
                    while (index > 0)
                    {
                        list.RemoveAt(0);
                        index--;
                    }
                    return move;
                }
                index = 1;
            }
        }
        return null;
    }
    
    static bool arePermutation(String str1, String str2)
    {
        // Get lenghts of both strings
        int n1 = str1.Length;
        int n2 = str2.Length;
 
        // If length of both strings is not same,
        // then they cannot be Permutation
        if (n1 != n2)
            return false;
        char []ch1 = str1.ToCharArray();
        char []ch2 = str2.ToCharArray();
 
        // Sort both strings
        Array.Sort(ch1);
        Array.Sort(ch2);
 
        // Compare sorted strings
        for (int i = 0; i < n1; i++)
            if (ch1[i] != ch2[i])
                return false;
        //Debug.Log("String1: " + str1);
        //Debug.Log("String2: " + str2);
        return true;
    }
    
    private static bool CompareLists(List<KeyCode> aListA, List<KeyCode> aListB)
    {
        if (aListA == null || aListB == null || aListA.Count != aListB.Count)
            return false;
        if (aListA.Count == 0)
            return true;
        Dictionary<KeyCode, int> lookUp = new Dictionary<KeyCode, int>();
        // create index for the first list
        for(int i = 0; i < aListA.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListA[i], out count))
            {
                lookUp.Add(aListA[i], 1);
                continue;
            }
            lookUp[aListA[i]] = count + 1;
        }
        for (int i = 0; i < aListB.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListB[i], out count))
            {
                // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
                return false;
            }
            count--;
            if (count <= 0)
                lookUp.Remove(aListB[i]);
            else
                lookUp[aListB[i]] = count;
        }
        // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
        return lookUp.Count == 0;
    }
}
