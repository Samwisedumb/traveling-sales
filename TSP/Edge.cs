namespace TSP
{
    class Edge
    {

        private const int PHEROMONE_WEIGHT = 5;
        //These are implied
        //private int startCityIndex;
        //private int endCityIndex;
        private double cost;
        //private bool visited;
        private int pheromones;

        public Edge(double cost)
        {
            this.cost = cost;
            this.pheromones = 0;
        }

        public double Cost
        {
            get
            {
                return cost;
            }
        }

        public int Pheromones
        {
            get
            {
                return pheromones * PHEROMONE_WEIGHT;
            }
            set
            {
                pheromones = value;
            }
        }
    }
}
