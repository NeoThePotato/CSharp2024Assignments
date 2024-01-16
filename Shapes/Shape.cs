﻿namespace BloodyShapes.Shapes
{
	abstract class Shape
	{
		public float PositionX { get; protected set; }
		public float PositionY { get; protected set; }
		public float Width { get; protected set; }
		public float Height { get; protected set; }
		public virtual float BottomX => PositionX - (Width*.5f);
		public virtual float TopX => PositionX + (Width * .5f);
		public virtual float BottomY => PositionY - (Height * .5f);
		public virtual float TopY => PositionY + (Height * .5f);

		protected Shape(float width = 1, float height = 1, float posX = 0, float posY = 0)
		{
			PositionX = posX;
			PositionY = posY;
			Width = width;
			Height = height;
		}

		public abstract float Area();

		public abstract float Perimeter();

		public virtual void Draw() => Console.WriteLine(this);

		public override bool Equals(object? obj)
		{
			if (obj is Shape other)
				return Width == other.Width & Height == other.Height & PositionX == other.PositionX & PositionY == other.PositionY;
			else
				return false;
		}

		public override int GetHashCode()
		{
			short widthHash = (short)Width.GetHashCode();
			short heightHash = (short)Height.GetHashCode();
			short posXHash = (short)PositionX.GetHashCode();
			short posYHash = (short)PositionY.GetHashCode();
			return ((posXHash ^ widthHash) << 16) + (posYHash ^ heightHash);
		}
	}
}
