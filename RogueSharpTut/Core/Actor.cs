using RLNET;
using RogueSharp;
using RogueSharpTut.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharpTut.Core
{
    public class Actor : IActor, IDrawable
    {
        //Iactor
        public string Name { get; set; }
        public int Awareness { get; set; }

        // IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Draw(RLConsole console, IMap map)
        {
            //don't draw actors in cells that havent been explored
            if (!map.GetCell(X,Y).IsExplored)
            {
                return;
            }
            // only draw the actor with the color and symbol when they are in field of view
            if(map.IsInFov(X,Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                //when not in field of vew just draw a normal floor
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
            }

        }

    }
}
