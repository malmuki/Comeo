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
            gameInfo = _gameInfo;
            int sizeX = gameInfo.board.GetLength(0);
            int sizeY = gameInfo.board.GetLength(0);

            globalMap = new PathfinderTile[sizeX, sizeY];

            //Initialise pathfinding board
            for (int i = 0; i < gameInfo.board.GetLength(0); ++i)
            {
                for (int j = 0; j < gameInfo.board.GetLength(0); ++j)
                {
                    globalMap[i, j] = new PathfinderTile(i, j, gameInfo.board[i][j]);
                }
            }
        }

        public string GetNextMoveToGetToDestination(int _destinationX, int _destinationY)
        {
            openList.Clear();
            closedList.Clear();

            openList.Add(globalMap[gameInfo.myHero.pos.x, gameInfo.myHero.pos.y]);

            PathfinderTile lastTileAddedToClosedList;

            do
            {
                AddLowestHeuristicToClosedListFromOpenList(_destinationX, _destinationY);
                lastTileAddedToClosedList = closedList.ElementAt(closedList.Count - 1);
                openList.Remove(lastTileAddedToClosedList);
                AddSurroundingTilesToOpenList(lastTileAddedToClosedList.x, lastTileAddedToClosedList.y, globalMap[_destinationX, _destinationY]);
            } while (openList.Count > 0 && (lastTileAddedToClosedList.x != _destinationX || lastTileAddedToClosedList.y != _destinationY));

            if (openList.Count == 0)
                return Direction.Stay;

            List<PathfinderTile> finalPath = new List<PathfinderTile>();

            do
            {
                finalPath.Add(lastTileAddedToClosedList);
                lastTileAddedToClosedList = lastTileAddedToClosedList.parent;
            } while (lastTileAddedToClosedList != null);

            finalPath.RemoveAt(finalPath.Count - 1);

            if (finalPath[finalPath.Count - 1].x < gameInfo.myHero.pos.x)
                return Direction.North;

            if (finalPath[finalPath.Count - 1].x > gameInfo.myHero.pos.x)
                return Direction.South;

            if (finalPath[finalPath.Count - 1].y < gameInfo.myHero.pos.y)
               return Direction.West;

            if (finalPath[finalPath.Count - 1].y > gameInfo.myHero.pos.y)
                return Direction.East;

            return Direction.Stay;
        }

        public int GetDestinationCost(Pos destination)
        {
            openList.Clear();
            closedList.Clear();

            int _destinationX = destination.x;
            int _destinationY = destination.y;

            openList.Add(globalMap[gameInfo.myHero.pos.x, gameInfo.myHero.pos.y]);

            PathfinderTile lastTileAddedToClosedList;

            do
            {
                AddLowestHeuristicToClosedListFromOpenList(_destinationX, _destinationY);
                lastTileAddedToClosedList = closedList.ElementAt(closedList.Count - 1);
                openList.Remove(lastTileAddedToClosedList);
                AddSurroundingTilesToOpenList(lastTileAddedToClosedList.x, lastTileAddedToClosedList.y, globalMap[_destinationX, _destinationY]);
            } while (openList.Count > 0 && (lastTileAddedToClosedList.x != _destinationX || lastTileAddedToClosedList.y != _destinationY));

            if (openList.Count == 0)
                return 9999;

            List<PathfinderTile> finalPath = new List<PathfinderTile>();

            do
            {
                finalPath.Add(lastTileAddedToClosedList);
                lastTileAddedToClosedList = lastTileAddedToClosedList.parent;
            } while (lastTileAddedToClosedList != null);

            finalPath.RemoveAt(finalPath.Count - 1);

            return finalPath.Count;
        }

        private void AddSurroundingTilesToOpenList(int _x, int _y, PathfinderTile tile)
        {
            if (AddSpecificTileToOpenListIfValid(_x - 1, _y,tile))
                openList[openList.Count - 1].parent = globalMap[_x, _y];

            if (AddSpecificTileToOpenListIfValid(_x + 1, _y, tile))
                openList[openList.Count - 1].parent = globalMap[_x, _y];

            if (AddSpecificTileToOpenListIfValid(_x, _y - 1, tile))
                openList[openList.Count - 1].parent = globalMap[_x, _y];

            if (AddSpecificTileToOpenListIfValid(_x, _y + 1, tile))
                openList[openList.Count - 1].parent = globalMap[_x, _y];
        }

        private bool AddSpecificTileToOpenListIfValid(int _x, int _y, PathfinderTile tile)
        {
            if (_x >= 0 && _y >= 0)
            {
                if (_x < gameInfo.board.GetLength(0) && _y < gameInfo.board.GetLength(0))
                {
                    if (globalMap[_x, _y].IsTraversable(gameInfo.myHero.life, gameInfo.myHero.id) || globalMap[_x, _y]== tile)
                    {
                        if (!closedList.Contains(globalMap[_x, _y]) && !openList.Contains(globalMap[_x, _y]))
                        {
                            openList.Add(globalMap[_x, _y]);
                            return true;
                        }
                    }
                }
            }

            return false;
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

            closedList.Add(bestTile);
        }


    }
}