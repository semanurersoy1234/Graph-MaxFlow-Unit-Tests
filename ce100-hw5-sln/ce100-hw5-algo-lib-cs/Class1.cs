/**
* @file ce100-hw5-algo-lib-cs
* @author Sema Nur Ersoy
* @date 27 May 2022
*
* @brief <b> HW-5 Functions </b>
*
* HW-5 Sample Lib Functions
*
* @see http://bilgisayar.mmf.erdogan.edu.tr/en/
*
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ce100_hw5_algo_lib_cs;


namespace ce100_hw5_algo_lib_cs
{
    public class Class1
    {
        public class GraphCycleDetection
        {

            private readonly int V;
            private readonly List<List<int>> adj;

            public GraphCycleDetection(int V)
            {
                this.V = V;
                adj = new List<List<int>>(V);

                for (int i = 0; i < V; i++)
                    adj.Add(new List<int>());
            }
            /**
            * @name IsCyclicUtil
            * @param [in] i [\b int]
            * @param [in] visited [\b bool[]]
            * @param [in] recStack [\b bool[]]
            * @retval [\b bool]
            * Graphs are one of the most versatile data structures.
            * This is because they allow us to solve interesting problems. 
            * They are used in social networks and GPS applications.
            **/
            public bool IsCyclicUtil(int i, bool[] visited, bool[] recStack)
            {

                // Mark the current node as visited and
                // part of recursion stack
                if (recStack[i])
                    return true;

                if (visited[i])
                    return false;

                visited[i] = true;

                recStack[i] = true;
                List<int> children = adj[i];

                foreach (int c in children)
                    if (IsCyclicUtil(c, visited, recStack))
                        return true;

                recStack[i] = false;

                return false;
            }
            /**
            * @name addEdge
            * @param [in] sou [\b int]
            * @param [in] desk [\b int]
            * One can apply it anywhere you want to model the relationship between a bunch of objects.
            * In this article, our primary focus will be on graphs that have a cycle. 
            **/
            public void addEdge(int sou, int dest)
            {
                adj[sou].Add(dest);
            }
            /**
            * @name isCyclic
            * @retval [\b bool]
            * It is important to know this concept to help us detect infinite loops in a computer program.
            * The number of edges in the spanning tree is the subset of the number of edges in the original graph.
            **/
            public bool isCyclic()
            {
                bool[] visited = new bool[V];
                bool[] recStack = new bool[V];

                for (int i = 0; i < V; i++)
                    if (IsCyclicUtil(i, visited, recStack))
                        return true;

                return false;
            }
        }
       
        public class MinimumSpanningTree
        {

            public class Edge : IComparable<Edge>
            {
                public int src, dest, weight;

                public int CompareTo(Edge compareEdge)
                {
                    return this.weight
                           - compareEdge.weight;
                }
            }

            public class subset
            {
                public int parent, rank;
            };

            int V, E; 
            public Edge[] edge;
            /**
            * @name MinimumSpanningTree
            * @param [in] v [\b int]
            * @param [in] e [\b int]
            * Minimum spanning tree is the spanning tree where the cost is minimum among all the spanning trees. 
            * There also can be many minimum spanning trees.
            **/
            public MinimumSpanningTree(int v, int e)
            {
                V = v;
                E = e;
                edge = new Edge[E];
                for (int i = 0; i < e; ++i)
                    edge[i] = new Edge();
            }
            /**
            * @name find
            * @param [in] subsets [\b subset[]]
            * @param [in] i [\b int]
            * @retval [\b int]
            * The cost of the spanning tree is the sum of the weights of all the edges in the tree. 
            * There can be many spanning trees. 
            **/
            int find(subset[] subsets, int i)
            {
              
                if (subsets[i].parent != i)
                    subsets[i].parent
                        = find(subsets, subsets[i].parent);

                return subsets[i].parent;
            }
            /**
            * @name Union
            * @param [in] subsets [\b subset[]]
            * @param [in] x [\b int]
            * @param [in] y [\b int]
            * Minimum spanning tree has direct application in the design of networks.
            * It is used in algorithms approximating the travelling salesman problem.
            **/
            void Union(subset[] subsets, int x, int y)
            {
                int xroot = find(subsets, x);
                int yroot = find(subsets, y);

                if (subsets[xroot].rank < subsets[yroot].rank)
                    subsets[xroot].parent = yroot;
                else if (subsets[xroot].rank > subsets[yroot].rank)
                    subsets[yroot].parent = xroot;

                else
                {
                    subsets[yroot].parent = xroot;
                    subsets[xroot].rank++;
                }
            }
            /**
            * @name KruskalMST
            * @retval [\b string]
            * An algorithm to construct a Minimum Spanning Tree for a connected weighted graph.
            * A minimum spanning tree (MST) or minimum weight spanning tree is a subset of the edges of a connected.
            **/
            public string KruskalMST()
            {
               
                Edge[] result = new Edge[V];
               string mst = "";
                int e = 0; 
                int i
                    = 0; // An index variable, used for sorted edges
                for (i = 0; i < V; ++i)
                    result[i] = new Edge();

                Array.Sort(edge);

                // Allocate memory for creating V subsets
                subset[] subsets = new subset[V];
                for (i = 0; i < V; ++i)
                    subsets[i] = new subset();

                // Create V subsets with single elements
                for (int v = 0; v < V; ++v)
                {
                    subsets[v].parent = v;
                    subsets[v].rank = 0;
                }

                i = 0; // Index used to pick next edge

                // Number of edges to be taken is equal to V-1
                while (e < V - 1)
                {
                    // Step 2: Pick the smallest edge. And increment
                    // the index for next iteration
                    Edge next_edge = new Edge();
                    next_edge = edge[i++];

                    int x = find(subsets, next_edge.src);
                    int y = find(subsets, next_edge.dest);

                    // If including this edge doesn't cause cycle,
                    // include it in result and increment the index
                    // of result for next edge
                    if (x != y)
                    {
                        result[e++] = next_edge;
                        Union(subsets, x, y);
                    }
                    // Else discard the next_edge
                }
                mst+= "(source --> destination : weight)\r\n";
                for (i = 0; i < e; ++i)
                {
                    mst += "(" + result[i].src + " --> " + result[i].dest + " : " + result[i].weight + ")\r\n";
                }

                return mst;
            }
        }
        

        public class SingleSourceShortestPath
        {
            public class Edge
            {
                public int src, dest, weight;
                public Edge()
                {
                    src = dest = weight = 0;
                }
            };

            int V, E;
            public Edge[] edge;

            // Creates a graph with V vertices and E edges
            /**
            * @name SingleSourceShortestPath
            * @param [in] v [\b int]
            * @param [in] e [\b int]
            * The breadth-first- search algorithm is the shortest path algorithm that works on unweighted graphs.
            * The Single-Pair Shortest Path (SPSP) problem consists of finding the shortest path between a single pair of vertices.
            **/
            public SingleSourceShortestPath(int v, int e)
            {
                V = v;
                E = e;
                edge = new Edge[e];
                for (int i = 0; i < e; ++i)
                    edge[i] = new Edge();
            }

            // The main function that finds shortest distances from src
            // to all other vertices using Bellman-Ford algorithm. The
            // function also detects negative weight cycle

            /**
            * @name BellmanFord
            * @param [in] src [\b int[]]
            * @retval [\b string]
            * Negative edge weights are found in various applications of graphs, hence the usefulness of this algorithm.
            * The core of the algorithm is a loop that scans across all edges at every loop.
            **/
            public string BellmanFord(SingleSourceShortestPath graph, int src)
            {
                string print = "";
                int V = graph.V, E = graph.E;
                int[] dist = new int[V];

                // Step 1: Initialize distances from src to all other
                // vertices as INFINITE
                for (int i = 0; i < V; ++i)
                    dist[i] = int.MaxValue;
                dist[src] = 0;

                // Step 2: Relax all edges |V| - 1 times. A simple
                // shortest path from src to any other vertex can
                // have at-most |V| - 1 edges
                for (int i = 1; i < V; ++i)
                {
                    for (int j = 0; j < E; ++j)
                    {
                        int u = graph.edge[j].src;
                        int v = graph.edge[j].dest;
                        int weight = graph.edge[j].weight;
                        if (dist[u] != int.MaxValue && dist[u] + weight < dist[v])
                            dist[v] = dist[u] + weight;
                    }
                }

                for (int j = 0; j < E; ++j)
                {
                    int u = graph.edge[j].src;
                    int v = graph.edge[j].dest;
                    int weight = graph.edge[j].weight;
                   
                }
                print += "(Vertex --> Distance from Source)\r\n";
                for (int i = 0; i < V; ++i)
                {
                    print += "(" + i + " --> " + dist[i] + ")\r\n";
                }
                return print;
            }
        }
        
        public class MaxFlow
        {
            static readonly int V = 6; // Number of vertices in
                                       // graph

            /* Returns true if there is a path
            from source 's' to sink 't' in residual
            graph. Also fills parent[] to store the
            path */
            /**
            * @name bfs
            * @param [in] s [\b int[]]
            * @param [in] t [\b int[]]
            * Flow on an edge doesn’t exceed the given capacity of the edge.
            * Incoming flow is equal to outgoing flow for every vertex except s and t.
            * Given a graph which represents a flow network where every edge has a capacity.
            **/
            bool bfs(int[,] rGraph, int s, int t, int[] parent)
            {
                // Create a visited array and mark
                // all vertices as not visited
                bool[] visited = new bool[V];
                for (int i = 0; i < V; ++i)
                    visited[i] = false;

                // Create a queue, enqueue source vertex and mark
                // source vertex as visited
                List<int> queue = new List<int>();
                queue.Add(s);
                visited[s] = true;
                parent[s] = -1;

                // Standard BFS Loop
                while (queue.Count != 0)
                {
                    int u = queue[0];
                    queue.RemoveAt(0);

                    for (int v = 0; v < V; v++)
                    {
                        if (visited[v] == false
                            && rGraph[u, v] > 0)
                        {
                            // If we find a connection to the sink
                            // node, then there is no point in BFS
                            // anymore We just have to set its parent
                            // and can return true
                            if (v == t)
                            {
                                parent[v] = u;
                                return true;
                            }
                            queue.Add(v);
                            parent[v] = u;
                            visited[v] = true;
                        }
                    }
                }

                // We didn't reach sink in BFS starting from source,
                // so return false
                return false;
            }

            // Returns the maximum flow
            // from s to t in the given graph
            /**
            * @name fordFulkerson
            * @param [in] s [\b int]
            * @param [in] t [\b int]
            * This means that the flow through the network is a legal flow after each round in the algorithm.
            * In the case that the algorithm runs forever, the flow might not even converge towards the maximum flow.
            **/
            public int fordFulkerson(int[,] graph, int s, int t)
            {
                int u, v;

                // Create a residual graph and fill
                // the residual graph with given
                // capacities in the original graph as
                // residual capacities in residual graph

                // Residual graph where rGraph[i,j]
                // indicates residual capacity of
                // edge from i to j (if there is an
                // edge. If rGraph[i,j] is 0, then
                // there is not)
                int[,] rGraph = new int[V, V];

                for (u = 0; u < V; u++)
                    for (v = 0; v < V; v++)
                        rGraph[u, v] = graph[u, v];

                // This array is filled by BFS and to store path
                int[] parent = new int[V];

                int max_flow = 0; // There is no flow initially

                // Augment the flow while there is path from source
                // to sink
                // Returns the maximum flow
                // from s to t in the given graph

                while (bfs(rGraph, s, t, parent))
                {
                    // Find minimum residual capacity of the edhes
                    // along the path filled by BFS. Or we can say
                    // find the maximum flow through the path found.
                    int path_flow = int.MaxValue;
                    for (v = t; v != s; v = parent[v])
                    {
                        u = parent[v];
                        path_flow
                            = Math.Min(path_flow, rGraph[u, v]);
                    }

                    // update residual capacities of the edges and
                    // reverse edges along the path
                    for (v = t; v != s; v = parent[v])
                    {
                        u = parent[v];
                        rGraph[u, v] -= path_flow;
                        rGraph[v, u] += path_flow;
                    }

                    // Add path flow to overall flow
                    max_flow += path_flow;
                }

                // Return the overall flow
                return max_flow;
            }
        }
    }
}





