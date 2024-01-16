﻿namespace BloodyShapes.Shapes
{
	class Circle : Shape
	{
		private const float PI = (float)Math.PI;

		public float Radius { get; protected set; }

		public float Diameter => Radius * 2;

		public Circle(float radius = 1, float posX = 0, float posY = 0) : base(radius * 2, radius * 2, posX, posY)
		{
			Radius = radius;
		}

		public override float Area() => PI * Radius * Radius;

		public override float Perimeter() => Diameter * PI;

		public override string ToString() => $"Circle of radius {Radius} centered on ({PositionX}, {PositionY})";

		public override bool Equals(object? obj)
		{
			if (obj is Circle other)
				return Radius == other.Radius & PositionX == other.PositionX & PositionY == other.PositionY;
			else
				return false;
		}

		public override int GetHashCode()
		{
			int radiusHash = Radius.GetHashCode();
			short posXHash = (short)PositionX.GetHashCode();
			short posYHash = (short)PositionY.GetHashCode();
			return ((posXHash << 16) + posYHash) ^ radiusHash;
		}
	}
}
