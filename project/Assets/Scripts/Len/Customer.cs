﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Customer",
    menuName = "Customer",
    order = 0
    )]
public class Customer : ScriptableObject
{
    #region Fields

    [HideInInspector]
    public static Dictionary<string, Customer> customerLookup =
        new Dictionary<string, Customer>();

    public string customerName;

    public int customerIndex;

    public TeaPreference[] teaPreferences;

    public CondimentPreference[] condimentPreferences;

    [SerializeField]
    private float toleranceTaste;

    [SerializeField]
    private float toleranceStrength;

    [SerializeField]
    private float toleranceTemperature;

    [SerializeField]
    private float weight;

    private float percentile;

    private bool hasVisitedToday;

    #endregion

    #region Properties

    public float ToleranceTaste { get { return toleranceTaste; } }

    public float ToleranceStrength { get { return toleranceStrength; } }

    public float ToleranceTemperature { get { return toleranceTemperature; } }

    #endregion

    #region Functions

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void InitializeAll()
    {
        // Loads all additives in Resources folder.
        Customer[] customers =
            Resources.LoadAll<Customer>("Common/GameData/Customers/");

        // Adds all loaded additives to static additiveList.
        foreach (Customer customer in customers)
        {
            Customer newCustomer = Instantiate(customer);
            // Additive is added to the lookup by it's name.
            customerLookup.Add(customer.customerName, newCustomer);

            // Additive's index is set.
            newCustomer.customerIndex = customerLookup.Count - 1;
        }

        float totalWeight = 0.0f;
        int customersDefinedWeight = 0;

        foreach (Customer customer in customerLookup.Values)
        {
            customer.Initialize();

            if (customer.weight > 0.0f)
            {
                totalWeight += customer.weight;
                customersDefinedWeight++;
            }
        }

        if (totalWeight > 1.0f)
        {
            float normalise = 1.0f / totalWeight;

            foreach (Customer customer in customerLookup.Values)
            {
                customer.weight *= normalise;
            }
        }
        else
        {
            float remainingWeight = 1.0f - totalWeight;
            float remainingWeightAverage = remainingWeight /
                (customerLookup.Count - customersDefinedWeight);

            foreach (Customer customer in customerLookup.Values)
            {
                if (customer.weight == 0.0f)
                {
                    customer.weight = remainingWeightAverage;
                }
            }
        }

        float aggregateWeight = 0.0f;

        foreach (Customer customer in customerLookup.Values)
        {
            aggregateWeight += customer.weight;
            customer.percentile = aggregateWeight;
        }
    }

