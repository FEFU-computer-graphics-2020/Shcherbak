using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Triangle_1
{
    class Application : GameWindow
    {
        public Application(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            
        }

        float[] vertices =
        {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
            0.0f,  0.5f, 0.0f  //Top vertex
        }; 

        float[] colors =
        {
            0.0f, 0.0f, 0.0f, //Bottom-left vertex
            0.5f, 0.5f, 0.5f, //Bottom-right vertex
            1.0f,  1.0f, 1.0f  //Top vertex
        }; 

        int VertexBufferObject;
        int ColorBufferObject;
        int VertexArrayObject;

        private Shader shader;

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.8f, 0.8f, 0.8f, 1.0f);

            VertexBufferObject = GL.GenBuffer();
            ColorBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            
            GL.BindVertexArray(VertexArrayObject); 
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            shader = new Shader("shaders/shader.v", "shaders/shader.f");

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
            GL.DeleteBuffer(VertexBufferObject);
            base.OnUnload(e);
        }
    }
}
