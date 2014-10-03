using System;
using System.Collections.Generic;
using System.Drawing;
using GBHL;

namespace Minish_Cap_Level_Editor
{
	public struct Tile
	{
		public byte Palette { get; set; }
		public ushort Index { get; set; }
		public bool HFlip { get; set; }
		public bool VFlip { get; set; }
	}

	public class Tileset
	{
		private GBFile gb;

		public int Index { get; set; }
		public Bitmap LastRenderedImage { get; set; }

		public int FormationAddress { get; set; }
		public DataBuffer[] FormationBuffers { get; set; }
		public Tile[,] BackgroundBlocks { get; set; }
		public Tile[,] ForegroundBlocks { get; set; }

		private int[] offsets = new int[] { 0x0202CEB4, 0x02012654, 0 };

		public Tileset(GBFile g)
		{
			gb = g;
		}

		public void LoadFormations()
		{
			if(FormationBuffers.Length > 0)
				BackgroundBlocks = UnpackFormations(FormationBuffers[0], offsets[0]);
			if (FormationBuffers.Length > 1)
			ForegroundBlocks = UnpackFormations( FormationBuffers[1], offsets[1]);
		}

		public Tile[,] UnpackFormations(DataBuffer d, int offset)
		{
			Tile[,] blocks = new Tile[0x800, 4];
			for (int i = 0; i < 0x800; i++)
			{
				if ((d.Destination - offset) + i < 0 || (d.Destination - offset) + i >= blocks.GetLength(0))
					continue;
				for (int k = 0; k < 4; k++)
				{
					int value = 0x0200;
					if (i * 8 + k * 2 + 1 < d.Data.Length)
						value = d.Data[i * 8 + k * 2] + d.Data[i * 8 + k * 2 + 1] * 0x100;
					int tile = value & 0x3FF;
					bool hFlip = (value & 0x400) != 0;
					bool vFlip = (value & 0x800) != 0;
					int palette = (value & 0xF000) >> 12;
					Tile t = new Tile();
					t.Index = (ushort)tile;
					t.Palette = (byte)palette;
					t.HFlip = hFlip;
					t.VFlip = vFlip;
					blocks[i + d.Destination - offset, k] = t;
				}
			}

			return blocks;
		}

		public Bitmap DrawTileset(Tile[,] blocks, Color[] palette, byte[] graphicsBuffer, int width, int bank = 0)
		{
			Bitmap tileset = new Bitmap(width * 256, 2048 / width);
			FastPixel fp = new FastPixel(tileset);
			fp.rgbValues = new byte[tileset.Width * tileset.Height * 4];
			fp.Lock();
			try
			{
				int smallX = 0, smallY = 0, tileX = 0, tileY = 0, blockX = 0, blockY = 0, bigX = 0, bigY = 0;
				int height = 2048 / width;
				for (int i = 0; i < 0x800; i++)
				{
					bigX = (i / height) * 256;
					blockX = i % 16;
					blockY = (i % height) / 16;
					for (int k = 0; k < 4; k++)
					{
						tileX = k % 2;
						tileY = k / 2;
						Tile t = blocks[i, k];
						for (int tileWord = 0; tileWord < 32; tileWord++)
						{
							for (int pixel = 0; pixel < 2; pixel++)
							{
								int index = 0;
								if (t.Index * 32 + tileWord + bank * 0x4000 < graphicsBuffer.Length)
									index = (graphicsBuffer[t.Index * 32 + tileWord + bank * 0x4000] >> (pixel * 4)) & 0xF;
								Color c = palette[index + t.Palette * 16];
								if (index == 0 && bank == 1)
									c = Color.FromArgb(0, 0, 0, 0);
								fp.SetPixel(blockX * 16 + tileX * 8 + (t.HFlip ? 7 - smallX : smallX) + bigX, blockY * 16 + tileY * 8 + (t.VFlip ? 7 - smallY : smallY) + bigY, c);
								smallX++;
								if (smallX == 8)
								{
									smallX = 0;
									smallY++;
									if (smallY == 8)
									{
										smallY = 0;
									}
								}
							}
						}
					}
				}
			}
			catch (StackOverflowException)
			{
				System.Windows.Forms.MessageBox.Show("Bad tileset.");
			}

			fp.Unlock(true);
			LastRenderedImage = tileset;
			return tileset;
		}

		public static void DrawTileset(byte[] buffer, Color[] palette, int paletteIndex, int yStart, ref Bitmap dest)
		{
			int smallX = 0, smallY = 0, tileX = 0, tileY = yStart;
			for (int i = 0; i < buffer.Length; i++)
			{
				for (int k = 0; k < 2; k++)
				{
					int index = (buffer[i] >> (k * 4)) & 0xF;
					Color c = palette[index + paletteIndex * 16];
					dest.SetPixel(tileX * 8 + smallX, tileY * 8 + smallY, c);
					smallX++;
					if (smallX == 8)
					{
						smallX = 0;
						smallY++;
						if (smallY == 8)
						{
							smallY = 0;
							tileX++;
							if (tileX == 32)
							{
								tileX = 0;
								tileY++;
							}
						}
					}
				}
			}
		}
	}
}
