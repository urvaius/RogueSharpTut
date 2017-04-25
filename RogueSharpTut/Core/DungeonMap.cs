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

        public List<Rectangle> Rooms;

        public DungeonMap()
        {
            //initialize the list of rooms when we crate a new dungeonmap
            Rooms = new List<Rectangle>();
        }
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
        //called by mapgenerator after we generate a new map to add the player to the map
        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
        }
        // this method will be called any time we move the player to update field of view
        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            //compute the field of vew based on the players location and awareness
            ComputeFov(player.X, player.Y, player.Awareness, true);
            //mark all cells in field of vew as having been explored
            foreach(Cell cell in GetAllCells())
            {
                if(IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }


        //returns true when able to place the actor on the cell or false otherwise
        public bool SetActorPostion(Actor actor, int x, int y)
        {
            //only allow actor placement if the cell is walkable
            if(GetCell(x,y).IsWalkable)
            {
                // the cell the actor was perviously on is now walkable
                SetIsWalkable(actor.X, actor.Y, true);
                //update the actors position
                actor.X = x;
                actor.Y = y;
                // the new cell the actor is on is now not walkavble
                SetIsWalkable(actor.X, actor.Y, false);
                // update the filed of view if we just reposition the player
                if(actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }
                return true;

            }
            return false;
        }

        // helper method for setting the iswalkable property on a cell
        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Cell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
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
