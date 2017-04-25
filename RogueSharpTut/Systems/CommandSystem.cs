﻿using RogueSharpTut.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharpTut.Systems
{
    public class CommandSystem
    {
        //return a value is true if the player was able to move
        //false when the player couldnt move, such as trying to move into a wall'
        public bool MovePlayer(Direction direction)
        {
            int x = Game.Player.X;
            int y = Game.Player.Y;
            
            switch (direction)
            {
                case Direction.Up:
                    {
                        y = Game.Player.Y - 1;
                        break;
                    }
                case Direction.Down:
                    {
                        y = Game.Player.Y + 1;
                        break;
                    }
                case Direction.Left:
                    {
                        x = Game.Player.X - 1;
                        break;

                    }
                case Direction.Right:
                    {
                        x = Game.Player.X + 1;
                        break;
                            
                    }
                default:
                    {
                        return false;
                    }
            }
            if(Game.DungeonMap.SetActorPostion(Game.Player,x,y))
            {
                return true;
            }
            return false;
        }
    }
}
