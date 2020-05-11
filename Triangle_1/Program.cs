using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Triangle_1
{
    public class Program
    {
        public static void Main()
        {
            var application = new Application(800, 600, "opengl");
            application.Run(60);
        }

    }
}