    public void Initialize()
    {
        // SET UP TEA PREFERENCE LIST
        float teaWeighting = 0.0f;

        foreach (TeaPreference preference in teaPreferences)
        {
            preference.Initialize();
            preference.customerSelected = true;
            teaWeighting += preference.weighting;
        }

        // If total weighting of all tea preferences is more than 1,
        // normalise them all.
        if (teaWeighting > 1.0f)
        {
            float teaWeightingInverse = 1.0f / teaWeighting;

            foreach (TeaPreference preference in teaPreferences)
            {
                preference.weighting *= teaWeightingInverse;
            }

            Array.Sort(teaPreferences, TeaPreference.CompareByWeight);
        }

        // If total weighting is less than 1, fill out rest of
        // preference list with the other teas and share the
        // remaining weight between them.
        else
        {
            // Sort preferences by index.
            Array.Sort(teaPreferences, TeaPreference.CompareByIndex);

            // Get all inclusive list of tea additives.
            Additive[] allTeaAdditives = Additive.GetAllAdditives(Additive.Type.TEA);

            // Calculate average weight of remaining 
            int remainingTeaCount = allTeaAdditives.Length - teaPreferences.Length;
            float remainingWeight = 1.0f - teaWeighting;
            float remainingTeaWeighting = remainingWeight / remainingTeaCount;

            int currentIndex = 0;
            Additive currentTeaAdditive = null;

            List<TeaPreference> bloatedList = new List<TeaPreference>();

            foreach (TeaPreference preference in teaPreferences)
            {
                while (allTeaAdditives[currentIndex].Index < preference.additiveIndex)
                {
                    TeaPreference newPreference = new TeaPreference();
                    currentTeaAdditive = allTeaAdditives[currentIndex];

                    newPreference.teaName = currentTeaAdditive.Name;
                    newPreference.additiveIndex = currentTeaAdditive.Index;
                    newPreference.weighting = remainingTeaWeighting;
                    newPreference.customerSelected = false;

                    bloatedList.Add(newPreference);
                    currentIndex++;
                }

                bloatedList.Add(preference);
                currentIndex++;
            }

            // Create default preferences for remaining tea additives.
            for (int i = currentIndex; i < allTeaAdditives.Length; i++)
            {
                TeaPreference newPreference = new TeaPreference();
                currentTeaAdditive = allTeaAdditives[i];

                newPreference.teaName = currentTeaAdditive.Name;
                newPreference.additiveIndex = currentTeaAdditive.Index;
                newPreference.weighting = remainingTeaWeighting;
                newPreference.customerSelected = false;

                bloatedList.Add(newPreference);
            }

            // After bloated list construction, sort by weight, and replace
            // tea preferences with an array copy.

            teaPreferences = bloatedList.ToArray();
            Array.Sort(teaPreferences, TeaPreference.CompareByWeight);
        }

        // Set up all preference percentiles.
        float teaWeightRunningTotal = 0;

        foreach (TeaPreference preference in teaPreferences)
        {
            teaWeightRunningTotal += preference.weighting;
            preference.percentile = teaWeightRunningTotal;
        }

        // SET UP CONDIMENT PREFERENCE LIST

        float condimentWeighting = 0.0f;

        foreach (CondimentPreference preference in condimentPreferences)
        {
            preference.Initialize();
            preference.customerSelected = true;
            condimentWeighting += preference.weighting;
        }

        // If total weighting of all condiment preferences is more than 1,
        // normalise them all.
        if (condimentWeighting > 1.0f)
        {
            float condimentWeightingInverse = 1.0f / condimentWeighting;

            foreach (CondimentPreference preference in condimentPreferences)
            {
                preference.weighting *= condimentWeightingInverse;
            }

            Array.Sort(condimentPreferences, CondimentPreference.CompareByWeight);
        }

        // If total weighting is less than 1, fill out rest of
        // preference list with the other condiments and share the
        // remaining weight between them.
        else
        {
            // Sort preferences by index.
            Array.Sort(condimentPreferences, CondimentPreference.CompareByIndex);

            // Get all inclusive list of condiment additives.
            Additive[] allCondimentAdditives = Additive.GetAllAdditives(Additive.Type.CONDIMENT);

            // Calculate average weight of remaining 
            int remainingCondimentCount = allCondimentAdditives.Length - condimentPreferences.Length;
            float remainingWeight = 1.0f - condimentWeighting;
            float remainingCondimentWeighting = remainingWeight / remainingCondimentCount;

            int currentIndex = 0;
            Additive currentCondimentAdditive = null;

            List<CondimentPreference> bloatedList = new List<CondimentPreference>();

            foreach (CondimentPreference preference in condimentPreferences)
            {
                while (allCondimentAdditives[currentIndex].Index < preference.additiveIndex)
                {
                    CondimentPreference newPreference = new CondimentPreference();
                    currentCondimentAdditive = allCondimentAdditives[currentIndex];

                    newPreference.condimentName = currentCondimentAdditive.Name;
                    newPreference.additiveIndex = currentCondimentAdditive.Index;
                    newPreference.weighting = remainingCondimentWeighting;
                    newPreference.customerSelected = false;

                    bloatedList.Add(newPreference);
                    currentIndex++;
                }

                bloatedList.Add(preference);
                currentIndex++;
            }

            // Create default preferences for remaining tea additives.
            for (int i = currentIndex; i < allCondimentAdditives.Length; i++)
            {
                CondimentPreference newPreference = new CondimentPreference();
                currentCondimentAdditive = allCondimentAdditives[i];

                newPreference.condimentName = currentCondimentAdditive.Name;
                newPreference.additiveIndex = currentCondimentAdditive.Index;
                newPreference.weighting = remainingCondimentWeighting;
                newPreference.customerSelected = false;

                bloatedList.Add(newPreference);
            }

            // After bloated list construction, sort by weight, and replace
            // condiment preferences with an array copy.

            condimentPreferences = bloatedList.ToArray();
            Array.Sort(condimentPreferences, CondimentPreference.CompareByWeight);
        }

        // Set up all preference percentiles.
        float condimentWeightRunningTotal = 0;

        foreach (CondimentPreference preference in condimentPreferences)
        {
            condimentWeightRunningTotal += preference.weighting;
            preference.percentile = condimentWeightRunningTotal;
        }
    }

