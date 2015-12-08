namespace TSP
{
    class Edge
    {
        private double cost;
        private double pheromones;
        private double cost_factor_weight;
        //The higher this is, the more that edge cost will impact decisions
        private const int COST_FACTOR = 25;
        
        public Edge(double cost)
        {
            this.cost = cost;
            this.cost_factor_weight = System.Math.Pow((1 / cost), COST_FACTOR);
            this.pheromones = 1;
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
                //Pheromones will now have even more of an impact on edge decision
                return (cost_factor_weight * pheromones);
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