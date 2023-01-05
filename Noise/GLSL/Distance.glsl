
// Joins d2 and d1.
float opUnion( float d1, float d2 )
{
    return min(d1, d2);
}

// Subtracts d2 from d1
float opSubtraction( float d1, float d2 )
{
    return max(-d1, d2);
}

// The common part between d2 and d1.
float opIntersection( float d1, float d2 )
{
    return max(d1, d2);
}


// ---------------------------------
// ---------------------------------
// ---------------------------------

// Computes the (unsigned) distance between a line (infinitely long) passing through 2 points, and a point.
// - `uv` - test coordinate.
// - `p1` - first point defining the line.
// - `p2` - second point defining the line.
float distanceToLine(vec2 uv, vec2 p1, vec2 p2)
{
    vec2 lineDir = p2 - p1;
    vec2 perpDir = vec2(lineDir.y, -lineDir.x);
    vec2 dirToPt1 = p1 - uv;
    return abs(dot(normalize(perpDir), dirToPt1));
}

// ---------------------------------
// ---------------------------------
// ---------------------------------

// Computes the (unsigned) distance between a line segment and a point.
// - `uv` - test coordinate.
// - `p1` - first point defining the line segment.
// - `p2` - second point defining the line segment.
float distanceToLineSegment(vec2 uv, vec2 p1, vec2 p2)
{
    // Compute the line segment's direction vector
    vec2 v = p2 - p1;
    // Compute the line segment's normal vector
    vec2 w = uv - p1;
    // Compute the projection of w onto v
    float projection = dot(w, v) / dot(v, v);
    
    if (projection < 0.0) 
    {
        // If the projection is negative, the closest point is p1
        return distance(p1, uv);
    }
    else if (projection > 1.0)
    {
        // If the projection is greater than 1, the closest point is p2
        return distance(p2, uv);
    }
    else
    {
        // Otherwise, the closest point is the projection of p3 onto the line segment
        vec2 projectionVector = projection * v;
        vec2 closestPoint = p1 + projectionVector;
        return distance(uv, closestPoint);
    }
}

// ---------------------------------
// ---------------------------------
// ---------------------------------

// Computes the (unsigned) distance from a circle (the inside is hollow).
// - `uv` - test coordinate.
// - `center` - position of the center of the circle.
// - `radius` - the radius of the circle.
float distanceToCircle(vec2 uv, vec2 center, float radius)
{
    // Compute the distance between p and the center of the circle
    float d = distance(uv, center);
    // Return the difference between the distance and the radius
    return abs(d - radius);
}

// ---------------------------------
// ---------------------------------
// ---------------------------------

// Draws an ellipse with 1 at its perimeter and 0 at its center, interpolated uniformly.
float distanceInsideEllipse(vec2 p, vec2 center, vec2 radii)
{
    // Compute the distance between p and the center of the ellipse
    vec2 v = p - center;
    // Normalize v by the ellipse's radii
    vec2 norm = v / radii;
    // Return the length of the normalized vector
    return length(norm);
}

// ---------------------------------
// ---------------------------------
// ---------------------------------
//      POLYGON

#define N 5
#define SIGNED

float distanceToPolygon( in vec2 p, in vec2[N] v )
{
    float d = dot(p-v[0],p-v[0]);

#ifdef SIGNED
    float s = 1.0;
#endif

    for( int i = 0, j = N - 1; i < N; j = i, i++ )
    {
        // distance.
        vec2 e = v[j] - v[i];
        vec2 w = p - v[i];
        vec2 b = w - e * clamp(dot(w, e) / dot(e, e), 0.0, 1.0);
        d = min(d, dot(b, b));

#ifdef SIGNED
        // winding number.
        bvec3 cond = bvec3( p.y >= v[i].y, 
                            p.y < v[j].y, 
                            e.x * w.y > e.y * w.x );
        if (all(cond) || all(not(cond)))
        {
            s = -s;
        }
#endif
    }
    
#ifdef SIGNED
    return s*sqrt(d);
#else
    return sqrt(d);
#endif
}

// ---------------------------------
// ---------------------------------
// ---------------------------------
//      QUADRATIC BEZIER APPROX

float det(vec2 a, vec2 b)
{
    return (a.x * b.y) - (b.x * a.y);
}

