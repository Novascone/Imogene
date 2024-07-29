shader_type spatial;
render_mode cull_disabled;
uniform sampler2D _albedo: source_color;
global uniform vec4 border_color: source_color;
uniform sampler2D _noise;
uniform mat4 cutplane;
varying vec3 wvtx;

void vertex(){
	wvtx = (MODEL_MATRIX * vec4(VERTEX,1.0)).xyz;
}

void fragment() {
	
	vec3 planeNormal = normalize(-cutplane[1].xyz);
	float planeDistance = dot(planeNormal, cutplane[3].xyz);
	float vertexDistance = dot(planeNormal, wvtx);
	float dist = vertexDistance - planeDistance;
	
	vec4 clip_pos = PROJECTION_MATRIX * vec4(VERTEX, 1.0);
	vec2 uv = clip_pos.xy/5.0;
	
	uv+=TIME * 0.03;
	float noise = texture(_noise, uv).r;
	dist+= noise*0.3;
	
	if(dist < 0.0) discard;
	
	float border_dist = smoothstep(0.0, 0.3
	, dist);
	vec4 alb_color = texture(_albedo, UV);
	ALBEDO = (mix(border_color, alb_color, border_dist)).rbg;
	EMISSION = (mix(border_color * 7.0, vec4(0.0), border_dist)).rbg;
}

//void light() {
	// Called for every pixel for every li1ght affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
