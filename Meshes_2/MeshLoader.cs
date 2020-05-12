using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenTK;

namespace Meshes_2
{
    public struct Mesh
    {
        public Vertex[] Vertices;
        public int[] Indices;

        public Mesh(List<Vertex> vertices, List<int> indices)
        {
            Vertices = vertices.ToArray();
            Indices = indices.ToArray();
        }
    }

    public class MeshLoader
    {
        public static Mesh LoadMesh(string path)
        {
            string file;
            using (var stream = new StreamReader(path))
            {
                file = stream.ReadToEnd();
            }

            var lines = file.Split('\n');

            var vertices = new List<Vertex>();
            var indices = new List<int>();

            var vertexRegex = new Regex(@"v ([-.\d]+) ([-.\d]+) ([-.\d]+)");
            var indexRegex = new Regex(@"f (\d+)/(\d+)/(\d+) (\d+)/(\d+)/(\d+) (\d+)/(\d+)/(\d+)");

            foreach (var line in lines)
            {
                if (vertexRegex.IsMatch(line))
                {
                    var match = vertexRegex.Match(line);

                    var vertex = new Vertex(
                        new Vector3(
                            float.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture), 
                            float.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture),
                            float.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture)), 
                        new Vector3(0.6f, 0.7f, 0.7f)
                        );
                    vertices.Add(vertex);
                }

                if (indexRegex.IsMatch(line))
                {
                    var match = indexRegex.Match(line);

                    indices.Add(int.Parse(match.Groups[1].Value) - 1);
                    indices.Add(int.Parse(match.Groups[4].Value) - 1);
                    indices.Add(int.Parse(match.Groups[7].Value) - 1);
                }
            }
            return new Mesh(vertices, indices);
        }
    }
}
