#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;

out vec4 vertexColor;

void main()
{
	vec3 newPosition = aPosition*1.3 + vec3(0.1, 0.3, 0.2);
	gl_Position = vec4(newPosition, 1.0);
	vertexColor = vec4(aColor, 1.0);
}