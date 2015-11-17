using System.Collections.Generic;

namespace TSP
{
    class Ant
    {
        private List<Edge> edges;
        private double totalCost;
        private City currentCity;

        public Ant()
        {
            edges = new List<Edge>();
            totalCost = 0;
        }

        public Ant(double totalCost)
        {
            this.totalCost = totalCost;
        }

        public Ant(List<Edge> edges, double totalCost)
        {
            this.edges = edges;
            this.totalCost = totalCost;
        }

        public void addEdgeToRoute(Edge e)
        {
            this.edges.Add(e);
            this.totalCost += e.GetCost();
        }

        public City getCurrentCity()
        {
            return currentCity;
        }

        public void setCurrentCity(City city)
        {
            this.currentCity = city;
        }

        public List<Edge> GetEdges()
        {
            return edges;
        }

        public void SetEdges(List<Edge> temp)
        {
            edges = temp;
        }

        public double GetTotalCost()
        {
            return totalCost;
        }

        public void SetTotalCost(double temp)
        {
            totalCost = temp;
        }
    }
}
