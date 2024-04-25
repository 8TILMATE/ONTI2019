using _2019_onti.Models;
using ONTI2019.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONTI2019
{
    public partial class BibliotecarBiblioteca : Form
    {
        public BibliotecarBiblioteca()
        {
            InitializeComponent();
        }
        string numeCititor;
        int idCititor;
        private void BibliotecarBiblioteca_Load(object sender, EventArgs e)
        {
            DatabaseHelper.GetCarti();
            DatabaseHelper.GetReaders();

            foreach(UserModel model in DatabaseHelper.cititori)
            {
                dataGridView1.Rows.Add(model.Id, model.Name, model.Email);
            }
            foreach(string File in Directory.GetFiles(Resources.utilizatorImage))
            {
                if (File.Contains(DatabaseHelper.model.Id.ToString()))
                {
                    pictureBox1.ImageLocation = File;
                    break;
                }
            }
            foreach(CartiModel x in DatabaseHelper.carti)
            {
                dataGridView4.Rows.Add(x.IdCarte, x.Titlu, x.Autor, x.Nrpag);
            }
            label1.Text = DatabaseHelper.model.Email;
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
           textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            pictureBox2.ImageLocation= ofd.FileName;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == textBox4.Text) {
                int tip = 1;
                if (radioButton1.Checked)
                {
                    tip = 2;
                }
                else
                {
                    tip = 1;
                }
                UserModel userModel = new UserModel
                {
                    Name = textBox1.Text,
                    Email = textBox2.Text,
                    Tip = tip,
                    Password = PasswordEncryption.EncryptPassword(textBox3.Text)
                };
                DatabaseHelper.InsertUser(userModel);

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >0) 
            {
                 numeCititor= dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString()+" "+ dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                idCititor = Int32.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                label7.Text = numeCititor;
                foreach (string File in Directory.GetFiles(Resources.utilizatorImage))
                {
                    if (File.Contains(idCititor.ToString()))
                    {
                        pictureBox3.ImageLocation = File;
                        break;
                    }
                }
                DatabaseHelper.imprumut.Clear();
                DatabaseHelper.rezervari.Clear();
                DatabaseHelper.GetRezervari(idCititor);
                DatabaseHelper.GetImprumuturi(idCititor);
                dataGridView2.Rows.Clear();
                foreach (var x in DatabaseHelper.imprumut)
                {
                    foreach (var y in DatabaseHelper.carti)
                    {
                        if (x.IdCarte==y.IdCarte)
                        {
                            dataGridView2.Rows.Add(x.IdImprumut, x.IdCarte, y.Titlu,y.Autor,x.DataImprumut,x.DataRestituire);
                            break;
                        }
                    }
                }
                foreach (var x in DatabaseHelper.rezervari)
                {
                    foreach (var y in DatabaseHelper.carti)
                    {
                        if (x.IdCarte == y.IdCarte)
                        {
                            dataGridView3.Rows.Add(x.IdRezervare, x.IdCarte, y.Titlu, y.Autor, x.DataRezervare, x.DataRezervare.AddDays(30));
                            break;
                        }
                    }
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            foreach(var x in DatabaseHelper.cititori)
            {
                if (x.Name.Contains(textBox5.Text))
                {
                    dataGridView1.Rows.Add(x.Id,   x.Name, x.Email);
                }
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            if (dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                
                int idImprumut = Int32.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                dataGridView2.Rows.Remove(dataGridView.Rows[e.RowIndex]);
                DatabaseHelper.DeleteFromImprumuturi(idImprumut);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label8.Text = DateTime.Now.ToString();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 6)
                {
                    int idImprumut = Int32.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                    dataGridView3.Rows.Remove(dataGridView.Rows[e.RowIndex]);
                    DatabaseHelper.DeleteFromRezervari(idImprumut);
                }
                else
                {
                    int idImprumut = Int32.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                    DatabaseHelper.DeleteFromRezervari(idImprumut);
                    ImprumutModel x = new ImprumutModel
                    {
                        IdImprumut = DatabaseHelper.imprumut.Count,
                        IdCarte = Int32.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()),
                        IdCititor = idCititor,
                        DataImprumut = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy hh/mm/ss tt"), "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture),
                        DataRestituire = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy hh/mm/ss tt"), "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture).AddDays(30)
                    };

                    dataGridView2.Rows.Add(x.IdImprumut, x.IdCarte, dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), dataGridView.Rows[e.RowIndex].Cells[3].Value.ToString(), x.DataImprumut, x.DataRestituire);
                    DatabaseHelper.imprumut.Add(x);
                    DatabaseHelper.InsertImprumuturi(x);
                    dataGridView3.Rows.Remove(dataGridView.Rows[e.RowIndex]);



                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            foreach (var x in DatabaseHelper.carti)
            {
                if (x.Titlu.Contains(textBox6.Text)&&x.Autor.Contains(textBox7.Text))
                {
                    dataGridView4.Rows.Add(x.IdCarte, x.Titlu, x.Autor,x.Nrpag);
                }
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 5)
                {
                    RezervariModel x = new RezervariModel
                    {
                        IdRezervare = DatabaseHelper.imprumut.Count,
                        IdCarte = Int32.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()),
                        IdCititor = idCititor,
                        DataRezervare = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy hh/mm/ss tt"), "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture),
                        StatusRezervare = 0
                    };
                    DatabaseHelper.rezervari.Add(x);
                }
                else
                {
                    ImprumutModel x = new ImprumutModel
                    {
                        IdImprumut = DatabaseHelper.imprumut.Count,
                        IdCarte = Int32.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()),
                        IdCititor = idCititor,
                        DataImprumut = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy hh/mm/ss tt"), "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture),
                        DataRestituire = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy hh/mm/ss tt"), "MM/dd/yyyy hh/mm/ss tt", CultureInfo.InvariantCulture).AddDays(30)
                    };
                    DatabaseHelper.imprumut.Add(x);
                }
            }
            else
            {
                CartiModel model1 = new CartiModel
                {
                    IdCarte = Int32.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()),
                    Titlu = dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString(),
                    Autor = dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(),
                    Nrpag = Int32.Parse(dataGridView.Rows[e.RowIndex].Cells[3].Value.ToString())
                };
                PrevizualizareCarte playboicarti = new PrevizualizareCarte(model1);
                this.Hide();
                playboicarti.ShowDialog();
                this.Show();
            }
        }
        private void AddtoPropunere(List<CartiModel> mode)
        {

            if (mode.Count == 1)
            {
                dataGridView5.Rows.Add(mode[0].IdCarte, mode[0].Autor, mode[0].Titlu, mode[0].Nrpag);
            }
            if (mode.Count == 2)
            {
                dataGridView5.Rows.Add(mode[0].IdCarte, mode[0].Autor, mode[0].Titlu, mode[0].Nrpag, mode[1].IdCarte, mode[1].Autor, mode[1].Titlu, mode[1].Nrpag);
            }
            if (mode.Count == 3)
            {
                dataGridView5.Rows.Add(mode[0].IdCarte, mode[0].Autor, mode[0].Titlu, mode[0].Nrpag, mode[1].IdCarte, mode[1].Autor, mode[1].Titlu, mode[1].Nrpag, mode[2].IdCarte, mode[2].Autor, mode[2].Titlu, mode[2].Nrpag);
            }

        }
        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView5.Rows.Clear();
            int nrpagpersapt = Int32.Parse(textBox8.Text) * 7;
            List<CartiModel> array = new List<CartiModel>();

            foreach (CartiModel model in DatabaseHelper.carti)
            {
                if (model.Nrpag < nrpagpersapt)
                {
                    int currentnr = model.Nrpag;
                    array.Clear();
                    array.Add(model);
                    AddtoPropunere(array);
                    foreach (CartiModel model1 in DatabaseHelper.carti)
                    {
                        int currentnr2 = model1.Nrpag + currentnr;

                        if (currentnr2 < nrpagpersapt)
                        {
                            array.Add(model1);
                            AddtoPropunere(array);
                            foreach (CartiModel model2 in DatabaseHelper.carti)
                            {
                                List<CartiModel> array2 = new List<CartiModel>();
                                array2 = array;
                                int currentnr3 = currentnr2;
                                if (model2.Nrpag + currentnr3 < nrpagpersapt)
                                {
                                    array2.Add(model2);
                                    AddtoPropunere(array2);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
