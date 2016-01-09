using System.Collections.Generic;
using System.Linq;
using CoveoBlitz;

namespace Coveo.Bot
{
    public class Pathfinder
    {

        public GameState gameInfo;
        private PathfinderTile[,] globalMap;
        private List<PathfinderTile> openList = new List<PathfinderTile>();
        private List<PathfinderTile> closedList = new List<PathfinderTile>();

        public Pathfinder(GameState _gameInfo)
        {
            int sizeX = gameInfo.board.GetLength(0);
            int sizeY = gameInfo.board.GetLength(1);

            gameInfo = _gameInfo;

            globalMap = new PathfinderTile[sizeX, sizeY];

            //Initialise pathfinding board
            for (int i = 0; i < gameInfo.board.GetLength(0); ++i)
            {
                for (int j = 0; j < gameInfo.board.GetLength(1); ++j)
                {
                    globalMap[i, j] = new PathfinderTile(i, j, gameInfo.board[i][j]);
                }
            }
        }

        public string GetNextMoveToGetToDestination(int _destinationX, int _destinationY)
        {
            AddSurroundingTilesToOpenList(gameInfo.myHero.pos.x, gameInfo.myHero.pos.y);

            PathfinderTile lastTileAddedToClosedList;

            do
            {
                AddLowestHeuristicToClosedListFromOpenList(_destinationX, _destinationY);
                lastTileAddedToClosedList = closedList.ElementAt(closedList.Count - 1);
                openList.Remove(lastTileAddedToClosedList);

            } while (openList.Count > 0 && (lastTileAddedToClosedList.x != _destinationX || lastTileAddedToClosedList.y != _destinationY));

            if (openList.Count == 0)
                return Direction.Stay;

            List<PathfinderTile> finalPath = new List<PathfinderTile>();

            while (lastTileAddedToClosedList.parent != null)
            {
                finalPath.Add(lastTileAddedToClosedList);
                lastTileAddedToClosedList = lastTileAddedToClosedList.parent;
            }

            if (finalPath[finalPath.Count - 1].x > gameInfo.myHero.pos.x)
                return Direction.West;

            if (finalPath[finalPath.Count - 1].x < gameInfo.myHero.pos.x)
                return Direction.East;

            if (finalPath[finalPath.Count - 1].y > gameInfo.myHero.pos.y)
               return Direction.South;

            if (finalPath[finalPath.Count - 1].y < gameInfo.myHero.pos.y)
                return Direction.North;

            return Direction.Stay;
        }

        private void AddSurroundingTilesToOpenList(int _x, int _y)
        {
            AddSpecificTileToOpenListIfValid(_x - 1, _y);
            AddSpecificTileToOpenListIfValid(_x + 1, _y);
            AddSpecificTileToOpenListIfValid(_x, _y - 1);
            AddSpecificTileToOpenListIfValid(_x, _y + 1);
        }

        private void AddSpecificTileToOpenListIfValid(int _x, int _y)
        {
            if (_x >= 0 && _y >= 0)
            {
                if (_x < gameInfo.board.GetLength(0) && _y < gameInfo.board.GetLength(1))
                {
                    if (globalMap[_x, _y].IsTraversable(gameInfo.myHero.life, gameInfo.myHero.id))
                    {
                        openList.Add(globalMap[_x, _y]);
                    }
                }
            }
        }

        private void AddLowestHeuristicToClosedListFromOpenList(int _destinationX, int _destinationY)
        {
            PathfinderTile bestTile = openList[0];

            foreach (PathfinderTile currentTile in openList)
            {
                if (currentTile.GetEstimationCostToDestination(_destinationX, _destinationY) < bestTile.GetEstimationCostToDestination(_destinationX, _destinationY))
                {
                    bestTile = currentTile;
                }
            }

            if (closedList.Count > 0)
            {
                bestTile.parent = closedList.ElementAt(closedList.Count - 1);
            }

            closedList.Add(bestTile);
        }



    }
}