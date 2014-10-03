using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GBHL;

namespace Minish_Cap_Level_Editor
{
	public struct DataBuffer
	{
		public int Source { get; set; }
		public int Destination { get; set; }
		public int AbsoluteDestination { get; set; }
		public byte[] Data { get; set; }
	}

	public struct MapSize
	{
		public int Width { get; set; }
		public int Height { get; set; }
	}

	public class Map
	{
		private GBFile gb;

		public int Group { get; set; }
		public MapSize[] Size { get; set; }

		public int HeaderLocation { get; set; }
		public int TilesetLocation { get; set; }
		public int BackgroundPointersLocation { get; set; }
		public int[] BackgroundLocations { get; set; }
		public List<int>[] PaletteIndexes { get; set; }
		public List<DataBuffer> Graphics { get; set; }
		public List<DataBuffer>[] BackgroundData { get; set; }
		public List<DataBuffer>[] ForegroundData { get; set; }

		public Tileset Tileset { get; set; }

		public Map(GBFile g)
		{
			gb = g;
			Graphics = new List<DataBuffer>();
			Tileset = new Tileset(gb);
		}

		public void LoadMap(int group, int index)
		{
			this.Group = group;
			Tileset.Index = group;
			LoadMapHeader();
			Graphics = LoadDataBuffers(index, TilesetLocation);
			LoadLevelData(index);
			Tileset.FormationBuffers = LoadDataBuffers(index, Tileset.FormationAddress, 0).ToArray();
			Tileset.LoadFormations();
		}

		private void LoadMapHeader()
		{
			PaletteIndexes = new List<int>[0x40];

			gb.BufferLocation = ReadInt(0x11E214 + Group * 4) & 0xFFFFFF;
			Size = new MapSize[0x40];
			for (int i = 0; i < 0x40; i++)
			{
				PaletteIndexes[i] = new List<int>();
				gb.BufferLocation += 4;
				Size[i] = new MapSize();
				Size[i].Width = ((gb.ReadByte() + (gb.ReadByte() << 8)) >> 4) & 0x7F;
				Size[i].Height = ((gb.ReadByte() + (gb.ReadByte() << 8)) >> 4) & 0x7F;
				gb.BufferLocation += 2;
			}

			gb.BufferLocation = ReadInt(0x10246C + Group * 4) & 0xFFFFFF;
			HeaderLocation = gb.BufferLocation;
			TilesetLocation = ReadInt() & 0xFFFFFF;

			gb.BufferLocation = ReadInt(0x107988 + Group * 4) & 0xFFFFFF;
			BackgroundPointersLocation = gb.BufferLocation;
			BackgroundLocations = new int[0x40];
			for (int i = 0; i < 0x40; i++)
				BackgroundLocations[i] = ReadInt();

			gb.BufferLocation = 0x0010309C + Group * 4;
			Tileset.FormationAddress = (gb.ReadByte() + (gb.ReadByte() << 8) + (gb.ReadByte() << 16) + (gb.ReadByte() << 24)) & 0xFFFFFF;
		}

		private void LoadLevelData(int index)
		{
			BackgroundData = new List<DataBuffer>[0x3F];
			ForegroundData = new List<DataBuffer>[0x3F];
			for (int i = 0; i < 0x40; i++)
			{
				try
				{
					List<DataBuffer> buffers = LoadDataBuffers(i, BackgroundLocations[i] & 0xFFFFFF);
					BackgroundData[i] = new List<DataBuffer>();
					ForegroundData[i] = new List<DataBuffer>();
					foreach (DataBuffer d in buffers)
					{
						if (d.Destination >> 0x18 == 6 && i == index) //Graphics
						{
							Graphics.Add(d);
						}
						else if (d.Destination >= 0x02025EB4) //Background
						{
							DataBuffer b = d;
							b.Destination -= 0x02025EB4;
							BackgroundData[i].Add(b);
						}
						else //Foreground
						{
							DataBuffer b = d;
							b.Destination -= 0x0200B654;
							ForegroundData[i].Add(b);
						}
					}
				}
				catch (Exception)
				{

				}
			}
		}

