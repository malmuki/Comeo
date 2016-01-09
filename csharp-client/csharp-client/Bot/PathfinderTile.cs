using System;
using CoveoBlitz;

namespace Coveo.Bot
{
    class PathfinderTile
    {
        public int x;
        public int y;
        public Tile type;
        public PathfinderTile parent = null;

        public PathfinderTile(int _x, int _y, Tile _type)
        {
            x = _x;
            y = _y;
            type = _type;
        }

        private int GetCostToTraverse()
        {
            if (type == Tile.TAVERN)
                return 3;

            if (type == Tile.SPIKES)
                return 6;

            //Free/Mines
            return 1;
        }

        public bool IsTraversable(int _heroHP, int _heroID)
        {
            //Spikes are not traversable if you are at 10 or less HP
            if (type == Tile.FREE || type == Tile.TAVERN || type == Tile.GOLD_MINE_NEUTRAL)
                return true;

            if (type == Tile.GOLD_MINE_1 && _heroID != 1)
                return true;

            if (type == Tile.GOLD_MINE_2 && _heroID != 2)
                return true;

            if (type == Tile.GOLD_MINE_3 && _heroID != 3)
                return true;

            if (type == Tile.GOLD_MINE_4 && _heroID != 4)
                return true;

            if (type == Tile.SPIKES && _heroHP >= 10)
                return true;

            return false;

            //type == Tile.IMPASSABLE_WOOD  || type == Tile.HERO_1 || type == Tile.HERO_2 || type == Tile.HERO_3 || type == Tile.HERO_4
        }

        public int GetEstimationCostToDestination(int _destinationX, int _destinationY)
        {
            int cost = 0;
            int lastAdditionToCost;

            cost += GetCostToTraverse();

            lastAdditionToCost = x - _destinationX;
            if (lastAdditionToCost < 0)
            {
                lastAdditionToCost *= -1;
            }
            cost += lastAdditionToCost;

            lastAdditionToCost = y - _destinationY;
            if (lastAdditionToCost < 0)
            {
                lastAdditionToCost *= -1;
            }
            cost += lastAdditionToCost;

            return cost;
        }

    }
}