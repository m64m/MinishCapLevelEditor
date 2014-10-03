using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using GBHL;

namespace Minish_Cap_Level_Editor
{
	public partial class Form1 : Form
	{
		private GBFile gb;

		public Tileset TilesetLoader { get; set; }
		public PaletteLoader PaletteLoader { get; set; }
		public MapHeaders Header { get; set; }

		private Map map;

		public Form1()
		{
			InitializeComponent();
		}

		private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog o = new OpenFileDialog();
			o.Title = "Open a Minish Cap ROM";
			o.Filter = "GBA ROMs|*.gba|All Files|*.*";
			if (o.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;
			BinaryReader br = new BinaryReader(File.OpenRead(o.FileName));
			gb = new GBFile(br.ReadBytes((int)br.BaseStream.Length));
			br.Close();

			TilesetLoader = new Tileset(gb);
			PaletteLoader = new PaletteLoader(gb);
			Header = new MapHeaders(gb);

			//TilesetLoader.LoadTileset(0);
			//int address = PaletteLoader.GetPaletteAddress(0x34);

			map = new Map(gb);
			LoadMap(true);
		}

		private void nGroup_ValueChanged(object sender, EventArgs e)
		{
			LoadMap(true);
		}

		Bitmap bgt;
		Bitmap fgt;
		private void LoadMap(bool loadTileset)
		{
			int map = (int)nGroup.Value;
			this.map.Group = map;
			this.map.LoadMap(map, (int)nMapIndex.Value);

			Color[] palette = null;
			PaletteLoader.LoadPalette(ref palette, 0x0C);
			PaletteLoader.LoadPalette(ref palette, 0x0A);
			foreach (int i in this.map.PaletteIndexes[(int)nMapIndex.Value])
				PaletteLoader.LoadPalette(ref palette, i);
			pbPalette.Image = PaletteLoader.DrawPalette(palette);

			byte[] graphics = Decompression.BuildBuffer(this.map.Graphics.ToArray());
			if (checkBox1.Checked)
				this.map.OverrideTiles((int)nOverride.Value, ref graphics);

			/*Bitmap tiles = new Bitmap(256, 768);
			Tileset.DrawTileset(graphics, palette, 0, 0, ref tiles);
			pictureBox1.Image = tiles;*/
			//if (loadTileset)
			{
				bgt = this.map.Tileset.DrawTileset(this.map.Tileset.BackgroundBlocks, palette, graphics, 1, 0);
				fgt = this.map.Tileset.DrawTileset(this.map.Tileset.ForegroundBlocks, palette, graphics, 1, 1);
			}
			Bitmap output = null;
			this.map.DrawMap((int)nMapIndex.Value, false, bgt, ref output);
			this.map.DrawMap((int)nMapIndex.Value, true, fgt, ref output);
			pictureBox1.Image = output;
			//pictureBox1.Image = this.map.Tileset.DrawTileset(this.map.Tileset.ForegroundBlocks, palette, graphics, 4, 0);
		}

		private void nMapIndex_ValueChanged(object sender, EventArgs e)
		{
			LoadMap(false);
		}

		private void nOverride_ValueChanged(object sender, EventArgs e)
		{

		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			panel1.Height = this.Height - 91;
		}
	}
}