float distanceToQuadraticCurve2(vec2 p, vec2 p0, vec2 p1, vec2 p2)
{
    // Returns an APPROXIMATION. Works reasonably for well-behaved curves.
  vec2 b0 = p0 - p;
  vec2 b1 = p1 - p;
  vec2 b2 = p2 - p;

  float a = det(b0,b2);
  float b = 2.0 * det(b1,b0);
  float d = 2.0 * det(b2,b1);
  
  float f = b*d-a*a;
  vec2 d21 = b2-b1, d10=b1-b0, d20=b2-b0;
  vec2 gf = 2.0*(b * d21 + d * d10 + a * d20);
  gf = vec2(gf.y, -gf.x);
  
  vec2 pp = -f * gf/dot(gf,gf);
  vec2 d0p = b0-pp;
  float ap = det(d0p,d20), bp=2.0*det(d10,d0p);
  // (note that 2*ap+bp+dp=2*a+b+d=4*area(b0,b1,b2))
  float t = clamp((ap+bp)/(2.0*a+b+d), 0.0, 1.0);
  
  vec2 x = mix(mix(b0, b1, t),mix(b1, b2, t),t);
  
  return length(x);
}

// ---------------------------------
// ---------------------------------
// ---------------------------------
//      QUADRATIC BEZIER

vec2 solveCubic2(vec3 a)
{
    // Find roots using Cardano's method - http://en.wikipedia.org/wiki/Cubic_function#Cardano.27s_method
	float p = a.y - a.x * a.x / 3.0;
	float p3 = p * p * p;
	float q = a.x * (2.0 * a.x * a.x - 9.0 * a.y) / 27.0 + a.z;
	float d = q * q + 4.0 * p3 / 27.0;
    
	if(d > 0.0)
	{
		vec2 x = (vec2(1.0, -1.0) * sqrt(d) - q) * 0.5;
        vec2 t1 = sign(x) * pow(abs(x), vec2(1.0 / 3.0));
  		return vec2((t1.x + t1.y)-a.x/3.0);
  	}
    
 	float v = acos(-sqrt(-27.0 / p3) * q * 0.5) / 3.0;
 	float m = cos(v);
 	float n = sin(v) * 1.732050808;
	return vec2(m + m, -n - m)*sqrt(-p / 3.0) - a.x / 3.0;
}

// Returns the distance from a quadratic curve.
float distanceToQuadraticCurve(vec2 p, vec2 a, vec2 b, vec2 c)
{
    // How to solve the equation below can be seen on this image.
    // http://www.perbloksgaard.dk/research/DistanceToQuadraticBezier.jpg
    
	b += mix(vec2(0.0001), vec2(0.0), abs(sign(b * 2.0 - a - c)));
	vec2 A = b - a;
	vec2 B = c - b - A;
	vec2 C = p - a;
	vec2 D = A * 2.0;
	vec2 T = clamp((solveCubic2(vec3(-3.0 * dot(A, B), dot(C, B) - 2.0 * dot(A, A), dot(C, A)) / -dot(B, B))), 0.0, 1.0);
    
    vec2 t1 = C - (D + B * T.x) * T.x;
    vec2 t2 = C - (D + B * T.y) * T.y;
	return sqrt(min(dot(t1, t1),dot(t2, t2)));
}

// ---------------------------------
// ---------------------------------
// ---------------------------------
//      CUBIC BEZIER APPROX

