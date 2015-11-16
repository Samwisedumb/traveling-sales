using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP
{
    class Edge
    {
        City startNode;
        City endNode;
        double cost;
        bool visited;
        double probability;

        public Edge(City startNode, City endNode, bool visited)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.visited = visited;
        }

        public Edge() { }
        public City GetStartNode()
        {
            return startNode;
        }
        public void SetStartNode(City temp)
        {
            startNode = temp;
        }
        
        public City GetEndNode()
        {
            return endNode;
        }
        public void SetEndNode(City temp)
        {
            endNode = temp;
        }

        public double GetCost()
        {
            return cost;
        }
        public void SetCost(double temp)
        {
            cost = temp;
        }

        public bool IsVisited()
        {
            return visited;
        }
        public void SetVisited(bool temp)
        {
            visited = temp;
        }

        public double GetProbability()
        {
            return probability;
        }
        public void SetProbability(double temp)
        {
            probability = temp;
        }

    }
}
