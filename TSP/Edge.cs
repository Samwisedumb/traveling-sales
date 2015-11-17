namespace TSP
{
    class Edge
    {
        private City startNode;
        private City endNode;
        private double cost;
        private bool visited;
        private double probability;

        public Edge(City startNode, City endNode, bool visited, double cost)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.visited = visited;
            this.cost = cost;
        }

        public Edge() { }

        public void increaseProbability()
        {
            //TODO: correctly increase probability
        }

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
