using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.Web;

namespace inprogress_winforms_app
{
    public partial class Form1 : Form
    {
        List<Termek> termeklista = new List<Termek>();
        public Form1()
        {
            InitializeComponent();
            
            var snaps = proxy.ProductsFindAll();
            for (int i = 0; i < snaps.Content.Count; i++)
            {
                var prodinv = proxy.ProductInventoryFindForProduct(snaps.Content[i].Bvin);
                listBox1.Items.Add(snaps.Content[i].ProductName);
                Termek t = new Termek();
                t.id = i + 1;
                t.nev = snaps.Content[i].ProductName;
                t.keszlet = prodinv.Content[0].QuantityOnHand;
                t.inventory_id = prodinv.Content[0].Bvin;

                termeklista.Add(t);
            }
        }

         

        

        private void button_plus_Click(object sender, EventArgs e)
        {
            var z = int.Parse(textBox_mennyiseg.Text);
            z = z + 1;
            textBox_mennyiseg.Text = z.ToString();
        }

        private void button_minus_Click(object sender, EventArgs e)
        {
            var z = int.Parse(textBox_mennyiseg.Text);
            z = z - 1;
            textBox_mennyiseg.Text = z.ToString();
        }

        static string url = "http://20.234.113.211:8099/";
        static string key = "1-04d5d1e8-fa54-4a6b-871e-7a3d89631ed9";

        Api proxy = new Api(url, key);

        private void textBox_kereses_TextChanged(object sender, EventArgs e)
        {
            Szures();
        }

        private void Szures()
        {
            List<string> szures = new List<string>();
            for (int i = 0; i < termeklista.Count; i++)
            {
                if (termeklista[i].nev.StartsWith(textBox_kereses.Text))
                {
                    szures.Add(termeklista[i].nev);
                }
            }

            listBox1.DataSource = szures;      

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ((ListBox)sender).SelectedIndex + 1;
            textBox_mennyiseg.Text = (termeklista[index].keszlet).ToString();
        }

        private void button_mentes_Click(object sender, EventArgs e)
        {
            var index = listBox1.SelectedIndex + 1;
            var curproduct = termeklista[index];
            var inv = proxy.ProductInventoryFind(curproduct.inventory_id).Content;
            inv.QuantityOnHand = int.Parse(textBox_mennyiseg.Text);
            var response = proxy.ProductInventoryUpdate(inv);

        }
    }
}
