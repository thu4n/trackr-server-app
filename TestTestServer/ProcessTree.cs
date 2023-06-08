using System;
using System.Runtime.CompilerServices;

namespace TestTestServer
{
    public class ProcessTree
    {
        public  int NO_PARENT = -1;
        public  int check = 0;
        public int[] node = new int[19];
        public int distance = 0;
        public  void dijkstra(int startVertex, int endVertex)
        {
            int nVertices = adjacencyMatrix.GetLength(0);
            int[] shortestDistances = new int[nVertices];
            bool[] added = new bool[nVertices];

            for (int vertexIndex = 0; vertexIndex < nVertices;
                                                vertexIndex++)
            {
                shortestDistances[vertexIndex] = int.MaxValue;
                added[vertexIndex] = false;
            }
            shortestDistances[startVertex] = 0;

            int[] parents = new int[nVertices];

            parents[startVertex] = NO_PARENT;

            for (int i = 1; i < nVertices; i++)
            {

                int nearestVertex = -1;
                int shortestDistance = int.MaxValue;
                for (int vertexIndex = 0;
                        vertexIndex < nVertices;
                        vertexIndex++)
                {
                    if (!added[vertexIndex] &&
                        shortestDistances[vertexIndex] <
                        shortestDistance)
                    {
                        nearestVertex = vertexIndex;
                        shortestDistance = shortestDistances[vertexIndex];
                    }
                }
                added[nearestVertex] = true;

                for (int vertexIndex = 0;
                        vertexIndex < nVertices;
                        vertexIndex++)
                {
                    int edgeDistance = adjacencyMatrix[nearestVertex, vertexIndex];

                    if (edgeDistance > 0
                        && ((shortestDistance + edgeDistance) <
                            shortestDistances[vertexIndex]))
                    {
                        parents[vertexIndex] = nearestVertex;
                        shortestDistances[vertexIndex] = shortestDistance +
                                                        edgeDistance;
                    }
                }
            }
            printSolution(startVertex, shortestDistances, parents, endVertex);
        }
     
        public  void printSolution(int startVertex,
                                        int[] distances,
                                        int[] parents, int endVertex)
        {
            int nVertices = distances.Length;

            for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
            {
                if (vertexIndex != startVertex && vertexIndex == endVertex)
                {
                    // Console.Write(distances[vertexIndex] + "\t\t");
                    distance = distances[vertexIndex];
                    printPath(vertexIndex, parents);
                }
            }
        }
        // In đường đi
        public  void printPath(int currentVertex,
                                    int[] parents)
        {
            if (currentVertex == NO_PARENT)
            {
                return;
            }
            printPath(parents[currentVertex], parents);
            node[check++] = currentVertex;
        }
        public int Location()
        {
            return check;
        }
        public int Distance()
        {
            return distance;
        }
        public int[] Tree()
        {
            return node;
        }
      
        private static int[,] adjacencyMatrix = { {0, 8, 2, 3, 6, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
{8, 0, 0, 8, 0, 0, 10, 0, 5, 0, 0, 0, 0, 11, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0},
{2, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
{3, 8, 0, 0, 6, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
{6, 0, 0, 6, 0, 3, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
{0, 0, 0, 0, 3, 0, 0, 4, 0, 0, 3, 0, 8, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0},
{0, 10, 0, 7, 0, 0, 0, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 5, 52},
{0, 0, 0, 0, 0, 4, 11, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 25, 0, 0},
{0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0},
{0, 0, 3, 0, 5, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0},
{0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 0, 6, 0, 0, 12, 24, 9, 25, 0, 0, 0},
{0, 0, 0, 0, 0, 8, 0, 12, 0, 0, 0, 21, 0, 0, 0, 0, 0, 0, 0, 22, 0, 0, 0, 0},
{3, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 3, 0, 0, 5, 0, 0, 0, 0, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 5, 0, 6, 8, 0, 0, 0, 0, 0, 0, 0},
{4, 0, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 3, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 8, 0, 0, 2, 0, 0, 0, 0, 0, 0},
{0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
{0, 5, 0, 0, 0, 0, 0, 0, 5, 0, 0, 24, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 22, 0, 0, 0, 0, 0, 0, 0, 16, 0, 0, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0},
{0, 0, 0, 0, 0, 0, 20, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 0},
{0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 0, 47},
{0, 0, 0, 0, 0, 0, 52, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 47, 0} };
    }
}
