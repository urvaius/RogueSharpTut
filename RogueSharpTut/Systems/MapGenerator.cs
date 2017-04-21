using RogueSharp;
using RogueSharpTut.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharpTut.Systems
{
    
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly DungeonMap _map;

        //contructing a new mapgenerator requires the dimensions of the maps it wil create
        public MapGenerator( int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }

        public DungeonMap CreateMap()
        {
            //initialize every cell in the map by 
            // setting walkable, transparencty and explored to true
            _map.Initialize(_width, _height);
            foreach (Cell cell in _map.GetAllCells()) 
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            //set the first and last rows in teh map to not be transparent or walkable
            foreach(Cell cell in _map.GetCellsInRows(0,_height -1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            //set the first and last columns in the map to not be transparent or walkable
            foreach(Cell cell in _map.GetCellsInColumns(0,_width -1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            return _map;
        }

    }
}
