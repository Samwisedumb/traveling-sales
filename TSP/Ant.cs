using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP
{
    class Ant
    {
        List<Edge> edges;
        double totalCost;
        public Ant()
        {

        }
        public Ant(List<Edge> edges, double totalCost)
        {
            this.edges = edges;
            this.totalCost = totalCost;
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
