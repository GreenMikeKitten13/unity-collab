using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UpgradeAnalyzer : MonoBehaviour
{
    private Dictionary<string, string> upgradeMappings;
    private List<string> validKeywords;

    private void Start()
    {
        // Define upgrade keywords with synonyms
        upgradeMappings = new Dictionary<string, string>()
        {
            { "fire rate|firerate|shoot faster|increase speed", "IncreaseFireRate" },
            { "damage|stronger bullets|power|increase attack", "IncreaseDamage" },
            { "reload speed|faster reload|quick reload", "ReduceReloadTime" },
            { "magazine size|more ammo|bigger mag", "IncreaseMagazine" },
            { "explosive bullets|explosives|boom bullets", "ExplosiveRounds" },
            { "laser|energy beam|plasma shot", "LaserBeamUpgrade" }
        };

        // Store all keywords for typo correction
        validKeywords = new List<string>();
        foreach (var key in upgradeMappings.Keys)
        {
            validKeywords.AddRange(key.Split('|')); // Split synonyms into individual words
        }
    }

    public string AnalyzeUpgradeRequest(string playerInput)
    {
        playerInput = playerInput.ToLower();

        // Check for exact match with Regex
        foreach (var entry in upgradeMappings)
        {
            if (Regex.IsMatch(playerInput, @"\b(" + entry.Key + @")\b"))
            {
                return entry.Value;
            }
        }

        // Try to correct typos using Levenshtein Distance
        string closestMatch = FindClosestMatch(playerInput);
        if (closestMatch != null)
        {
            return upgradeMappings[FindKeyByKeyword(closestMatch)];
        }

        return "InvalidUpgrade"; // No valid upgrade found
    }

    private string FindClosestMatch(string input)
    {
        string bestMatch = null;
        int lowestDistance = 2; // Only allow small typo corrections

        foreach (var word in validKeywords)
        {
            int distance = LevenshteinDistance(input, word);
            if (distance < lowestDistance)
            {
                lowestDistance = distance;
                bestMatch = word;
            }
        }

        return bestMatch;
    }

    private string FindKeyByKeyword(string keyword)
    {
        foreach (var key in upgradeMappings.Keys)
        {
            if (Regex.IsMatch(keyword, @"\b(" + key + @")\b"))
            {
                return key;
            }
        }
        return null;
    }

    // Levenshtein Distance Algorithm (calculates how many edits needed to turn one word into another)
    private int LevenshteinDistance(string a, string b)
    {
        if (string.IsNullOrEmpty(a)) return b.Length;
        if (string.IsNullOrEmpty(b)) return a.Length;

        int[,] costs = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++)
            costs[i, 0] = i;
        for (int j = 0; j <= b.Length; j++)
            costs[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                costs[i, j] = Mathf.Min(
                    Mathf.Min(costs[i - 1, j] + 1, costs[i, j - 1] + 1),
                    costs[i - 1, j - 1] + cost
                );
            }
        }
        return costs[a.Length, b.Length];
    }
}
