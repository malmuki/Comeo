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
            Dictionary<Pos, Tile> POI = new Dictionary<Pos, Tile>();
            numberOfMinePlayer1 = 0;
            numberOfMinePlayer2 = 0;
            numberOfMinePlayer3 = 0;
            numberOfMinePlayer4 = 0;

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
                int cost = WhatsMyScore(pathfinder.GetDestinationCost(poi.Key), poi.Value);
                if (cost < bestScore)
                {
                    bestPoiPos = poi.Key;
                    bestScore = cost;
                }
            }

            return pathfinder.GetNextMoveToGetToDestination(bestPoiPos.x, bestPoiPos.y);
        }

        private int WhatsMyScore(int cost, Tile destType)
        {
            int score = cost;
            GameState state = gState;
            int heroId = state.myHero.id;
            switch (destType)
            {
                case Tile.TAVERN:
                    if (state.myHero.gold != 0)
                    {
                        if (state.myHero.life <= 35 && cost <= 25)
                        {
                            score -= 30;
                        }
                        else if (cost == 1 && state.myHero.life <= 90)
                        {
                            score -= 40;
                        }
                        else
                        {
                            score += 35;
                        }
                    }
                    else
                    {
                        score += 56;
                    }
                    break;

                case Tile.GOLD_MINE_NEUTRAL:
                    if (state.myHero.life > 26)
                    {
                        score -= 22;
                    }
                    break;

                case Tile.GOLD_MINE_1:
                    if (heroId != 1 && state.myHero.life > 26)
                    {
                        score -= 25;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;

                case Tile.GOLD_MINE_2:
                    if (heroId != 2 && state.myHero.life > 26)
                    {
                        score -= 25;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;

                case Tile.GOLD_MINE_3:
                    if (heroId != 3 && state.myHero.life > 26)
                    {
                        score -= 25;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;

                case Tile.GOLD_MINE_4:
                    if (heroId != 4 && state.myHero.life > 26)
                    {
                        score -= 25;
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;

                case Tile.HERO_1:
                    if (heroId != 1)
                    {
                        score -= 4 * numberOfMinePlayer1;
                        if (state.heroes[0].mineCount < 0 && state.heroes[0].life <= 25)
                        {
                            score -= 5;
                        }
                    }
                    else
                    {
                        score += 1000;
                    }

                    break;

                case Tile.HERO_2:
                    if (heroId != 2)
                    {
                        score -= 4 * numberOfMinePlayer2;
                        if (state.heroes[1].mineCount < 0 && state.heroes[1].life <= 25)
                        {
                            score -= 5;
                        }
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;

                case Tile.HERO_3:
                    if (heroId != 3)
                    {
                        score -= 4 * numberOfMinePlayer3;
                        if (state.heroes[2].mineCount < 0 && state.heroes[2].life <= 25)
                        {
                            score -= 5;
                        }
                    }
                    else
                    {
                        score += 1000;
                    }
                    break;

                case Tile.HERO_4:
                    if (heroId != 4)
                    {
                        score -= 4 * numberOfMinePlayer4;
                        if (state.heroes[3].mineCount < 0 && state.heroes[3].life <= 25)
                        {
                            score -= 5;
                        }
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