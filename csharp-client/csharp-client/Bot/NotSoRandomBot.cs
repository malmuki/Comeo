// Copyright (c) 2005-2016, Coveo Solutions Inc.

using System;
using Coveo.Bot;

namespace CoveoBlitz.NotSoRandomBot
{
   
    public class NotSoRandomBot : ISimpleBot
    {

        private BotAI AI = new BotAI();
        /// <summary>
        /// This will be run before the game starts
        /// </summary>
        public void Setup()
        {
            Console.WriteLine("Coveo's C# RandomBot");
        }

        /// <summary>
        /// This will be run on each turns. It must return a direction fot the bot to follow
        /// </summary>
        /// <param name="state">The game state</param>
        /// <returns></returns>
        public string Move(GameState state)
        {
            string direction = AI.WhatShouldIDo(state);

            Console.WriteLine("Completed turn {0}, going {1}", state.currentTurn, direction);
            return direction;
        }

        /// <summary>
        /// This is run after the game.
        /// </summary>
        public void Shutdown()
        {
            Console.WriteLine("Done");
        }
    }
}