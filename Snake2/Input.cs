using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections; // For Hashtable
using System.Windows.Forms; // For Key

namespace Snake2
{
    class Input
    {
        // Load a list of available keyboard buttons
        private static Hashtable keyTable = new Hashtable();

        // Check to see if a particular button is pressed
        public static bool KeyPressed(Keys key)
        {
            if(keyTable[key] == null)
            {
                return false;
            }
            return (bool)keyTable[key];
        }

        // Detect if a button is pressed
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
