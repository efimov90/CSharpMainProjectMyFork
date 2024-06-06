using Model;
using System.Collections.Generic;
using System.Linq;
using UnitBrains.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.UnitBrains.Pathfinding
{
    public partial class AStarPath : BaseUnitPath
    {
        private int MaxLength = 100;

        private Vector2Int[] successors = {
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.right,
        };

        public AStarPath(IReadOnlyRuntimeModel runtimeModel, Vector2Int startPoint, Vector2Int endPoint) : base(runtimeModel, startPoint, endPoint)
        {
            MaxLength = runtimeModel.RoMap.Width * runtimeModel.RoMap.Height;
        }

        protected override void Calculate()
        {
            var route = CalculateAStar(startPoint, endPoint);
            if (route != null)
            {
                var r = getPathFromNode(route);
                r.Reverse();
                path = r.ToArray();
            }
        }

        private List<Vector2Int> getPathFromNode(AStarPathNode node)
        {
            List<Vector2Int> _path = new();
            while (node != null)
            {
                _path.Add(node.ToVector2());
                node = node.Parent;
            }
            return _path;
        }

        private AStarPathNode getBestNode(List<AStarPathNode> frontier)
        {
            if (frontier.Count == 0)
            {
                return null;
            }
            int bestIndex = 0;

            for (int i = 0; i < frontier.Count; i++)
            {
                if (frontier[i].Value < frontier[bestIndex].Value)
                {
                    bestIndex = i;
                }
            }
            AStarPathNode bestNode = frontier[bestIndex];
            frontier.RemoveAt(bestIndex);
            return bestNode;
        }

        private AStarPathNode CalculateAStar(Vector2Int fromPos, Vector2Int toPos)
        {
            AStarPathNode startNode = new(fromPos);
            startNode.CalculateEstimate(toPos.x, toPos.y);
            List<AStarPathNode> frontier = new() { startNode };
            HashSet<Vector2Int> visited = new();
            bool routeFound = false;
            var counter = 0;
            AStarPathNode currentNode = null;

            while (frontier.Count > 0 && counter++ < MaxLength)
            {
                currentNode = getBestNode(frontier);
                if (currentNode == null)
                {
                    break;
                }

                if (routeFound)
                {
                    return currentNode;
                }

                visited.Add(currentNode.ToVector2());

                for (int i = 0; i < successors.Length; i++)
                {
                    Vector2Int s = successors[i];
                    Vector2Int neighborPoint = currentNode.ToVector2() + s;

                    if (visited.Contains(neighborPoint))
                        continue;

                    if (neighborPoint == endPoint)
                    {
                        routeFound = true;
                    }
                    if (IsTileValid(neighborPoint) || routeFound)
                        CalculateNeigborWeights(frontier, currentNode, neighborPoint, endPoint);
                }

            }
            return currentNode;
        }

        private bool IsTileValid(Vector2Int neighborPoint) =>
            neighborPoint.x >= 0
            && neighborPoint.x < runtimeModel.RoMap.Width
            && neighborPoint.y >= 0
            && neighborPoint.y < runtimeModel.RoMap.Height
            && runtimeModel.IsTileWalkable(neighborPoint);

        private void CalculateNeigborWeights(List<AStarPathNode> openList, AStarPathNode currentNode, Vector2Int neighborPoint, Vector2Int endPoint)
        {
            if (!openList.Any(n => n.ToVector2() == neighborPoint))
            {
                var newNode = new AStarPathNode(neighborPoint);
                newNode.CalculateEstimate(endPoint.x, endPoint.y);
                newNode.Parent = currentNode;

                openList.Add(newNode);
            }
        }
    }
}
