namespace TSP
{
    class Edge
    {
        //These are implied
        //private int startCityIndex;
        //private int endCityIndex;
        private double cost;
        //private bool visited;
        private double pheromones;

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

        public double Desirability
        {
            get
            {
                return ((1 / cost) + pheromones);
            }
        }
            
        public double Pheromones
        {
            get
            {
                return pheromones;
            }

            set
           {
                pheromones = value;
            }
        }

        public double CalculateProbability(double totalRowWeight){
            return (Desirability / totalRowWeight);
        }
    }
}