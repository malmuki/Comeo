using CoveoBlitz;
using System;
using System.Collections.Generic;

namespace Coveo.Bot
{
    public class BotAI
    {
        private GameState gState;
        private int numberOfMinePlayer1 = 0;
        private int numberOfMinePlayer2 = 0;
        private int numberOfMinePlayer3 = 0;
        private int numberOfMinePlayer4 = 0;

        public string WhatShouldIDo(GameState state)
        {
            gState = state;
            Dictionary<Pos,Tile> POI = new Dictionary<Pos, Tile>();
            for (int i = 0; i < state.board.Length; i++)
            {
                for (int j = 0; j < state.board[i].Length; j++)
                {
                    if (state.board[i][j] != Tile.FREE && state.board[i][j] != Tile.IMPASSABLE_WOOD)
                    {
                        POI.Add(new Pos() { x = i, y = j }, state.board[i][j]);
                    }
                    if (state.board[i][j] == Tile.GOLD_MINE_1)
                        numberOfMinePlayer1++;
                    else if (state.board[i][j] == Tile.GOLD_MINE_2)
                        numberOfMinePlayer2++;
                    else if (state.board[i][j] == Tile.GOLD_MINE_3)
                        numberOfMinePlayer3++;
                    else if (state.board[i][j] == Tile.GOLD_MINE_4)
                        numberOfMinePlayer4++;
                }
            }
            Pos bestPoiPos = new Pos();
            int bestScore = Int32.MaxValue;

            Pathfinder pathfinder = new Pathfinder(state);

            foreach (var poi in POI)
            {
                int cost = WhatsMyScore(pathfinder.GetDestinationCost(poi.Key),poi.Value);
               if(cost < bestScore)
                {
                    bestPoiPos = poi.Key;
                    bestScore = cost;
                }
            }

            return pathfinder.GetNextMoveToGetToDestination(bestPoiPos.x,bestPoiPos.y);
        }

        private int WhatsMyScore(int cost, Tile destType)
        {
            int score = cost;
            GameState state = gState;
            int heroId = state.myHero.id;
            switch (destType)
            {
                case Tile.TAVERN:
                    if (state.myHero.life <= 25 && cost <= 25)
                    {
                        score -= 30;
                    }
                    else
                    {
                        score += 30;
                    }
                    break;
                case Tile.GOLD_MINE_NEUTRAL:
                    if (state.myHero.life <= 25 && cost <= 25)
                    {
                        score -= 13;
                    }
                    break;

                case Tile.GOLD_MINE_1:
                    if (heroId == 1)
                    {
                        score -= 15;
                    }
                    else
                    {
                        score += 100000;
                    }
                    break;

                case Tile.GOLD_MINE_2:
                    if (heroId == 2)
                    {
                        score -= 15;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;
                case Tile.GOLD_MINE_3:
                    if (heroId == 3)
                    {
                        score -= 15;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;
                case Tile.GOLD_MINE_4:
                    if (heroId == 4)
                    {
                        score -= 15;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;

                case Tile.HERO_1:
                    if (heroId != 1 )
                    {
                        score -= 7*numberOfMinePlayer1;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;
                case Tile.HERO_2:
                    if (heroId != 2)
                    {
                        score -= 7*numberOfMinePlayer2;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;
                case Tile.HERO_3:
                    if (heroId != 3)
                    {
                        score -= 7*numberOfMinePlayer3;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;
                case Tile.HERO_4:
                    if (heroId != 4)
                    {
                        score -= 7*numberOfMinePlayer4;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;
            }
            return score;
        }
    }
}