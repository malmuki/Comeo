using System.Collections.Generic;
using System.Linq;
using CoveoBlitz;

namespace Coveo.Bot
{
    public class Pathfinder
    {

        public GameState gameInfo;
        private PathfinderTile[][] globalMap;
        private List<PathfinderTile> openList = new List<PathfinderTile>();
        private List<PathfinderTile> closedList = new List<PathfinderTile>();

        public Pathfinder(GameState _gameInfo)
        {
            gameInfo = _gameInfo;
            globalMap = new PathfinderTile[gameInfo.board.GetLength(0)][gameInfo.board.GetLength(1)];

            //Initialise pathfinding board
            for (int i = 0; i < gameInfo.board.GetLength(0); ++i)
            {
                for (int j = 0; j < gameInfo.board.GetLength(1); ++j)
                {
                    globalMap[i][j] = new PathfinderTile(i, j, gameInfo.board[i][j]);
                }
            }
        }

        public string GetNextMoveToGetToDestination(int _destinationX, int _destinationY)
        {
            AddSurroundingTilesToOpenList(gameInfo.myHero.pos.x, gameInfo.myHero.pos.y);

            PathfinderTile lastTileAdded;

            do
            {
                AddLowestHeuristicToClosedListFromOpenList();

                lastTileAdded = closedList.ElementAt(closedList.Count - 1);


            } while (openList.Count > 0 && (lastTileAdded.x != _destinationX || lastTileAdded.y != _destinationY));

            return Direction.Stay;
        }

        //private List<PathfinderTile> GetCompletePath()
        //{
            
        //}

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
                    if (globalMap[_x][_y].IsTraversable(gameInfo.myHero.life, gameInfo.myHero.id))
                    {
                        openList.Add(globalMap[_x][_y]);
                    }
                }
            }
        }

        private void AddLowestHeuristicToClosedListFromOpenList()
        {
            PathfinderTile bestTile = openList.Where(_x => _x == openList.Min()).ElementAt(0);

            if ()
            {

            }
            bestTile.parent = closedList.ElementAt(closedList.Count - 1);

            closedList.Add();
        }

    }
}