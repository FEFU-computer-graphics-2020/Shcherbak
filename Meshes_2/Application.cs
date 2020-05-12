using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Meshes_2
{
    class Application : GameWindow
    {
        public Application(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            
        }

        //float[] vertices =
        //{
        //    0.5f, 0.5f,  //Bottom-left vertex
        //    0.5f, -0.5f, //Bottom-right vertex
        //    -0.5f, -0.5f,  //Top vertex
        //    -0.5f, 0.5f
        //}; 

        //float[] colors =
        //{
        //    1.0f, 0.0f, 0.0f,
        //    0.0f, 1.0f, 0.0f, 
        //    0.0f, 0.0f, 1.0f,
        //    1.0f, 0.0f, 0.0f,
        //};

        private Vertex[] vertices =
        {
            new Vertex(new Vector2(0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f)),
            new Vertex(new Vector2(0.5f, -0.5f), new Vector3(0.0f, 0.0f, 1.0f)),
            new Vertex(new Vector2(-0.5f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f)),
            new Vertex(new Vector2(-0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f)),
        };

        private int[] indices =
        {
            1, 2, 3,
            0, 1, 3,
        };

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        private Mesh _mesh;

        private Shader _shader;

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            _shader = new Shader("shaders/shader.v", "shaders/shader.f");

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _mesh = MeshLoader.LoadMesh("mesh/mesh.obj"); 

            _vertexBufferObject = GL.GenBuffer();
            // copy our vertices array in a buffer for OpenGL to use
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh.Vertices.Length * Unsafe.SizeOf<Vertex>(), _mesh.Vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(_shader.GetAttributeLocation("aPosition"), 2, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), 0);
            GL.EnableVertexAttribArray(_shader.GetAttributeLocation("aPosition"));

            //_colorBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferObject);
            //GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_shader.GetAttributeLocation("aColor"), 3, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), Unsafe.SizeOf<Vector2>());
            GL.EnableVertexAttribArray(_shader.GetAttributeLocation("aColor"));

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _mesh.Indices.Length * sizeof(int), _mesh.Indices, BufferUsageHint.StaticDraw);

            base.OnLoad(e);
        }

        //private float scale = 0.0f;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();
            _shader.SetUniform("scaleFactor", 3f);


            GL.BindVertexArray(_vertexArrayObject);

            //GL.PointSize(30);
            GL.DrawElements(PrimitiveType.Triangles, _mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            base.OnUnload(e);
        }
    }
}
