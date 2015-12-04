using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using System.Diagnostics;

namespace TSP
{
    class Ant
    {
        //private List<Edge> edges;
        private bool[] visited;
        private List<int> route;
        private bool complete = false;
        private double totalCost = 0;
        private Edge[,] costMatrix;
        private Random rand = new Random();

        private IEnumerable<int> ValidDestinations(int startCity)
        {
            for (int i = 0; i < costMatrix.GetLength(0); i++)
            {
                if (!(visited[i] || double.IsInfinity(costMatrix[startCity, i].Cost)))
                    yield return i;
            }
        }

        private int BeginTraversal(){
            int startCity = rand.Next(0, costMatrix.GetLength(0));
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
                if (decision <= partitionSize){
                    visited[destCity] = true;
                    return destCity;
                }
                decision -= partitionSize;
            }
            return -1;
        }
        
        public Ant(ref Edge[,] costMatrix)
        {
            Debug.Assert(costMatrix.GetLength(0) > 0);
            this.costMatrix = costMatrix;
            visited = new bool[costMatrix.GetLength(0)];
            route = new List<int>(20);
            //Generate the route.
            route.Add(BeginTraversal());
            while (route.Count < costMatrix.GetLength(0) && route.Last() != -1)
                route.Add(TraverseFrom(route.Last()));
            complete = route.Last() != -1;
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

        public bool IsComplete
        {
            get
            {
                return complete;   
            }
        }
    }
}
