using System;
using System.Collections.Generic;
using System.Text;
using GBHL;
namespace Minish_Cap_Level_Editor
{
	public class Decompression
	{
		private GBFile gb;

		public Decompression(GBFile g)
		{
			gb = g;
		}

		public static byte[] LZ77Decompress(GBFile gb, int location)
		{
			//1194
			int temp = gb.BufferLocation;
			gb.BufferLocation = location;

			byte[] output = new byte[0x10000];
			int writeLocation = 0;
			uint r2 = 0;
			uint r3 = 0;
			uint r4 = 0;
			uint r5 = 0;
			uint r8 = 0;
			int remaining = 0; //r10
			uint r12 = 0;
			uint r14 = 0x11B0;

			remaining = (int)(ReadDWord(gb) >> 8);
			r12 = (uint)remaining;

			r12 &= 0x1FFFFFF;
			r12 += (uint)gb.BufferLocation;

			while (remaining > 0)
			{
				uint mask = gb.ReadByte();
				for (int i = 0; i < 8; i++)
				{
					if ((mask & 0x80) == 0)
					{
						uint b = gb.ReadByte();
						r3 = (uint)(r3 | (b << (byte)r2));
						remaining--;
						r2 ^= 8;
						if (r2 == 0)
						{
							output[writeLocation++] = (byte)(r3 & 0xFF);
							output[writeLocation++] = (byte)((r3 >> 8) & 0xFF);
							r3 = 0;
						}
					}
					else
					{
						uint b = gb.ReadByte();
						r5 = 3 + (b >> 4);
						r8 = (b & 0xF);
						r4 = r8 << 8;
						b = gb.ReadByte();
						r8 = b | r4;
						r4 = r8 + 1;
						r8 = 8 - r2;
						b = r4 & 1;
						r14 = r8 ^ (b << 3);
						remaining -= (int)r5;
					R5Procedure:
						r14 ^= 8;
						r8 = 8 - r2;
						r8 = r4 + (r8 >> 3);
						r8 >>= 1;
						r8 <<= 1;
						b = (uint)(output[writeLocation - r8 + 1] * 0x100 + output[writeLocation - r8]);
						r8 = 0xFF;
						r8 = b & (r8 << (byte)r14);
						r8 >>= (byte)r14;
						r3 = r3 | (r8 << (byte)r2);

						r2 ^= 8;
						if (r2 == 0)
						{
							output[writeLocation++] = (byte)(r3 & 0xFF);
							output[writeLocation++] = (byte)((r3 >> 8) & 0xFF);
							r3 = 0;
						}
						r5 -= 1;
						if (r5 > 0)
							goto R5Procedure;
					}
					if (remaining <= 0)
						break;
					mask <<= 1;
				}
			}

			if (writeLocation != output.Length)
			{
				byte[] n = new byte[writeLocation];
				Array.Copy(output, n, writeLocation);
				output = n;
			}

			gb.BufferLocation = temp;
			return output;
		}

		public static byte[] BuildBuffer(DataBuffer[] buffers, int subtract = 0)
		{
			byte[] buffer = new byte[0x1000000];
			int max = 0;
			foreach (DataBuffer d in buffers)
			{
				int change = (d.Destination >> 0x18 == 6 ? 0x06000000 : 0);
				Array.Copy(d.Data, 0, buffer, d.Destination - change, d.Data.Length);
				if (d.Data.Length + d.Destination - change > max)
					max = d.Data.Length + d.Destination - change;
			}

			byte[] output = new byte[max];
			Array.Copy(buffer, output, max);
			return output;
		}

		private static uint ReadDWord(GBFile gb)
		{
			return (uint)(gb.ReadByte() + (gb.ReadByte() << 8) + (gb.ReadByte() << 16) + (gb.ReadByte() << 24));
		}
	}
}
