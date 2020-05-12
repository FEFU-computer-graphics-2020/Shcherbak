using OpenTK;

namespace Meshes_2
{
    public struct Vertex
    {
        public Vertex(Vector2 position, Vector3 color)
        {
            this.position = position;
            this.color = color;
        }

        public Vector2 position;
        public Vector3 color;
    }
}