float distanceToCubicCurve(vec2 p, vec2 P0, vec2 P1, vec2 P2, vec2 P3)
{
    #define CLAMP
    #define ITERS 10
    
    // Cubic Bezier curve: 
    //
    //     B(t) = (1-t)^3 P0 + (1-t)^2 t P1 + (1-t) t^2 P2 + t^3 P3
    //
    // or
    //
    //     B(t) = A t^3 + B t^2 + C t + D
    //
    // where
    //
    //     A = -P0 + 3 P1 - 3 P2 + P3
    // 	   B = 3 (P0 - 2 P1 + P2)
    //     C = 3 (P1 - P0)
    //     D = P0
    
    vec2 A = -P0 + 3.0*P1 - 3.0*P2 + P3;
    vec2 B = 3.0*(P0 - 2.0*P1 + P2);
    vec2 C = 3.0*(P1 - P0);
    vec2 D = P0;
    
    // Goal:
    //
    //     "find t such that d^2 = ||B(t) - p||^2 is minimized"
    //
    // i.e. the find t such that
    //
    //     f(t) = d/dt dot(B(t) - p, B(t) - p) = 0
    // 
    // and hope that is a global minima
    
    // Expanding the above gives a 5th degree polynomial:
    //
    //     f(t) = a5 t^5 + a4 t^4 + a3 t^3 + a2 t^2 + a1 t + a0
    //
    // where 
    //
    //     a5 = 6 A A
    //     a4 = 10 A B
    //     a3 = 8 A C + 4 B B
    //     a2 = 6 A D' + 6 B C
    //     a1 = 4 B D' + 2 C C
    //     a0 = 2 C D'
    //
    // where
    // 
    //     D' = D - p
    
    float a5 = 6.0*dot(A,A);
    float a4 = 10.0*dot(A,B);
    float a3 = 8.0*dot(A,C) + 4.0*dot(B,B);
    float a2 = 6.0*dot(A,D-p) + 6.0*dot(B,C);
    float a1 = 4.0*dot(B,D-p) + 2.0*dot(C,C);
    float a0 = 2.0*dot(C,D-p);
    
    // calculate distances to the control points
    float d0 = length(p-P0);
    float d1 = length(p-P1);
    float d2 = length(p-P2);
    float d3 = length(p-P3);
    float d = min(d0, min(d1, min(d2,d3)));
    
    
    // Use the Newton-Raphson method to find a local minima, i.e. iterate:
    //
    //     t_{n+1} = t_n - f(t_n)/f'(t_n)
    //
    // until convergence is reached
    
    // Choose initial value of t
    float t;
    if (abs(d3 - d) < 1.0e-5)
        t = 1.0;
    else if (abs(d0 - d) < 1.0e-5)
        t = 0.0;
    else
        t = 0.5;
        
	// iterate
    for (int i = 0; i < ITERS; i++)
    {
        float t2 = t*t;
        float t3 = t2*t;
        float t4 = t3*t;
        float t5 = t4*t;
        
        float f = a5*t5 + a4*t4 + a3*t3 + a2*t2 + a1*t + a0;
        float df = 5.0*a5*t4 + 4.0*a4*t3 + 3.0*a3*t2 + 2.0*a2*t + a1;
        
        t = t - f/df;
    }
    
    // clamp to edge of bezier segment
#ifdef CLAMP
    t = clamp(t, 0.0, 1.0);
#endif
    
    // get the point on the curve
    vec2 P = A*t*t*t + B*t*t + C*t + D;
        
    // return distance to the point on the curve

    // Newton iteration converge to local minima that is not the global for some values
    // return length(p-P); 

    // Taking the union with the end point distances
    // seem to fix the issue for most "nice" control points
    // (since NR can only find interior points?)
    return min(length(p-P), min(d0, d3)); 
}

// ---------------------------------
// ---------------------------------
// ---------------------------------
//      CUBIC BEZIER

vec2 posBezier(in vec2 a, in vec2 b, in vec2 c, in vec2 d, in float t) {
    float tInv = 1.0 - t;
    return a * tInv * tInv * tInv + b * 3.0 * t * tInv * tInv + c * 3.0 * tInv * t * t + d * t * t * t;
}

// https://www.shadertoy.com/view/st33Wj
vec2 cmul(in vec2 z, in vec2 w) { return mat2(z, -z.y, z.x) * w; }

vec2 cdiv(in vec2 z, in vec2 w) { return cmul(z, vec2(w.x, -w.y)) / dot(w, w); }

