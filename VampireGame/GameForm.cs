using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VampireGame
{
    public partial class GameForm : Form
    {
        private string _gameLocation = "Downtown";
        public string GameLocation
        {
            get { return _gameLocation; }
            set { _gameLocation = value; }
        }

        public SaveFile save = new SaveFile();
        int _index = 0;

        # region Place lists
        public List<Person> Home = new List<Person>();
        public List<Person> Hospital = new List<Person>();
        public List<Person> Office = new List<Person>();
        public List<Person> School = new List<Person>();
        public List<Person> Shop = new List<Person>();
        # endregion

        public GameForm()
        {
            InitializeComponent();
            UpdateNumbers();
        }

        public void LoadFile(SaveFile s)
        {
            save = s;

            for (int j = 0; j < save.Locale.Count; j++)
            {
                if (save.Locale[j].Name == GameLocation)
                {
                    _index = j;
                    break;
                }
            }

            LoadAllPeople();
            timer1.Start();
        }

        private void LoadAllPeople()
        {
            ClearLists();

            if (_index >= save.Locale.Count)
            {
                return;
            }

            GameLocation gl = save.Locale[_index];

            if (gl.Name == GameLocation)
            {
                foreach (Person p in gl.People)
                {
                    # region set place
                    string currentPlace = p.Morning;
                    if (save.TimeDay == Options.TimeOfDay.Evening)
                    {
                        currentPlace = p.Evening;
                    }
                    # endregion

                    # region switch to place
                    switch (currentPlace)
                    {
                        case "Home":
                            Home.Add(p);
                            break;

                        case "Hospital":
                            Hospital.Add(p);
                            break;

                        case "Office":
                            Office.Add(p);
                            break;

                        case "School":
                            School.Add(p);
                            break;

                        case "Shop":
                            Shop.Add(p);
                            break;

                        default:
                            System.Diagnostics.Debug.WriteLine("{0} not found", p);
                            break;
                    }
                    # endregion
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Locale not found {0}.", gl.Name);
            }


            UpdateNumbers();
        }

        private void ClearLists()
        {
            Home = new List<Person>();
            Hospital = new List<Person>();
            Office = new List<Person>();
            School = new List<Person>();
            Shop = new List<Person>();
        }

        private void UpdateNumbers()
        {
            buttonHomes.Text = "Homes (" + Home.Count + ")";
            buttonSchool.Text = "School (" + School.Count + ")";
            buttonShop.Text = "Shops (" + Shop.Count + ")";
            buttonOffice.Text = "Offices (" + Office.Count + ")";
            buttonHospital.Text = "Hospital (" + Hospital.Count + ")";
        }

        private void Tick()
        {
            LoadAllPeople();

            ProcPlaces();

            SetTimeDay();
        }

        private void ProcPlaces()
        {
            double OpAv;
            List<Person> _list = Home;

            # region set belief in area of city
            double GlobOp = 0;
            foreach (Person p in save.Locale[_index].People)
            {
                GlobOp += p.Opinion;
            }
            GlobOp /= save.Locale[_index].People.Count;
            # endregion

            for (int i = 0; i < 5; i++)
            {
                # region prepare and adjust
                if (i == 0)
                {
                    //work on a per-home basis
                    List<Person> _people = _list.OrderBy(o => o.House).ToList();
                    

                }
                else
                {
                    OpAv = 0;
                    int count = 0;
                    foreach (Person p in _list)
                    {
                        count++;
                        OpAv += Math.Round(p.Opinion, 2);
                    }

                    if (count > 0)
                    {
                        OpAv /= count;
                    }

                    # region cycle through each person
                    foreach (Person p in _list)
                    {
                        double Diff = ((OpAv + GlobOp) / 2) - p.Opinion;
                        double adj = (Diff / 10);

                        # region set mod to absolute value
                        double mod = (p.Disposition / 100);
                        if (mod > 0)
                        {
                            mod++;
                        }
                        else
                        {
                            mod *= -1;
                            mod++;
                        }
                        # endregion

                        # region
                        if (adj > 0)        //positive change
                        {
                            if (p.Disposition > 0)    //likes person
                            {
                                adj *= mod;
                            }
                            else            //dislikes
                            {
                                adj /= mod;
                            }
                        }
                        else                //negative change
                        {
                            if (p.Disposition > 0)    //
                            {
                                adj /= mod;
                            }
                            else            //
                            {
                                adj *= mod;
                            }
                        }
                        # endregion

                        p.Opinion += adj;

                        p.Opinion = Math.Round(p.Opinion, 2);
                    }
                    # endregion
                }
                # endregion

                # region prepare for next run
                if (i == 0)
                {
                    labelHome.Text = "Home " + (int)count + "\r\nAverage " + OpAv;
                    _list = School;
                }
                else if (i == 1)
                {
                    labelSchool.Text = "School " + (int)count + "\r\nAverage " + OpAv;
                    _list = Shop;
                }
                else if (i == 2)
                {
                    labelShop.Text = "Shop " + (int)count + "\r\nAverage " + OpAv;
                    _list = Office;
                }
                else if (i == 3)
                {
                    labelOffice.Text = "Office " + (int)count + "\r\nAverage " + OpAv;
                    _list = Hospital;
                }
                else if (i == 4)
                {
                    labelHospital.Text = "Hospital " + (int)count + "\r\nAverage " + OpAv;
                }
                # endregion
            }
        }

        private void SetTimeDay()
        {
            if (save.TimeDay == Options.TimeOfDay.Morning)
            {
                save.TimeDay = Options.TimeOfDay.Evening;
            }
            //else if (save.TimeDay == Options.TimeOfDay.Evening)
            //{
            //    save.TimeDay = Options.TimeOfDay.Night;
            //}
            else
            {
                save.TimeDay = Options.TimeOfDay.Morning;
            }
        }

        private void FillListView()
        {
            save.Locale[_index].People = save.Locale[_index].People.OrderByDescending(o => o.Opinion).ToList();
            List<Person> people = save.Locale[_index].People;

            foreach (Person p in save.Locale[_index].People)
            {
                ListViewItem lvi = new ListViewItem(p.Surname);
                lvi.SubItems.Add(p.ForeName);
                lvi.SubItems.Add(p.Opinion.ToString());
                lvi.SubItems.Add(p.Morning);
                lvi.SubItems.Add(p.Evening);

                listView1.Items.Add(lvi);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Tick();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown1.Value;
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (timer1.Enabled)
            {
                timer1.Stop();
                FillListView();
                buttonPause.Text = "Play";
            }
            else
            {
                timer1.Start();
                buttonPause.Text = "Pause";
            }
        }

        private void buttonHospital_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            //Visit hospital this turn
            LocExplorer Exp = new LocExplorer();
            //Exp.LoadSprites(Hospital);
            Exp.LoadSprites(save.Locale[_index]);

            Exp.Show();
            Exp.FormClosed += new FormClosedEventHandler(Exp_FormClosed);

        }

        void Exp_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1_Tick(sender, e);
            timer1.Start();
        }
    }
}
