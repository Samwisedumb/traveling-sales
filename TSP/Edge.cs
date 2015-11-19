namespace TSP
{
    class Edge
    {
        private int startCityIndex;
        private int endCityIndex;
        private double cost;
        private bool visited;
        private int pheromones;

        public Edge(int startCityIndex, int endCityIndex, bool visited, double cost)
        {
            this.startCityIndex = startCityIndex;
            this.endCityIndex = endCityIndex;
            this.visited = visited;
            this.cost = cost;
            this.pheromones = 0;
        }

        public Edge() { }

        //public void increaseProbability()
        //{
        //    //TODO: correctly increase probability
        //}

        public int GetStartCityIndex()
        {
            return startCityIndex;
        }
        public void SetStartCityIndex(int temp)
        {
            startCityIndex = temp;
        }
        
        public int GetEndCityIndex()
        {
            return endCityIndex;
        }
        public void SetEndCityIndex(int temp)
        {
            endCityIndex = temp;
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

        public void AddPheromone()
        {
            pheromones++;
        }
        public void RemovePheromone()
        {
            pheromones--;
        }
        public void SetPheromones(int ph)
        {
            pheromones = ph;
        }

    }
}
