using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharpTut.Systems
{
    // represents a queue of messages that can be added to 
    // has a method for and drawing to an rlconsole
    public class MessageLog
    {
        //define the maximum number of lines to store
        private static readonly int _maxLines = 9;
        // use a queue to keep track of the lines of text
        // the first line added to the log will also be the first removed
        private readonly Queue<string> _lines;
        public MessageLog()
        {
            _lines = new Queue<string>();

        }

        // add a line to the messagelog queue
        public void Add( string message)
        {
            _lines.Enqueue(message);
            //when exceeding the maximum number of lines remove the oldest one. 
            if(_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }

        }
        // draw each line of the messagelog queue to the console
        public void Draw(RLConsole console)
        {
            console.Clear();
            string[] lines = _lines.ToArray();
            for (int i=0;i< lines.Length;i++)
            {
                console.Print(1, i + 1, lines[i], RLColor.White);
            }
        }
    }
}
