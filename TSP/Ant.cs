using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using System.Diagnostics;

namespace TSP
{
    class Ant
    {
        private const int PHEROMONE_ADJ = 100;
        private const double DECAY_COEFFICIENT = .1;
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

        private double SumOfRowWeights(int startCity){
            double total_weights = 0;
            foreach (int destCity in ValidDestinations(startCity))
                total_weights += costMatrix[startCity, destCity].Desirability;
            return total_weights;
        }

        private int TraverseFrom(int startCity){
            double totalWeight = SumOfRowWeights(startCity);
            double decision = rand.NextDouble();
            foreach (int destCity in ValidDestinations(startCity)){
                double partitionSize = costMatrix[startCity, destCity].CalculateProbability(totalWeight);
                if (decision <= partitionSize){
                    visited[destCity] = true;
                    return destCity;
                }
                decision -= partitionSize;
            }
            return -1;
        }

        private void setTotalCost()
        {
            totalCost = 0;
            for(int i=0; i < route.Count-1; i++)
            {
                totalCost += costMatrix[route[i], route[i + 1]].Cost;
            }
            //add final edge:
            totalCost += costMatrix[route[route.Count - 1], route[0]].Cost;
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

            if (complete)
            {
                setTotalCost();
            }
        }

        private void UpdateFunction(bool included, ref Edge e){
            e.Pheromones = (1 - DECAY_COEFFICIENT) * e.Pheromones + (included ? PHEROMONE_ADJ / totalCost : 0);
        }

        public void updatePheromones(){
                
            for (int i = 0; i < route.Count - 1; i++)
            {
                for (int j = 0; j < route.Count; j++)
                {
                    UpdateFunction(j == route[i + 1], ref costMatrix[route[i], route[j]]);
                }
            }
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

        public void setTotalCost(double val)
        {
            totalCost = val;
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
