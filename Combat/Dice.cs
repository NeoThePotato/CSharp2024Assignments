﻿using System.Diagnostics.CodeAnalysis;

namespace Berzerkers.Combat
{
    readonly struct Dice
    {
        public readonly byte scalar;
        public readonly byte baseDie;
        public readonly short modifier;

        public Dice(byte scalar = 1, byte baseDie = 6, short modifier = 0)
        {
            this.scalar = scalar;
            this.baseDie = baseDie;
            this.modifier = modifier;
        }

        public readonly int Roll()
        {
            int retVal = modifier;
            for (int d = 0; d < scalar; d++)
                retVal += RollSingle();
            return retVal;
        }

        private readonly int RollSingle() => 1 + Random.Shared.Next(baseDie);

        public override readonly string ToString() => $"{scalar}d{baseDie}{(modifier>0?"": '+')}{modifier}";

        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            Dice other = (Dice)obj;
            return scalar == other.scalar & baseDie == other.baseDie & modifier == other.modifier;
        }

        public override readonly int GetHashCode() => (int)scalar << 24 + (int)baseDie << 16 + modifier;
    }
}
