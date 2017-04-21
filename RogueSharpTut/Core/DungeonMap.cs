using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
namespace RogueSharpTut.Core
{
    public class DungeonMap : Map
    {
        // the draw method will be called each time the map is updated
        //it will render all of the symbols colors for each cell to the map sub console
        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();
            foreach(Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }
        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
            //when we havent explored a cell yet we odn't want to draw anything
            if (!cell.IsExplored)
            {
                return;
            }
            // when a cell is currently in the fiel of fiew it should be drawn wiht lighter colors
            if(IsInFov(cell.X, cell.Y))
            {
                //choose the symbol to draw basoed on if the cell is walkable or not
                if(cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            // when a cell is outside of the file dof veiw draw it with darker colors
            else
            {
                if(cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }
        }
    }
}
