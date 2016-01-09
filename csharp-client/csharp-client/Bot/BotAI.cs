using System.Collections.Generic;
using CoveoBlitz;

namespace Coveo.Bot
{
    public class BotAI
    {
        public string WhatShouldIDo(GameState state)
        {
            Dictionary<Pos,Tile> POI = new Dictionary<Pos, Tile>();
            for (int i = 0; i < state.board.Length; i++)
            {
                for (int j = 0; j < state.board[i].Length; j++)
                {
                    if (state.board[i][j] != Tile.FREE && state.board[i][j] != Tile.IMPASSABLE_WOOD)
                    {
                        POI.Add(new Pos() { x = i, y = j }, state.board[i][j]);
                    }
                }
            }


            return "";
        }
    }
}