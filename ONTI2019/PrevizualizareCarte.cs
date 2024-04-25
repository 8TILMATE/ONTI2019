using _2019_onti.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONTI2019
{
    public partial class PrevizualizareCarte : Form
    {
        private CartiModel model= new CartiModel();
        public PrevizualizareCarte(CartiModel model1)
        {
            InitializeComponent();
            model = model1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap originalBitmap = (Bitmap)pictureBox1.Image;
            Size newSize = new Size((int)(originalBitmap.Width *2), (int)(originalBitmap.Height * 2));
            Bitmap bmp = new Bitmap(originalBitmap, newSize);
            pictureBox1.Image = bmp;
        }

        private void PrevizualizareCarte_Load(object sender, EventArgs e)
        {
            label1.Text = "Titlu: " + model.Titlu;
            label2.Text = "Autor: " + model.Autor;
            label3.Text = "Nrpag: " + model.Nrpag.ToString();
            foreach(string file in Directory.GetFiles("C:\\Users\\rafxg\\source\\repos\\ONTI2019\\ONTI2019\\Imagini\\carti\\"))
            {
                if (file.Contains(model.IdCarte.ToString()))
                {
                    pictureBox1.ImageLocation = file;
                    break;
                }
            }
        }
    }
}
