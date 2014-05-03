using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PokémonGame;

namespace VampireGame
{
    public partial class LocExplorer : Form
    {
        # region declaration
        GameLocation _map = new GameLocation();
        Player _Red = new Player();

        public int ticks = Options.Ticks;
        public Direction nextMove = Direction.None;
        Rectangle ScreenSize = new Rectangle();
        string _path = "";

        bool drawing = false;
        # endregion

        public LocExplorer()
        {
            InitializeComponent();
            _Red.MoveCompleted += new Action(_Red_MoveCompleted);

            _path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            _path = Path.GetDirectoryName(_path);
        }

        void _Red_MoveCompleted()
        {
            if (nextMove != Direction.None)
            {
                MovePlayer(nextMove);
            }
        }

        private void MovePlayer(Direction dir)
        {
            if (dir == Direction.None)
            {
                return;
            }

            _Red.Facing = dir;
            int[] pos = _Red.NextPos();

            if (CheckBounds(pos))
            {
                return;
            }

            if (_map.checkPos(pos))
            {
                Debug.WriteLine("{0} is in the way.", _map.getPlayer(pos).Name);
                return;
            }

            _Red.moveDir(dir);
        }

        private static bool CheckBounds(int[] pos)
        {
            bool bounding = false;

            if (pos[0] < 0 || pos[1] < 0)
            {
                bounding = true;
            }

            return bounding;
        }

        public void LoadSprites(List<Person> List)
        {
            _map = new GameLocation();

            foreach (Person p in List)
            {
                _map.People.Add(p);
            }

            InitGameLocation();
        }

        public void LoadSprites(GameLocation loc)
        {
            _map = loc;

            InitGameLocation();
        }

        private void InitGameLocation()
        {
            _map.Talking -= _map_Talking;
            _map.Talking += new Talk(_map_Talking);

            _map._backMapLoc = _path + "\\Resources\\Office.xml";
            _map.LoadMap();
            _map.LoadBackground();
            _map.ParsePeople();

            _Red.xPos = _map._backMap.StartX;
            _Red.yPos = _map._backMap.StartY;
        }

        void _map_Talking(string s)
        {
            MessageBox.Show(s);
            _Red.Energy -= 20;
            progressBar1.Value = _Red.Energy;

            if (_Red.Energy < 1)
            {
                MessageBox.Show("Time to headhome...");
                this.Close();
            }
        }

        private void timerDraw_Tick(object sender, EventArgs e)
        {
            if (!drawing)
            {
                DrawGame();
            }
            else
            { }
        }

        private bool inMapRange(Point sbLoc)
        {
            bool result = false;                //whether sbLoc is in rectangle

            if (ScreenSize.Contains(sbLoc))
            {
                result = true;
            }

            return result;
        }

        private void DrawGame()
        {
            drawing = true;

            _Red.Tick();

            //set visible screen area
            ScreenSize = new Rectangle(_Red.Position.X, _Red.Position.Y, 10 * ticks, 10 * ticks);

            Point _offset = new Point   //work out how far to center of visible area
            {
                X = (ScreenSize.Width - ticks) / -2,
                Y = (ScreenSize.Height - ticks) / -2,
            };

            ScreenSize.Offset(_offset);

            # region Background
            Bitmap backImg = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            using (Graphics g = Graphics.FromImage(backImg))
            {
                //draw a rectangle to show visible area
                g.DrawRectangle(new Pen(Color.Blue, 5), ScreenSize);

                //draw part of screen from backimg;
                //g.DrawImage(_map.BackImg, new Rectangle(new Point(0, 0), ScreenSize.Size), ScreenSize, GraphicsUnit.Pixel);

                foreach (SpriteBase sb in _map._backMap.baseList)
                {
                    if (sb.visible)
                    {
                        //set bounding rectangle for sprite.
                        Point sbRect = new Point
                        {
                            X = sb.xPos * ticks,
                            Y = sb.yPos * ticks,
                        };

                        Pen pen = new Pen(Color.Red, 3);

                        if (inMapRange(sbRect))
                        {
                            pen.Color = Color.Green;
                        }

                        g.DrawImage(sb.toImage(), sbRect);
                    }
                }
            }

            pictureBox1.BackgroundImage = backImg;
            # endregion

            # region Foreground
            Bitmap foreImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            using (Graphics g = Graphics.FromImage(foreImage))
            {
                foreach (SpriteBase sb in _map._backMap.midList)
                {
                    if (sb.visible)
                    {
                        //set bounding rectangle for sprite.
                        Point sbRect = new Point
                        {
                            X = sb.xPos * ticks,
                            Y = sb.yPos * ticks,
                        };

                        Pen pen = new Pen(Color.Red, 3);

                        if (inMapRange(sbRect))
                        {
                            pen.Color = Color.Green;
                        }

                        //g.DrawRectangle(pen, sbRect); //TODO remove - green for on screen, red for not
                        g.DrawString(sb.Name, this.Font, new SolidBrush(Color.Black), sbRect.X, sbRect.Y + 32);
                        g.DrawImage(sb.toImage(), sbRect);
                    }
                }

                //Draw player
                g.DrawRectangle(new Pen(Color.Black), new Rectangle(_Red.Position, new Size(32, 32)));  //TODO remove - player
                g.DrawImage(_Red.ToImage(), _Red.Position);
            }

            pictureBox1.Image = foreImage;
            # endregion

            drawing = false;
        }

        private void LocExplorer_Shown(object sender, EventArgs e)
        {
            timerDraw.Start();
        }

        private void LocExplorer_KeyUp(object sender, KeyEventArgs e)
        {
            nextMove = Direction.None;

            if (e.KeyCode == Keys.C)
            {
                if (_Red.Energy >= 20)
                {
                    int[] pos = _Red.NextPos();

                    _map.activatePos(pos);
                }
            }
        }

        private void LocExplorer_KeyDown(object sender, KeyEventArgs e)
        {
            # region set direction
            switch (e.KeyCode)
            {
                case Keys.Up:
                    nextMove = Direction.Up;
                    break;

                case Keys.Down:
                    nextMove = Direction.Down;
                    break;

                case Keys.Right:
                    nextMove = Direction.Right;
                    break;

                case Keys.Left:
                    nextMove = Direction.Left;
                    break;
            }
            # endregion

            MovePlayer(nextMove);
        }
    }
}
