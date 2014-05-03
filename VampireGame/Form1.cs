using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace VampireGame
{
    public partial class Form1 : Form
    {
        public SaveFile save = new SaveFile();
        Random r = new Random();

        public Form1()
        {
            InitializeComponent();

            comboBoxMorning.Items.AddRange(Options.Place.ToArray());
            comboBoxEvening.Items.AddRange(Options.Place.ToArray());
            comboBoxLocation.Items.AddRange(Options.Location.ToArray());
        }

        private void LoadPerson(Person p)
        {
            textBoxForename.Text = p.ForeName;
            textBoxSurname.Text = p.Surname;
            numLeft.Value = p.xPos;
            numTop.Value = p.yPos;
            comboBoxMorning.Text = p.Morning;
            comboBoxEvening.Text = p.Evening;
            comboBoxLocation.Text = p.Location;

            checkBoxMale.Checked = Options.BoyForename.Contains(p.ForeName);

            numOpinion.Value = (int)p.Opinion;
            numDisposition.Value = (int)p.Disposition;
        }

        private void SavePerson()
        {
            Person p = CreatePerson();

            int i = save.Locale.FindIndex(o => o.Name == p.Location);

            if (i == -1)
            {
                save.Locale.Add(new GameLocation { Name = p.Location });
                i = save.Locale.FindIndex(o => o.Name == p.Location);
            }

            save.Locale[i].People.Add(p);
            save.People.Add(p);

            listBox1.Items.Add(p);

            ClearPerson();
        }

        private Person CreatePerson()
        {
            Person p = new Person();

            p.ForeName = textBoxForename.Text;
            p.Surname = textBoxSurname.Text;
            p.Gender = checkBoxMale.Checked;
            p.xPos = (int)numLeft.Value;
            p.yPos = (int)numTop.Value;
            p.Morning = comboBoxMorning.Text;
            p.Evening = comboBoxEvening.Text;
            p.Location = comboBoxLocation.Text;

            p.Opinion = (int)numOpinion.Value;
            p.Disposition = (int)numDisposition.Value;

            return p;
        }

        private void ClearPerson()
        {
            textBoxForename.Text = "";
            textBoxSurname.Text = "";
            numLeft.Value = 0;
            numTop.Value = 0;
            comboBoxMorning.Text = "";
            comboBoxEvening.Text = "";
            comboBoxLocation.Text = "";

            numOpinion.Value = 0;
            numDisposition.Value = 0;
        }

        private void RandomPerson()
        {
            if (r.Next(2) > 0)
            {
                textBoxForename.Text = Options.GetBoyName();
                checkBoxMale.Checked = true;
            }
            else
            {
                textBoxForename.Text = Options.GetGirlName();
                checkBoxMale.Checked = false;
            }

            textBoxSurname.Text = Options.GetSurname();
            numLeft.Value = Options.r.Next(0, 32);
            numTop.Value = Options.r.Next(0, 32);

            comboBoxMorning.Text = Options.GetPlace();
            comboBoxEvening.Text = Options.GetPlace();
            comboBoxLocation.Text = Options.GetLocation();

            numOpinion.Value = Options.r.Next(-100, 100);
            numDisposition.Value = Options.r.Next(-100, 100);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            SavePerson();
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            RandomPerson();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;

            if (i == -1)
            {
                return;
            }

            save.People.RemoveAt(i);
            ClearPerson();
            listBox1.Items.RemoveAt(i);
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;

            if (i == -1)
            {
                ClearPerson();
                return;
            }

            LoadPerson(save.People[i]);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            GameForm gForm = new GameForm();
            gForm.LoadFile(save);
            gForm.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlSerializer serial = new XmlSerializer(typeof(SaveFile));

            save.ParseFile();

            using (Stream stream = File.Create("c:\\temp.xml"))
            {
                serial.Serialize(stream, save);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlSerializer serial = new XmlSerializer(typeof(SaveFile));

            string f = "c:\\temp.xml";

            if (File.Exists(f))
            {
                using (Stream stream = File.Open(f, FileMode.Open))
                {
                    save = serial.Deserialize(stream) as SaveFile;
                }

                Update_listBox1();
            }
        }

        private void Update_listBox1()
        {
            foreach (Person p in save.People)
            {
                listBox1.Items.Add(p);
            }
        }

        private void fillRandomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                RandomPerson();
                SavePerson();
            }
        }
    }

}
