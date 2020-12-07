using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Zavod> ZavodList;
        BindingSource BindSource;
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ZavodList = new List<Zavod>();
            BindSource = new BindingSource();
            var item1 = new Zavod("Durkot", 5, "Mechanic", 25, 25000);
            var item2 = new Zavod("Kobal", 6, "Mechanic", 18, 30000);
            var item3 = new Zavod("Luts", 7, "Mechanic", 21, 25000);
            var item4 = new Zavod("Vayda", 8, "Mechanic", 5, 25000);
            var item5 = new Zavod("Antal", 9, "Mechanic", 13, 25000);
            ZavodList.Add(item1);
            ZavodList.Add(item2);
            ZavodList.Add(item3);
            ZavodList.Add(item4);
            ZavodList.Add(item5);
            BindSource.DataSource = ZavodList;
            dataGridView1.DataSource = BindSource;
            dataGridView1.Columns["Surname"].HeaderText = "Surname";
            dataGridView1.Columns["Id"].HeaderText = "Id";
            dataGridView1.Columns["Position"].HeaderText = "Position";
            dataGridView1.Columns["Experience"].HeaderText = "Experience";
            dataGridView1.Columns["Salary"].HeaderText = "Salary";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Zavod item = new Zavod(textBox1.Text, Convert.ToInt32(textBox4.Text),
                textBox3.Text, Convert.ToInt32(textBox2.Text),
                Convert.ToInt32(textBox5.Text));
            BindSource.Add(item);
            textBox1.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
            textBox5.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count = 0;
            int sum = 0;
            int num = Convert.ToInt32(textBox6.Text);
            foreach (var item in ZavodList)
            {
                if (item.Id == num)
                {
                    sum += item.Salary;
                    count += 1;
                }


            }
            var Average = sum / count;

            MessageBox.Show(Average.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //save binarry
            dataGridView1.Rows.Clear();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, ZavodList);
                fs.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //save Jason
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var jsonFormatter = new DataContractJsonSerializer(typeof(List<Zavod>));
                using (FileStream file = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    jsonFormatter.WriteObject(file, ZavodList);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //save xml
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                saveToXMLDocument(saveFileDialog.FileName);
        }
        private void saveToXMLDocument(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            doc.AppendChild(declaration);
            doc.AppendChild(createXmlZavodList(doc));
            doc.Save(fileName);
        }
        private XmlElement createXmlZavodList(XmlDocument doc)
        {
            XmlElement peopleList = doc.CreateElement("zavodList");
            foreach (var item in ZavodList)
                peopleList.AppendChild(item.ToXmlElement(doc));
            return peopleList;
        }

        private void asBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter bf = new BinaryFormatter();
                BindSource.Clear();
                foreach (Zavod b in (List<Zavod>)bf.Deserialize(fs))
                {
                    BindSource.Add(b);
                }
                fs.Close();
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter bf = new BinaryFormatter();
                BindSource.Clear();
                foreach (Zavod b in (List<Zavod>)bf.Deserialize(fs))
                {
                    BindSource.Add(b);
                }
                fs.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            dataGridView1.Rows.Clear();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var jsonFormatter = new DataContractJsonSerializer(typeof(List<Zavod>));
                using (FileStream file = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    ZavodList = jsonFormatter.ReadObject(file) as List<Zavod>;
                    if (ZavodList != null)
                    {
                        for (int i = 0; i < ZavodList.Count; i++)
                        {
                            BindSource.Add(ZavodList[i]);
                        }
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(openFileDialog.FileName);

                XmlNodeList galleriesXmlList = doc.GetElementsByTagName("gallery");
                List<Zavod> list = new List<Zavod>();
                foreach (XmlElement item in galleriesXmlList)
                    list.Add(Zavod.FromXmlElement(item));
                foreach (Zavod g in list)
                    BindSource.Add(g);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int count = 0;
            int i = 1;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                count = count + 1;
        }
    }
}
        