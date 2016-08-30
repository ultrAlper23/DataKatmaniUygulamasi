using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace DataKatmaniKullanimi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataAccessLayer data = new DataAccessLayer();
        private void Form1_Load(object sender, EventArgs e)
        {
            cbKategori.DisplayMember = "CategoryName";
            cbKategori.ValueMember = "CategoryID";

            cbKategori.DataSource = data.GetDataTable("select * from Categories");
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            List<Parametreler> liste = new List<Parametreler>();
           
            //Parametreler p = new Parametreler{ Name="@ad", Value=txtAd.Text}; 
            //liste.Add(p);

            liste.Add(new Parametreler { Name = "@ad", Value = txtAd.Text });
            liste.Add(new Parametreler { Name = "@aciklama", Value = txtAciklama.Text });

            data.RunASqlStatement("insert Categories (CategoryName, Description) values(@ad, @aciklama)", liste);
            txtAciklama.Clear();
            txtAd.Clear();

            Form1_Load(sender, e);
        }
    }
}
