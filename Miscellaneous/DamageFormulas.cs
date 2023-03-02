using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous
{
    public class DamageFormulas
    {
        // damage == armor => 50% reduction.
        // 2x increase in armor = 2x increase in reduction.
        // 2x decrease in armor = 2x decrease in reduction.
        // clamped at full damage.
        public static float GetDamage( float damage, float armor )
        {
            if( damage / armor > 1 )
                return damage;
            return damage / armor / 2;
        }
    }
}
