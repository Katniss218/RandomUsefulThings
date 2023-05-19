using Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.Guidance
{
    public class PoweredExplicitGuidance
    {
        // decent overview
        // https://www.youtube.com/watch?v=GiT3WrUM3rs

        // very good source
        // https://pdfs.semanticscholar.org/03cd/62ecf2458205241b84791da8a952ed7a5d78.pdf?_gl=1*1su0fd9*_ga*Mzc0MDg5NzEyLjE2ODE5MjgyOTE.*_ga_H7P4ZT52H5*MTY4NDQzMDQyMS4yLjAuMTY4NDQzMDQyMS42MC4wLjA.

        // lamont's PEG (KSP)
        // https://github.com/lamont-granquist/MechJeb2/blob/lcg/PEGAS2/MechJeb2/MechJebModulePEGController.cs#L562-L830

        // there's a bunch of matrix math and integrals here.

        // "right now" is an important concept.

        // target position and velocity exist.

        // time-to-go is the remaining time until the orbit is achieved.

        // PEG starts by guessing the terminal (burnout) state (position and velocity) that satisfies the orbit.

        // Contributions from several sources (just gravity, and just thrust)
        // Rt = Position_desired - Position_current - Velocity_current * timeToGo + Position_gravity
        // Vt = Velocity_desired - Velocity_current - Velocity_gravity

        // a steering law is a thing that exist, and we must use it.
        // steering law can define expressions that return Position_after_thrust and velocity_after_thrust


        // V_GO = Velocity_desired - Velocity_current - Velocity_gravity        -- same as Vt


        // The simplest sterring law would be something like `set the attitude to point along V_GO (velocity at burnout)`      apparently this is what we do when flying maneuvers in KSP by hand
        //  - this doesn't give any control over the position at burnout though.


        // A better law would be to rotate the attitude to change linearly from initial to V_GO as time goes from 0 to TimeToGo.
        // - apparently this is called linear tangent steering law. Also apparently the most efficient if gravity was constant (in direction and magnitude).
        // - gravity rotates to point towards the earth though. which is a problem we have to account for

        // linear tangent steering: attitude = V_GO.normalized + omega_dot * time       -- omega-dot is a steering rate vector, which indicates where and how fast (* time) to curve the trajectory.

        //            a.k.a. thrustDirection = lambda_v + lambda_dot * (t - t_lambda).  -- `t - t_lambda` is equivalent to time since start flying

        // thrust integrals exist. they are a way to encode the amount of thrust over time.
        // can be split into intervals and joined by other integrals (like a piece-wise function)

        float GetTotalBurntime()
        {
            throw new NotImplementedException();
        }

        static Vector3 GetVt( Vector3 v0, Vector3 vThrustIntegral, Vector3 vGravIntegral )
        {
            // `v(t)` is the velocity vector at time t
            // `t` IS TIME SINCE LIFTOFF (`t = 0` at the start of the algorithm) (!!!).
            // `v_0` is initial velocity.
            // `v_thrust(t)` is the integrated velocity change due to thrust.
            // `v_grav(t)` is the integrated velocity change due to gravity.

            // v(t) = v_0 + v_thrust(t) + v_grav(t)
            return v0 + vThrustIntegral + vGravIntegral;
        }

        static Vector3 GetRt( Vector3 r0, Vector3 v0, Vector3 rThrustIntegral, Vector3 rGravIntegral, float timeSinceStart )
        {
            // `r(t)` is the position vector at time t.
            // `t` IS TIME SINCE LIFTOFF (`t = 0` at the start of the algorithm) (!!!).
            // `r_0` is initial position
            // `v_0` is initial velocity
            // `r_thrust(t)` is the integrated position change due to thrust.
            // `r_grav(t)` is the integrated position change due to gravity.

            // r(t) = r_0 + (v_0 * t) + r_thrust(t) + r_grav(t)
            return r0 + (v0 * timeSinceStart) + rThrustIntegral + rGravIntegral;
        }

        static Vector3 TangentSteering( Vector3 velocityAtBurnout, Vector3 lambdaDot, float timeSinceStart )
        {
            // `lambda_v` is velocity at burnout.
            // `lambda_dot`
            // `t` is present time.
            // `t_lambda` is time when the thrust vector is aligned with the total velocity change of the maneuver.

            // u_F = lambda_v + lambda_dot * (t - t_lambda)   also, `(t - t_lambda)` is apparently equivalent to `time since start`
            return velocityAtBurnout + (lambdaDot * timeSinceStart);
        }

        static class Prediction
        {
            // t_f is time at burnout (at the end of the maneuver/injection).

            // start with initial `v_grav(t_f)` and `r_grav(t_f)` equal to (0,0,0).

            static Vector3 GetVThrustTf( Vector3 injectionVelocity, Vector3 initialVelocity, Vector3 vGravIntegral ) // t_f can be replaced with t
            {
                // `v_thrust(t_f)` is velocity change at burnout due to thrust.
                // `v_inj` is injection velocity.
                // `v_0` is initial velocity.
                // `v_grav(t)` is the integrated velocity change due to gravity.

                // v_thrust(t_f) = v_inj - v_0 - v_grav(t)
                // or, in English, "`velocity change form start to end due to rocket thrust` = `end velocity` - `initial velocity` - `velocity change due to gravity`".
                // - if rocket starts in standstill, v_0 can be ignored, and it's simply end velocity minus gravity,
                // -    or end velocity minus whatever except thrust, because gravity is the only other thing that acts on the rocket in vacuum.
                return injectionVelocity - initialVelocity - vGravIntegral;
            }

            static float GetLt( Vector3 vThrustTf )
            {
                // L(t_f) = ||v_thrust(t_f)||
                return vThrustTf.Length;
            }

            static Vector3 GetLambdaV( Vector3 vThrustTf )
            {
                // lambda_v = v_thrust(t_f) / ||v_thrust(t_f)||
                return vThrustTf.Normalized();
            }

            static void GetLJSQ( float t )
            {
                // v_ex = E / m_dot
                // tau = m0 / m_dot

                // LJSQ are thrust integrals.

                // L(t) ~= -v_ex * ln(1 - (t / tau))
                // J(t) ~= -v_ex * (t + tau * ln(1 - (t / tau)))
                // S(t) ~=  L(t) * (t - tau) + v_ex * t
                // Q(t) ~=  S(t) * tau - v_ex * (t^2 / 2)
            }

            static void GetTlambda()
            {
                // t_lambda = J(t_f) / L(t_f)
            }

            static Vector3 GetRThrustTf() // t_f can be replaced with t
            {
                // `r_thrust(t_f)` is position change at burnout due to thrust.
                // r_thrust(t_f) = r_inj - r_0 - (v_0 * t_f) - r_grav(t_f)
                throw new NotImplementedException();
            }

            static float GetLambdaDot()
            {
                // lambda_dot = (r_thrust(t_f) - S(t_f) * lambda_v) / (Q(t_f) - t_lambda * S(t_f))
                throw new NotImplementedException();
            }

        }


        void IterationFromVideo() // according to video, not the paper
        {

            // guess Rd (position desired) and Vd (velocity desired)
            // we have to guess it because we can't define the target in terms of those. we define the target in terms of "is this prediction on the target orbit?"

            // first estimate of V_GO (velocity at burnout) = Vd (velocity desired) - V (velocity current)
            // calculate T_GO as time necessary to get to the velocity V_GO from current velocity, using the engines we have (delta-V stats?).
            // calculate the thrust integrals.

            // Calculate V_GO (how?)
            // set omega to magnitude(V_GO)
            // calculate omega_dot (how?)

            // combine the omega, omega_dot and thrust integrals to get the first estimate of Rt and Vt     -- Rt and Vt are pos/vel *from right now to burnout* using just the engines' thrust?

            //PEG uses a trick to account for gravity curving around.
            // "coasting trajectory" (which is just an orbit under no thrust) is propagated using gravity. the difference between final and initial of this trajectory are the Rg and Vg
            // apparently it's bad at launch and gets better as the rocket flies.
            // I integrate the gravity vector from `time-right-now` to `time-at-burnout` and set that to be Rg (position change due to gravity), and Vg (velocity change due to gravity)

            // first prediction of the terminal state.
            // Rp = R + Rt + Rg + V * T_GO
            // Vp = V + Vt + Vg

            // now corrector step.
            // compare the predicted R and V to the target orbit.
            // - we don't care where on the orbit it lies, only that it does lie on it.

            // project the Rp onto the orbit (straight down?). This gives the Rd for the next iter.
            // orbital velocity corresponding for this projected position gives the new Vd

            // update V_GO to Vd - V - Vg
            // variables saved for next iter are Rd, Vd, V_GO

            // we get the attitude using the steering law.

            // this is iterated until the output is nice and stable. in practice apparently we only check if T_GO stabilized and that attitude doesn't vary too much between iters


            // there is a problem at the end.
            // there WILL be errors in the position and velocity, but as time to go approaches 0, the attitude will swing wildly to (over-)correct for it in the ever decreasing amount of time.
            // this can be mitigated by stopping the iterations, and using a simpler algorithm, e.g. just pointing at magnitude(Vp[last]) and monitoring whether or not the orbit has been achieved (velocity too high, or something)
        }









        // r => position vector relative to the center of the body.
        // v => velocity vector in an inertial reference frame. Same frame as position.

        // Guide:

        // r_dot = dot(r, v) / magnitude(r)                          -- r_dot => vertical speed, _T => <something> at burnout.
        // b0 = b0(T, v_e, tau)
        // b1 = b1(T, v_e, tau)
        // c0 = c0(T, v_e, tau)
        // c1 = c1(T, v_e, tau)

        // M_vec_B = { { r_dot_T - r_dot }, { r_T - r - r_dot * T } }               -- this is a vector (2D?), I think.
        // [M_A] = { { b0, b1 }, { c0, c1 } }                                       -- this is a matrix (2x2?), I think. [] is just some stupid explicit thing that we can ignore?

        // Use a matrix solver to solve for M_vec_x, `[M_A] * M_vec_x = M_vec_B`, where M_vec_x = { { A }, { B } }

    }
}