    public Order GenerateOrder()
    {
        Order newOrder = new Order(toleranceTaste, toleranceStrength, toleranceTemperature);
        SortedSet<Additive> showOnDocket = new SortedSet<Additive>();
        System.Random randomGenerator = new System.Random();

        Teapot teapot = new Teapot();
        teapot.IsFull = true;
        teapot.Temperature = 0.01f * randomGenerator.Next(70, 100);

        //Choose and add tea
        float teaPercentile = 0.001f * randomGenerator.Next(0, 999);

        for (int i = 0; i < teaPreferences.Length; i++)
        {
            if (teaPercentile < teaPreferences[i].percentile)
            {
                Additive additive = Additive.GetAdditive(teaPreferences[i].additiveIndex);
                teapot.InsertAdditive(additive);

                if (teaPreferences[i].customerSelected)
                {
                    showOnDocket.Add(additive);
                }

                break;
            }
        }

        teapot.Simulate(randomGenerator.Next(0, 10));

        Cup cup = new Cup();

        teapot.DispenseToCup(cup);

        //Choose and add condiment/s
        int condiments = randomGenerator.Next(1, 3);

        for (int i = 0; i < condiments; i++)
        {
            float condimentPercentile = 0.001f * randomGenerator.Next(0, 999);

            for (int j = 0; j < condimentPreferences.Length; j++)
            {
                if (condimentPercentile < condimentPreferences[j].percentile)
                {
                    Additive additive = Additive.GetAdditive(condimentPreferences[j].additiveIndex);
                    cup.InsertAdditive(additive);

                    if (condimentPreferences[j].customerSelected)
                    {
                        showOnDocket.Add(additive);
                    }

                    break;
                }
            }
        }

        cup.Simulate(randomGenerator.Next(0, 10));
        newOrder.SetTarget(cup.Taste, cup.Strength, cup.Temperature);

        foreach (Additive additive in showOnDocket)
        {
            newOrder.additiveRepository.Add(additive);
        }

        return newOrder;
    }

    public static Customer[] GetAllCustomers()
    {
        return customerLookup.Values.ToArray();
    }

    public static Customer[] GetAllUnvisitedCustomers()
    {
        List<Customer> unvisitedCustomers = new List<Customer>();

        foreach (Customer customer in customerLookup.Values)
        {
            if (!customer.hasVisitedToday)
            {
                unvisitedCustomers.Add(customer);
            }
        }

        return unvisitedCustomers.ToArray();
    }

    public static Customer[] GetAllVisitedCustomers()
    {
        List<Customer> visitedCustomers = new List<Customer>();

        foreach (Customer customer in customerLookup.Values)
        {
            if (!customer.hasVisitedToday)
            {
                visitedCustomers.Add(customer);
            }
        }

        return visitedCustomers.ToArray();
    }

    public static void SetAllCustomersToUnvisited()
    {
        foreach (Customer customer in customerLookup.Values)
        {
            customer.hasVisitedToday = false;
        }
    }

    public static Customer GetRandomCustomer()
    {
        System.Random randomGenerator = new System.Random();

        float customerPercentile = 0.001f * randomGenerator.Next(0, 999);

        Customer[] unvisitedCustomers = GetAllUnvisitedCustomers();

        foreach (Customer customer in unvisitedCustomers)
        {
            if (customerPercentile < customer.percentile)
            {
                return customer;
            }
        }

        return null;
    }

    #endregion
}
