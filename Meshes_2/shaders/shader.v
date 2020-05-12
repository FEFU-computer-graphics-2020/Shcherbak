#version 330 core

in vec3 aPosition;
in vec3 aColor;

uniform float scaleFactor;
uniform mat4 model;

out vec3 vertexColor;

void main()
{
	gl_Position = model * vec4(aPosition * scaleFactor, 1.0);
	vertexColor = aColor;
}