using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Meshes_2
{
    class Application : GameWindow
    {
        public Application(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            
        }

        //private Vertex[] vertices =
        //{
        //    new Vertex(new Vector2(0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f)),
        //    new Vertex(new Vector2(0.5f, -0.5f), new Vector3(0.0f, 0.0f, 1.0f)),
        //    new Vertex(new Vector2(-0.5f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f)),
        //    new Vertex(new Vector2(-0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f)),
        //};

        //private int[] indices =
        //{
        //    1, 2, 3,
        //    0, 1, 3,
        //};

        private ImGuiController _controller;

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        private Mesh _mesh;

        private Shader _shader;

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.95f, 0.95f, 0.95f, 1.0f);

            _shader = new Shader("shaders/shader.v", "shaders/shader.f");

            _controller = new ImGuiController();

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _mesh = MeshLoader.LoadMesh("mesh/mesh.obj"); 

            _vertexBufferObject = GL.GenBuffer();
            // copy our vertices array in a buffer for OpenGL to use
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh.Vertices.Length * Unsafe.SizeOf<Vertex>(), _mesh.Vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(_shader.GetAttributeLocation("aPosition"), 3, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), 0);
            GL.EnableVertexAttribArray(_shader.GetAttributeLocation("aPosition"));

            GL.VertexAttribPointer(_shader.GetAttributeLocation("aColor"), 3, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), Unsafe.SizeOf<Vector3>());
            GL.EnableVertexAttribArray(_shader.GetAttributeLocation("aColor"));

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _mesh.Indices.Length * sizeof(int), _mesh.Indices, BufferUsageHint.StaticDraw);

            base.OnLoad(e);
        }

        private float _scale = 0.3f;
        private float _angle = 0.0f;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();

            _controller.NewFrame(this);


            ImGui.SliderFloat("Scale", ref _scale, 0, 3);
            ImGui.SliderFloat("Angle", ref _angle, 0, 2 * 3.14f);

            _shader.SetUniform("scaleFactor", _scale);
            _shader.SetUniform("angle", _angle);

            var model = Matrix4.CreateRotationY(_angle);

            _shader.SetUniform("model", model);

            GL.BindVertexArray(_vertexArrayObject);

            //GL.PointSize(30);
            GL.DrawElements(PrimitiveType.Triangles, _mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);

            _controller.Render();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            _controller.SetWindowSize(Width, Height);

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
