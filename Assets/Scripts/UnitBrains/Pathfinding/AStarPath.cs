using Model;
using System.Collections.Generic;
using System.Linq;
using UnitBrains.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.UnitBrains.Pathfinding
{
    public partial class AStarPath : BaseUnitPath
    {
        private readonly int[] _dx = new int[] { -1,  0,  1,  1 };
        private readonly int[] _dy = new int[] {  0,  1,  0, -1 };

        public AStarPath(
            IReadOnlyRuntimeModel runtimeModel,
            Vector2Int startPoint,
            Vector2Int endPoint)
            : base(runtimeModel, startPoint, endPoint)
        {
        }

        public int Value { get; private set; } = 0;

        protected override void Calculate()
        {
            path = FindPath();

            if (path is null)
            {
                path = new Vector2Int[] { StartPoint };
            }
        }

        public Vector2Int[] FindPath()
        {
            var startNode = new AStarPathNode(StartPoint);
            var targetNode = new AStarPathNode(EndPoint);

            var openList = new List<AStarPathNode> { startNode };
            var closedList = new List<AStarPathNode>();

            while (openList.Count > 0)
            {
                var currentNode = openList.First();

                foreach (var node in openList)
                {
                    if (node.Value < currentNode.Value)
                    {
                        currentNode = node;
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode.Equals(targetNode))
                {
                    var path = new List<AStarPathNode>();

                    while (currentNode != null)
                    {
                        path.Add(currentNode);
                        currentNode = currentNode.Parent;
                    }

                    path.Reverse();
                    return path.Select(x => x.ToVector2()).ToArray();
                }

                for (int i = 0; i < _dx.Length; i++)
                {
                    var newPoint = new Vector2Int(currentNode.X + _dx[i], currentNode.Y + _dy[i]);

                    if(!IsWalkable(newPoint) && newPoint != EndPoint)
                    {
                        continue;
                    }

                    var neighbour = new AStarPathNode(newPoint);

                    if (closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    neighbour.Parent = currentNode;
                    neighbour.CalculateEstimate(targetNode.X, targetNode.Y);

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }

            return null;
        }

        private bool IsWalkable(Vector2Int point) =>
            runtimeModel.IsTileWalkable(point)
            && point.x > 0
            && point.y > 0
            && point.x < runtimeModel.RoMap.Width
            && point.y < runtimeModel.RoMap.Height
            && !runtimeModel.RoUnits.Any(x => x.Pos == point);
    }
}
