shader_type sky;
uniform vec3 morning: source_color;
uniform vec3 sunset: source_color;
uniform vec3 night: source_color;
uniform float ratio;
uniform float offset : hint_range(0.0, 1.0, 0.1) = 0.5;
void sky() {
	vec3 sky_color = mix(sunset, night, EYEDIR.y + offset);
	COLOR = sky_color;
}
