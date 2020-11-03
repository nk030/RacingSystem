using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingSystem.Classes
{
    public static class Factory
    {
        public static Punter GetAPunter(string classname)
        {
            if( classname.Equals("Joe"))
            {
                return new Joe() { Name = "Joe", Cash = 50 };
            }
            else if(classname.Equals("Bob"))
            {
                return new Bob() { Name = "Bob", Cash = 50 };
            }
            else if (classname.Equals("AI"))
            {
                return new AI() { Name = "AI", Cash = 50 };
            }
            else
            {
                return null;
            }
        }
    }
}
