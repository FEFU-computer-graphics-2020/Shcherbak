#version 330 core

in vec3 aPosition;
in vec3 aColor;

uniform float scaleFactor;
uniform mat4 model;
uniform mat4 projection;

out vec3 vertexColor;

void main()
{
	gl_Position = projection * model * vec4(aPosition * scaleFactor, 1.0);
	vertexColor = aColor;
}