int solveQuintic(in float a, in float b, in float c,
    in float d, in float e, in float f, out float[5] realRoots) {
    float p = (5.0 * a * c - 2.0 * b * b) / (5.0 * a * a);
    float q = (25.0 * a * a * d - 15.0 * a * b * c + 4.0 * b * b * b) / (25.0 * a * a * a);
    float r = (125.0 * a * a * a * e - 50.0 * a * a * b * d + 15.0 * a * b * b * c - 3.0 * b * b * b * b) / (125.0 * a * a * a * a);
    float s = (3125.0 * a * a * a * a * f - 625.0 * a * a * a * b * e + 125.0 * a * a * b * b * d - 25.0 * a * b * b * b * c + 4.0 * b * b * b * b * b) / (3125.0 * a * a * a * a * a);

    float bound = 1.0 + max(1.0, max(abs(p), max(abs(q), max(abs(r), abs(s)))));
    //bound *= 0.414213562373; // Correction if perturbing with random([-1...1])
    bound *= 0.5;

    vec2[5] roots;
    roots[0] = vec2(bound, 0.0);
    roots[1] = vec2(0.309016994375, 0.951056516295) * bound;
    roots[2] = vec2(-0.809016994375, 0.587785252292) * bound;
    roots[3] = vec2(-0.809016994375, -0.587785252292) * bound;
    roots[4] = vec2(0.309016994375, -0.951056516295) * bound;

    for (int iter=0; iter < 25; iter++) {
        float maxEval = -1e20;
        for (int root=0; root < 5; root++) {
            vec2 z = roots[root];
            vec2 quinticVal = cmul(cmul(cmul(cmul(z, z) + vec2(p, 0.0), z) + vec2(q, 0.0), z) + vec2(r, 0.0), z) + vec2(s, 0.0);
            maxEval = max(maxEval, max(abs(quinticVal.x), abs(quinticVal.y)));

            vec2 denom = z - roots[(root + 1) % 5];
            denom = cmul(denom, z - roots[(root + 2) % 5]);
            denom = cmul(denom, z - roots[(root + 3) % 5]);
            denom = cmul(denom, z - roots[(root + 4) % 5]);

            roots[root] -= cdiv(quinticVal, denom);
        }

        if (maxEval < 1e-7) break;
    }

    int numRealRoots = 0;
    float offs = b / (5.0 * a);
    for (int root=0; root < 5; root++) {
        vec2 z = roots[root];
        if (abs(z.y) < 1e-7) {
            realRoots[numRealRoots] = z.x - offs;
            numRealRoots++;
        }
    }

    return numRealRoots;
}

float dot2(in vec2 v) { return dot(v, v); }

// Returns a distance to the cubic bezier curve (.w component).
vec4 distanceToCubicCurve(in vec2 p, in vec2 v1, in vec2 v2, in vec2 v3, in vec2 v4) {
    // Convert to power basis
    vec2 a = v4 + 3.0 * (v2 - v3) - v1;
    vec2 b = 3.0 * (v1 - 2.0 * v2 + v3);
    vec2 c = 3.0 * (v2 - v1);
    vec2 d = v1 - p;

    // Quintic coefficients (derivative of distance-for-t with 2 factored out)
    float qa = 3.0 * dot(a, a);
    float qb = 5.0 * dot(a, b);
    float qc = 4.0 * dot(a, c) + 2.0 * dot(b, b);
    float qd = 3.0 * (dot(b, c) + dot(d, a));
    float qe = dot(c, c) + 2.0 * dot(d, b);
    float qf = dot(d, c);

    float distSq = dot2(p - v1);
    float tNear = 0.0;
    vec2 pNear = v1;

    float distSq2 = dot2(p - v4);
    if (distSq2 < distSq) {
        distSq = distSq2;
        tNear = 1.0;
        pNear = v4;
    }

    float[5] roots;
    int numRoots = solveQuintic(qa, qb, qc, qd, qe, qf, roots);
    for (int n=0; n < numRoots; n++) {
        float t = roots[n];
        if (0.0 < t && t < 1.0) {
            vec2 pos = posBezier(v1, v2, v3, v4, t);
            float distSq2 = dot2(p - pos);
            if (distSq2 < distSq) {
                distSq = distSq2;
                pNear = pos;
                tNear = t;
            }
        }
    }

    return vec4(pNear, tNear, sqrt(distSq));
}


//

// https://iquilezles.org/articles/normalsSDF
vec3 calcNormal( in vec3 pos )
{
    const float ep = 0.0001;
    vec2 e = vec2(1.0,-1.0)*0.5773;
    return normalize( e.xyy*SDF + 
					  e.yyx*SDF + 
					  e.yxy*SDF + 
					  e.xxx*SDF );
}

// https://iquilezles.org/articles/rmshadows
float calcSoftshadow( in vec3 ro, in vec3 rd, float tmin, float tmax, const float k )
{
	float res = 1.0;
    float t = tmin;
    for( int i=0; i<50; i++ )
    {
		float h = map( ro + rd*t );
        res = min( res, k*h/t );
        t += clamp( h, 0.02, 0.20 );
        if( res<0.005 || t>tmax ) break;
    }
    return clamp( res, 0.0, 1.0 );
}





