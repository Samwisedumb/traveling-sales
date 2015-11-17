using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TSP
{

    class ProblemAndSolver
    {
        
        private class TSPSolution
        {
            /// <summary>
            /// we use the representation [cityB,cityA,cityC] 
            /// to mean that cityB is the first city in the solution, cityA is the second, cityC is the third 
            /// and the edge from cityC to cityB is the final edge in the path.  
            /// You are, of course, free to use a different representation if it would be more convenient or efficient 
            /// for your node data structure and search algorithm. 
            /// </summary>
            public ArrayList Route;

            public TSPSolution(ArrayList iroute)
            {
                Route = new ArrayList(iroute);
            }


            /// <summary>
            /// Compute the cost of the current route.  
            /// Note: This does not check that the route is complete.
            /// It assumes that the route passes from the last city back to the first city. 
            /// </summary>
            /// <returns></returns>
            public double costOfRoute()
            {
                // go through each edge in the route and add up the cost. 
                int x;
                City here;
                double cost = 0D;

                for (x = 0; x < Route.Count - 1; x++)
                {
                    here = Route[x] as City;
                    cost += here.costToGetTo(Route[x + 1] as City);
                }

                // go from the last city to the first. 
                here = Route[Route.Count - 1] as City;
                cost += here.costToGetTo(Route[0] as City);
                return cost;
            }
        }

        public void print(String sting) {
            System.Diagnostics.Debug.WriteLine(sting);
        }

        public Dictionary<City, List<Edge>> generateMap()
        {
            Dictionary<City, List<Edge>> result = new Dictionary<City, List<Edge>>();
            foreach (City city in Cities)
            {
                List<Edge> edges = new List<Edge>();
                foreach (City compareCity in Cities)
                {
                    double cost = city.costToGetTo(compareCity);
                    if (cost < double.PositiveInfinity)
                    {
                        edges.Add(new Edge(city, compareCity, false, cost));
                    }
                }
                result.Add(city, edges);
            }
            return result;
        }

        public List<Ant> generateAntGroup(int numAnts)
        {
            List<Ant> antGroup = new List<Ant>();

            for (int i=0; i < numAnts; i++)
            {
                antGroup.Add(new Ant());
            }

            return antGroup;
        }

        public Edge pickEdge(List<Edge> edges)
        {
            //TODO: Probabilistically choose an edge from c

            //randomly choose for now:
            Random r = new Random();
            return edges[r.Next(edges.Count)];
        }

        public void antSolve()
        {
            //BASF = Best Ant So Far
            Ant BASF = new Ant(double.PositiveInfinity);

            //generateMap runs in O(v^2)
            Dictionary<City, List<Edge>> map = generateMap();
            
            int numIterations = 1;

            Random random = new Random();
            for (int i=0; i < numIterations; i++)
            {
                //send out ants!
                List<Ant> antGroup = generateAntGroup(10);
                foreach (Ant ant in antGroup)
                {
                    //start from a random city
                    City startCity = Cities[random.Next(Cities.Length)];
                    ant.setCurrentCity(startCity);

                    List<City> visitedCities = new List<City>();
                    visitedCities.Add(startCity);
                    //This while loop is to traverse a path
                    while (ant.GetEdges().Count < Cities.Length-1)
                    {
                        Edge chosenEdge = pickEdge(map[ant.getCurrentCity()]);
                        //This part can for sure be improved
                        if (!visitedCities.Contains(chosenEdge.GetEndNode()))
                        {
                            ant.addEdgeToRoute(chosenEdge);
                            ant.setCurrentCity(chosenEdge.GetEndNode());
                            visitedCities.Add(chosenEdge.GetEndNode());
                        }
                    }
                    //need to add final edge back to start if it exists
                    Boolean edgeFound = false;
                    foreach (Edge edge in map[ant.getCurrentCity()])
                    {
                        if (edge.GetEndNode().Equals(startCity))
                        {
                            edgeFound = true;
                            ant.addEdgeToRoute(edge);
                            ant.setCurrentCity(edge.GetEndNode());
                        }
                    }
                    if(edgeFound == false)
                    {
                        ant.SetTotalCost(double.PositiveInfinity);
                    }
                }

                //see which ant had the shortest cost path
                Ant quickestAnt = new Ant(double.PositiveInfinity);
                foreach (Ant ant in antGroup)
                {
                    if (ant.GetTotalCost() < quickestAnt.GetTotalCost())
                    {
                        quickestAnt = ant;
                    }
                }
                
                //update the quickest ant's edges with increased probability
                foreach (Edge edge in quickestAnt.GetEdges())
                {
                    edge.increaseProbability();
                }

                if(quickestAnt.GetTotalCost() < BASF.GetTotalCost())
                {
                    BASF = quickestAnt;
                }
            }

            // call this the best solution so far.  bssf is the route that will be drawn by the Draw method. 
            Route = new ArrayList(Cities.Length);
            foreach (Edge edge in BASF.GetEdges())
            {
                Route.Add(edge.GetStartNode());
            }
            bssf = new TSPSolution(Route);
            // update the cost of the tour. 
            Program.MainForm.tbCostOfTour.Text = " " + bssf.costOfRoute();
            // do a refresh. 
            Program.MainForm.Invalidate();
        }


        public void greedySolve()
        {
            Route = new ArrayList(Cities.Length);
            HashSet<int> unvisitedIndexes = new HashSet<int>(); // using a city's index in Cities, we can interate through indexes that have yet to be added
            for (int index = 0; index < Cities.Length; index++)
            {
                unvisitedIndexes.Add(index);
            }

            print("\n\nTESTING\n");

            City city;
            for (int i = 0; i < Cities.Length; i++) // keep trying start nodes until a solution is found
            {
                if (Route.Count == Cities.Length)
                {
                    break; // DONE!
                }
                else
                {
                    Route.Clear();
                    for (int index = 0; index < Cities.Length; index++)
                    {
                        unvisitedIndexes.Add(index);
                    }
                    city = Cities[i];
                }

                for (int n = 0; n < Cities.Length; n++) // add nodes n times
                {

                    double shortestDistance = Double.PositiveInfinity;
                    int closestIndex = -1;
                    foreach (int check in unvisitedIndexes) //find the closest city to add to route
                    {
                        double distance = city.costToGetTo(Cities[check]);
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            closestIndex = check;
                        }
                    }

                    if (closestIndex != -1)
                    {
                        city = Cities[closestIndex];
                        Route.Add(city);
                        unvisitedIndexes.Remove(closestIndex);
                    }
                    else
                    {
                        break; // try again
                    }
                }                
            }

            // call this the best solution so far.  bssf is the route that will be drawn by the Draw method. 
            bssf = new TSPSolution(Route);
            // update the cost of the tour. 
            Program.MainForm.tbCostOfTour.Text = " " + bssf.costOfRoute();
            // do a refresh. 
            Program.MainForm.Invalidate();
        }

        #region Private members 

        /// <summary>
        /// Default number of cities (unused -- to set defaults, change the values in the GUI form)
        /// </summary>
        // (This is no longer used -- to set default values, edit the form directly.  Open Form1.cs,
        // click on the Problem Size text box, go to the Properties window (lower right corner), 
        // and change the "Text" value.)
        private const int DEFAULT_SIZE = 25;

        private const int CITY_ICON_SIZE = 5;

        // For normal and hard modes:
        // hard mode only
        private const double FRACTION_OF_PATHS_TO_REMOVE = 0.20;

        /// <summary>
        /// the cities in the current problem.
        /// </summary>
        private City[] Cities;
        /// <summary>
        /// a route through the current problem, useful as a temporary variable. 
        /// </summary>
        private ArrayList Route;
        /// <summary>
        /// best solution so far. 
        /// </summary>
        private TSPSolution bssf; 

        /// <summary>
        /// how to color various things. 
        /// </summary>
        private Brush cityBrushStartStyle;
        private Brush cityBrushStyle;
        private Pen routePenStyle;


        /// <summary>
        /// keep track of the seed value so that the same sequence of problems can be 
        /// regenerated next time the generator is run. 
        /// </summary>
        private int _seed;
        /// <summary>
        /// number of cities to include in a problem. 
        /// </summary>
        private int _size;

        /// <summary>
        /// Difficulty level
        /// </summary>
        private HardMode.Modes _mode;

        /// <summary>
        /// random number generator. 
        /// </summary>
        private Random rnd;
        #endregion

        #region Public members
        public int Size
        {
            get { return _size; }
        }

        public int Seed
        {
            get { return _seed; }
        }
        #endregion

        #region Constructors
        public ProblemAndSolver()
        {
            this._seed = 1; 
            rnd = new Random(1);
            this._size = DEFAULT_SIZE;

            this.resetData();
        }

        public ProblemAndSolver(int seed)
        {
            this._seed = seed;
            rnd = new Random(seed);
            this._size = DEFAULT_SIZE;

            this.resetData();
        }

        public ProblemAndSolver(int seed, int size)
        {
            this._seed = seed;
            this._size = size;
            rnd = new Random(seed); 
            this.resetData();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Reset the problem instance.
        /// </summary>
        private void resetData()
        {

            Cities = new City[_size];
            Route = new ArrayList(_size);
            bssf = null;

            if (_mode == HardMode.Modes.Easy)
            {
                for (int i = 0; i < _size; i++)
                    Cities[i] = new City(rnd.NextDouble(), rnd.NextDouble());
            }
            else // Medium and hard
            {
                for (int i = 0; i < _size; i++)
                    Cities[i] = new City(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble() * City.MAX_ELEVATION);
            }

            HardMode mm = new HardMode(this._mode, this.rnd, Cities);
            if (_mode == HardMode.Modes.Hard)
            {
                int edgesToRemove = (int)(_size * FRACTION_OF_PATHS_TO_REMOVE);
                mm.removePaths(edgesToRemove);
            }
            City.setModeManager(mm);

            cityBrushStyle = new SolidBrush(Color.Black);
            cityBrushStartStyle = new SolidBrush(Color.Red);
            routePenStyle = new Pen(Color.Blue,1);
            routePenStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// make a new problem with the given size.
        /// </summary>
        /// <param name="size">number of cities</param>
        //public void GenerateProblem(int size) // unused
        //{
        //   this.GenerateProblem(size, Modes.Normal);
        //}

        /// <summary>
        /// make a new problem with the given size.
        /// </summary>
        /// <param name="size">number of cities</param>
        public void GenerateProblem(int size, HardMode.Modes mode)
        {
            this._size = size;
            this._mode = mode;
            resetData();
        }

        /// <summary>
        /// return a copy of the cities in this problem. 
        /// </summary>
        /// <returns>array of cities</returns>
        public City[] GetCities()
        {
            City[] retCities = new City[Cities.Length];
            Array.Copy(Cities, retCities, Cities.Length);
            return retCities;
        }

        /// <summary>
        /// draw the cities in the problem.  if the bssf member is defined, then
        /// draw that too. 
        /// </summary>
        /// <param name="g">where to draw the stuff</param>
        public void Draw(Graphics g)
        {
            float width  = g.VisibleClipBounds.Width-45F;
            float height = g.VisibleClipBounds.Height-45F;
            Font labelFont = new Font("Arial", 10);

            // Draw lines
            if (bssf != null)
            {
                // make a list of points. 
                Point[] ps = new Point[bssf.Route.Count];
                int index = 0;
                foreach (City c in bssf.Route)
                {
                    if (index < bssf.Route.Count -1)
                        g.DrawString(" " + index +"("+c.costToGetTo(bssf.Route[index+1]as City)+")", labelFont, cityBrushStartStyle, new PointF((float)c.X * width + 3F, (float)c.Y * height));
                    else 
                        g.DrawString(" " + index +"("+c.costToGetTo(bssf.Route[0]as City)+")", labelFont, cityBrushStartStyle, new PointF((float)c.X * width + 3F, (float)c.Y * height));
                    ps[index++] = new Point((int)(c.X * width) + CITY_ICON_SIZE / 2, (int)(c.Y * height) + CITY_ICON_SIZE / 2);
                }

                if (ps.Length > 0)
                {
                    g.DrawLines(routePenStyle, ps);
                    g.FillEllipse(cityBrushStartStyle, (float)Cities[0].X * width - 1, (float)Cities[0].Y * height - 1, CITY_ICON_SIZE + 2, CITY_ICON_SIZE + 2);
                }

                // draw the last line. 
                g.DrawLine(routePenStyle, ps[0], ps[ps.Length - 1]);
            }

            // Draw city dots
            foreach (City c in Cities)
            {
                g.FillEllipse(cityBrushStyle, (float)c.X * width, (float)c.Y * height, CITY_ICON_SIZE, CITY_ICON_SIZE);
            }

        }

        /// <summary>
        ///  return the cost of the best solution so far. 
        /// </summary>
        /// <returns></returns>
        public double costOfBssf ()
        {
            if (bssf != null)
                return (bssf.costOfRoute());
            else
                return -1D; 
        }

        /// <summary>
        ///  solve the problem.  This is the entry point for the solver when the run button is clicked
        /// right now it just picks a simple solution. 
        /// </summary>
        public void solveProblem()
        {
            int x;
            Route = new ArrayList(); 
            // this is the trivial solution. 
            for (x = 0; x < Cities.Length; x++)
            {
                Route.Add( Cities[Cities.Length - x -1]);
            }
            // call this the best solution so far.  bssf is the route that will be drawn by the Draw method. 
            bssf = new TSPSolution(Route);
            // update the cost of the tour. 
            Program.MainForm.tbCostOfTour.Text = " " + bssf.costOfRoute();
            // do a refresh. 
            Program.MainForm.Invalidate();

        }
        #endregion
    }

}
