using System;
using System.Collections.Generic;
using System.Text;
using GBHL;

namespace Minish_Cap_Level_Editor
{
	public class MapHeaders
	{
		private GBFile gb;

		public MapHeaders(GBFile g)
		{
			gb = g;
		}

		public int GetHeaderAddress(int group)
		{
			return ReadInt(0x10246C + group * 4);
		}

		public void ReadHeader(int group)
		{
			gb.BufferLocation = GetHeaderPointerA(group);
			int unknown = PeakWord();
			if (unknown == 0xFFFF)
			{

			}
			gb.BufferLocation += 8;
			if (PeakWord() != 0xFFFF)
			{
				gb.BufferLocation -= 8;
				group *= 4;
				int temp = gb.BufferLocation;
				gb.BufferLocation = 0x10246C + group;
			}
		}

		public int GetHeaderPointerA(int group)
		{
			return ReadInt(0x11E214 + group * 4);
		}

		public int PeakWord()
		{
			int a = gb.ReadByte() + (gb.ReadByte() << 8);
			gb.BufferLocation -= 2;
			return a;
		}

		public int ReadWord(int location)
		{
			gb.BufferLocation = location;
			return ReadWord();
		}

		public int ReadWord()
		{
			return gb.ReadByte() + (gb.ReadByte() << 8);
		}

		public int ReadInt(int location)
		{
			gb.BufferLocation = location;
			return ReadInt();
		}

		public int ReadInt()
		{
			return gb.ReadByte() + (gb.ReadByte() << 8) + (gb.ReadByte() << 16) + (gb.ReadByte() & 0x00);
		}
	}
}
