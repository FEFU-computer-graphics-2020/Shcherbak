using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Meshes_2
{
    public class Program
    {
        public static void Main()
        {
            var application = new Application(800, 800, "opengl");
            application.Run(60);
        }

    }
}
