﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharpTut.Core;
using RogueSharpTut.Systems;
using RogueSharp.Random;

namespace RogueSharpTut
{
   public static class Game
    {

        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        // the map console takes up most of the screen and is wher the map will be drawn
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        //below the map console is the message console which displays attack rolls and otherinfo
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        // the stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        // above the map is the inventory console which shows the players equipment abilities items
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;
        public static Player Player { get;  set; }
        public static DungeonMap DungeonMap { get; private set; }
        private static bool _renderRequired = true;
        public static CommandSystem CommandSystem { get; private set; }
        // a singleton of irandom use throughtou the game when generating random numbers
        public static IRandom Random { get; private set; }
        public static MessageLog MessageLog { get; private set; }
            



        static void Main()
        {
            //establish the seed for the random number generator from the current time
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
            string fontFileName = "terminal8x8.png";
            // The title will appear at the top of the console window
            string consoleTitle = $"RougeSharp V3 Tutorial - Level 1 - Seed {seed}";
            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight,
              8, 8, 1f, consoleTitle);

            //initialize the sub consoles taht we will blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_mapWidth, _mapHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            //set background color and text for each console
            // so that we can verify they are in the correct position
            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            _mapConsole.Print(1, 1, "Map", Colors.TextHeading);

            // create a new message log and print hte random seed used to generate the level
            MessageLog = new MessageLog();
            MessageLog.Add("The rogue arrives on level 1");
            MessageLog.Add($"Level created with seed '{seed}'");

            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
            _statConsole.Print(1, 1, "Stats", Colors.TextHeading);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);
            
            //map generator

            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight,20,13,7);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();
            //command system
            CommandSystem = new CommandSystem();

            
            //

            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;
            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;
            // Begin RLNET's game loop
            _rootConsole.Run();
        }

        // Event handler for RLNET's Update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            //event handler for rlnet's update event
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();
            if(keyPress !=null)
            {
                if(keyPress.Key == RLKey.Up)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                }
                else if (keyPress.Key==RLKey.Down)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                }
                else if(keyPress.Key==RLKey.Left)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                }
                else if(keyPress.Key==RLKey.Right)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                }
                else if(keyPress.Key==RLKey.Escape)
                {
                    _rootConsole.Close();
                }

            }
            if (didPlayerAct)
            {
                _renderRequired = true;
            }
           
        }

        // Event handler for RLNET's Render event
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statConsole.Clear();
                _messageConsole.Clear();
                //draw dungeon map
                DungeonMap.Draw(_mapConsole,_statConsole);
                //draw message log
                MessageLog.Draw(_messageConsole);
                //draw player
                Player.Draw(_mapConsole, DungeonMap);
                //draw stats
                Player.DrawStats(_statConsole);
                // blit the sub consoles to the root console in the correct locations
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);
                
                
              



                // Tell RLNET to draw the console that we set
                _rootConsole.Draw();
                _renderRequired = false;
            }
        }
    }
}

