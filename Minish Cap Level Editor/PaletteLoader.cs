using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GBHL;

namespace Minish_Cap_Level_Editor
{
	public class PaletteLoader
	{
		private GBFile gb;

		public PaletteLoader(GBFile g)
		{
			gb = g;
		}

		public void LoadPalette (ref Color[] existing, int index)
		{
			if (existing == null)
				existing = new Color[256];
			//0801D714

			gb.BufferLocation = GetPaletteHeaderAddress(index);
			int src = 0x05A2E80 + ((gb.ReadByte() + gb.ReadByte() * 0x100) << 5);
			int dest = gb.ReadByte() << 4;
			if (dest >= 0x100) //Foreground
				return;
			int count = (gb.ReadByte() & 0x0F) << 4;
			if (count == 0)
				count = 0x10;

			gb.BufferLocation = src;
			for (int i = 0; i < count; i++)
			{
				existing[i + dest] = ReadColor();
			}
		}

		public int GetPaletteHeaderAddress(int index)
		{
			gb.BufferLocation = 0xFF850 + index * 4;
			gb.BufferLocation = gb.ReadByte() + (gb.ReadByte() << 8) + (gb.ReadByte() << 16) + ((gb.ReadByte() & 3) << 24);
			return gb.BufferLocation;
		}

		public Color ReadColor()
		{
			int rgb = gb.ReadByte() + (gb.ReadByte() << 8);
			int r = (rgb & 0x1F) * 8;
			int g = ((rgb >> 5) & 0x1F) * 8;
			int b = ((rgb >> 10) & 0x1F) * 8;
			return Color.FromArgb(r, g, b);
		}

		public Bitmap DrawPalette(Color[] colors)
		{
			Bitmap bmp = new Bitmap(128, 128);
			Graphics g = Graphics.FromImage(bmp);
			for (int i = 0; i < 256; i++)
			{
				if (colors[i].A < 255)
					colors[i] = Color.Black;
				g.FillRectangle(new SolidBrush(colors[i]), (i % 16) * 8, (i / 16) * 8, 8, 8);
			}

			return bmp;
		}
	}
}
