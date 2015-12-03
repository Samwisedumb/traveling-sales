﻿using System.Collections.Generic;
using System.Linq;
using System;

namespace TSP
{
    class Ant
    {
        //private List<Edge> edges;
        private bool[] visited;
        private List<int> route;
        private bool complete;
        private double totalCost = 0;
        private Edge[,] costMatrix;
        private Random rand = new Random();

        private IEnumerable<int> ValidDestinations(int startCity){
            for (int i = 0; i < costMatrix.Length; i++){
                if (!(visited[i] || double.IsInfinity(costMatrix[startCity, i].Cost)))
                    yield return i;
            }
        }
        /*

        private IEnumerable<int> Partition(int startCity, int sumOfCosts){
            foreach (int i in ValidDestinations(startCity))
                yield return sumOfCosts + 
                    (costMatrix[startCity, i].Pheromones - costMatrix[startCity, i].Cost);
        }
        */

        private int BeginTraversal(){
            int startCity = rand.Next(0, costMatrix.Length);
            visited[startCity] = true;
            return startCity;
        }

        private int TraverseFrom(int startCity){
            double sumOfCosts = 0;
            int sumOfPheromones = 0;
            int validCount = 0;
            foreach (int destCity in ValidDestinations(startCity)){
                sumOfCosts += costMatrix[startCity, destCity].Cost;
                sumOfPheromones += costMatrix[startCity, destCity].Pheromones;
                validCount++;
            }
            double maxDecision = (sumOfCosts * (validCount - 1) + sumOfPheromones);
            double decision = rand.NextDouble() * maxDecision;
            foreach (int destCity in ValidDestinations(startCity)){
                double partitionSize = sumOfCosts -
                                    costMatrix[startCity, destCity].Cost +
                                    costMatrix[startCity, destCity].Pheromones;
                if (decision < partitionSize){
                    visited[destCity] = true;
                    return destCity;
                }
                decision -= partitionSize;
            }
            return -1;
        }

        private IEnumerable<int> Travel(){
            int startCity = BeginTraversal();
            int currentCity = startCity;
            int routeLength = 0;
            int nextCity = currentCity;
            while (true)
            {
                //Loop Invariant
                //currentCity != -1 and routeLength < costMatrix.Length - 1
                yield return currentCity;
                nextCity = TraverseFrom(currentCity);
                if (nextCity == -1)
                {
                    if (routeLength == costMatrix.Length - 1 &&
                        !double.IsInfinity(costMatrix[currentCity, startCity].Cost))
                    {
                        totalCost += costMatrix[currentCity, startCity].Cost;
                        yield return startCity;
                    }
                    yield break;
                }
                totalCost += costMatrix[currentCity, nextCity].Cost;
                currentCity = nextCity;
                routeLength++;
            }
        }

        public Ant(ref Edge[,] costMatrix)
        {
            this.costMatrix = costMatrix;
            visited = new bool[costMatrix.Length];
            route = Travel().ToList();
            complete = (route.Count == costMatrix.Length);
        }

        public void decayPheromones(){
            for (int i = 0; i < route.Count - 1; i++)
                costMatrix[route[i], route[i + 1]].DecrementPheromones();
        }

        public void dropPheromones(){
            for (int i = 0; i < route.Count - 1; i++)
                costMatrix[route[i], route[i + 1]].IncrementPheromones();
        }

        public double TotalCost
        {
            get
            {
                if (complete)
                    return totalCost;
                else
                    return double.PositiveInfinity;
            }
        }

        public List<int> Route
        {
            get
            {
                return route;
            }
        }
    }
}
