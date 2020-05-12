using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Meshes_2
{
    public class Shader
    {
        public int Handle;

        private Dictionary<string, int> _uniformLocations = new Dictionary<string, int>();
        private Dictionary<string, int> _attributeLocations = new Dictionary<string, int>();

        int CompileShader(string path, ShaderType shaderType)
        {
            string shaderSource;

            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                shaderSource = reader.ReadToEnd();
            }

            var shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, shaderSource);
            GL.CompileShader(shader);

            var infoLog = GL.GetShaderInfoLog(shader);
            if (infoLog.Length != 0)
            {
                System.Console.WriteLine($"Failed at shader: {path}");
                System.Console.WriteLine(infoLog);
                return 0;
            }

            return shader;
        }

        public Shader(string vertexPath, string fragmentPath)
        {
            var vertexShader = CompileShader(vertexPath, ShaderType.VertexShader);
            var fragmentShader = CompileShader(fragmentPath, ShaderType.FragmentShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public int GetUniformLocation(string name)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
               _uniformLocations[name] = GL.GetUniformLocation(Handle, name);
            }
            return _uniformLocations[name];
        }

        public int GetAttributeLocation(string name)
        {
            if (!_attributeLocations.ContainsKey(name))
            {
                _attributeLocations[name] = GL.GetAttribLocation(Handle, name);
            }
            return _attributeLocations[name];
        }

        public void SetUniform(string name, float val)
        {
            GL.Uniform1(GetUniformLocation(name), val);
        }

        public void SetUniform(string name, Matrix4 val)
        {
            GL.UniformMatrix4(GetUniformLocation(name), false, ref val);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }
    }
}
 