		public void DrawMap(int index, bool foreground, Bitmap tileset, ref Bitmap destination)
		{
			try
			{
				int width = Size[index].Width;
				int height = Size[index].Height;
				if (width == 0 || height == 0)
					return;
				byte[] tiles = Decompression.BuildBuffer((!foreground ? BackgroundData[index].ToArray() : ForegroundData[index].ToArray()));
				if (destination == null)
					destination = new Bitmap(width * 16, height * 16);
				FastPixel fp = new FastPixel(destination);
				fp.rgbValues = new byte[destination.Width * destination.Height * 4];
				fp.Lock();

				FastPixel src = new FastPixel(tileset);
				src.rgbValues = new byte[src.Width * src.Height * 4];
				src.Lock();

				for (int i = 0; i < width * height; i++)
				{
					int x = (i % width) * 16;
					int y = (i / width) * 16;
					int tile = 0;
					if (i * 2 + 1 < tiles.Length)
						tile = (tiles[i * 2] + (tiles[i * 2 + 1] << 8)) & 0x7FF;
					for (int xx = 0; xx < 16; xx++)
					{
						for (int yy = 0; yy < 16; yy++)
						{
							Color c = src.GetPixel((tile % 16) * 16 + xx, (tile / 16) * 16 + yy);
							if (c.A > 0)
								fp.SetPixel(x + xx, y + yy, c);
						}
					}
				}

				src.Unlock(false);
				fp.Unlock(true);
			}
			catch (StackOverflowException)
			{
				System.Windows.Forms.MessageBox.Show("Bad map.");
				return;
			}
		}

		public void OverrideTiles(int index, ref byte[] graphics)
		{
			gb.BufferLocation = (Group == 0x15 ? 0x108468 : 0x108408) + (index << 4);
			int temp = gb.BufferLocation;
			for (int i = 0; i < 2; i++)
			{
				gb.BufferLocation = temp;
				int src = ReadInt() + 0x5A2E80; //foreground change
				int dest = ReadInt();
				temp = gb.BufferLocation;
				if (dest >> 0x18 == 6)
					dest -= 0x06000000;
				else
				{
					//System.Windows.Forms.MessageBox.Show("Destination as " + dest.ToString("X") + " in override " + index + " for " + (i == 0 ? "background" : "foreground") + ".");
					continue;
				}

				gb.BufferLocation = src;
				for (int k = 0; k < 0x1000; k++)
				{
					graphics[dest + k] = gb.ReadByte();
				}
			}
		}

		private List<DataBuffer> LoadDataBuffers(int index, int address, int subtract = 0, bool allowPalette = true)
		{
			List<DataBuffer> spots = new List<DataBuffer>();
			gb.BufferLocation = address;
			while (true)
			{
				int src = ReadInt();
				int dest = ReadInt();
				gb.BufferLocation -= 8;
				if (dest == 0)
				{
					PaletteIndexes[index].Add(gb.ReadByte() + (gb.ReadByte() << 8));
					gb.BufferLocation += 10;
					if ((src & 0x80000000) == 0)
						break;
					continue;
				}
				src = ReadInt() + 0x00324AE4;
				dest = ReadInt();
				int type = ReadInt();
				if ((type & 0x80000000) == 0)
				{
					break;
				}
				else
				{
					DataBuffer g = new DataBuffer();
					g.Source = src & 0xFFFFFF;
					g.Destination = dest;
					g.Data = Decompression.LZ77Decompress(gb, g.Source);
					spots.Add(g);
				}

				if ((src & 0x80000000) == 0)
					break;
			}

			return spots;
		}

		public int ReadInt(int location)
		{
			gb.BufferLocation = location;
			return ReadInt();
		}

		public int ReadInt()
		{
			return gb.ReadByte() + (gb.ReadByte() << 8) + (gb.ReadByte() << 16) + (gb.ReadByte() << 24);
		}
	}
}
