using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PokémonGame;

namespace VampireGame
{
    public class Player
    {
        # region Position
        private int _xPos = 0;
        private int _yPos = 0;
        public Point Position
        {
            get
            {
                return new Point(_xPos, _yPos);
            }
            set
            {
                _xPos = value.X;
                _yPos = value.Y;
            }
        }
        public int xPos
        {
            get { return _xPos / 32; }
            set { _xPos = value * 32; }
        }
        public int yPos
        {
            get { return _yPos / 32; }
            set { _yPos = value * 32; }
        }
        # endregion

        # region Facing
        private Direction _facing = Direction.Down;
        public Direction Facing
        {
            get { return _facing; }
            set { _facing = value; }
        }
        # endregion

        public bool Moving = false;
        private int _cooldown = 0;
        private int StepSize = 8;

        # region image variables
        public Image[,] sprites = new Image[3, 4]; //[variant(move1,move2,stop1/dirLook]
        public string spriteSheet = "";
        public int xStart = 0;
        public int yStart = 0;
        public int width = 0;
        public int height = 0;
        # endregion

        public int variant = 0;
        public int dirLook = 0;

        public int Energy = 100;

        # region MoveCompleted
        public event Action MoveCompleted;
        protected void onMoveCompleted()
        {
            xPos = xPos;
            yPos = yPos;
            Action handler = MoveCompleted;
            if (handler != null)
            {
                handler();
            }
        }
        # endregion

        public bool moveDir(Direction dir)
        {
            if (!Moving)
            {
                Facing = dir;

                Moving = true;
                _cooldown = -StepSize;
            }

            return Moving;
        }

        public void Tick()
        {
            _cooldown++;

            int steps = Options.Ticks / StepSize;

            if (_cooldown < 1)
            {
                MoveStep(steps);
            }

            if (_cooldown >= 0)
            {
                Moving = false;
                onMoveCompleted();
            }
        }

        private void MoveStep(int steps)
        {
            if (Facing == Direction.Up) { _yPos -= steps; }
            if (Facing == Direction.Down) { _yPos += steps; }
            if (Facing == Direction.Left) { _xPos -= steps; }
            if (Facing == Direction.Right) { _xPos += steps; }
        }

        public Image ToImage()
        {
            Image temp;

            if (sprites[variant, dirLook] != null)
            {
                temp = sprites[variant, dirLook];
            }
            else
            {
                temp = new Bitmap(32, 32);
            }

            return temp;
        }

        public Image BaseImage()
        {
            Image img = new Bitmap(32, 32);

            if (img == null)
            {
                string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string path = spriteSheet.Replace("%PATH%", dir);
                img = ImageLoad.FromSheet(path, xStart, yStart, width, height);
            }

            Image temp = img.Clone() as Image;

            return temp;
        }

        public int[] NextPos()
        {
            int x = xPos;
            int y = yPos;
            if (Facing == Direction.Up) { y--; }
            if (Facing == Direction.Down) { y++; }
            if (Facing == Direction.Left) { x--; }
            if (Facing == Direction.Right) { x++; }

            return new int[] { x, y };
        }
    }

    public class SaveFile
    {
        /// <summary>
        /// Old Method - Remove
        /// </summary>
        public List<Person> People = new List<Person>();

        /// <summary>
        /// Location that contains people
        /// </summary>
        public List<GameLocation> Locale = new List<GameLocation>();

        public Options.TimeOfDay TimeDay = Options.TimeOfDay.Morning;

        public void ParseFile()
        {
            foreach (GameLocation g in Locale)
            {
                g.People.Clear();
            }

            foreach (Person p in People)
            {
                int i = Locale.FindIndex(o => o.Name == p.Location);

                if (i == -1)
                {
                    Locale.Add(new GameLocation { Name = p.Location });
                    i = Locale.FindIndex(o => o.Name == p.Location);
                }

                Locale[i].People.Add(p);
            }
        }
    }

    public class GameLocation
    {
        # region attributes
        public string Name = "";
        public List<Person> People = new List<Person>();
        Random r = new Random();
        private Dictionary<string, Person> PeopleCoords = new Dictionary<string, Person>();

        public string _backMapLoc = "";
        [XmlIgnore]
        public map _backMap = new map();

        public string BackgroundLoc = "";
        [XmlIgnore]
        public Image BackImg = new Bitmap(160, 160);
        # endregion

        public event Talk Talking;
        protected void onTalking(string s)
        {
            Talk handler = Talking;
            if (handler != null)
            {
                handler(s);
            }
        }

        /// <summary>
        /// Check whether there are any people at the specified co-ordinates
        /// </summary>
        /// <param name="x">How far left</param>
        /// <param name="y">How far down</param>
        /// <returns>true for occupied, false for empty</returns>
        public bool checkPos(int x, int y)
        {
            bool result = false;

            foreach (Person p in People)
            {
                if (p.xPos == x && p.yPos == y)
                {
                    result = true;
                }
            }

            return result;
        }

        public bool checkPos(int[] pos)
        {
            return checkPos(pos[0], pos[1]);
        }

        public Person getPlayer(int x, int y)
        {
            Person result = null;

            foreach (Person p in People)
            {
                if (p.xPos == x && p.yPos == y)
                {
                    result = p;
                }
            }

            return result;
        }

        public Person getPlayer(int[] pos)
        {
            return getPlayer(pos[0], pos[1]);
        }

        public bool activatePos(int[] pos)
        {
            string s = pos[0] + "," + pos[1];

            if (PeopleCoords.ContainsKey(s))
            {
                Person p = PeopleCoords[s];

                onTalking("Huh, never thought of it that way before. [Opinion is " + p.Opinion + "]");
                p.Opinion += 10;

                return true;
            }
            else
            {
                return false;
            }
        }

        public void LoadMap()
        {
            if (!File.Exists(_backMapLoc))
            {
                Debug.WriteLine("File {0} not found.", _backMapLoc);
                return;
            }

            XmlSerializer serial = new XmlSerializer(typeof(map));
            using (Stream stream = File.Open(_backMapLoc, FileMode.Open))
            {
                _backMap = serial.Deserialize(stream) as map;
            }
        }

        public void ParsePeople()
        {
            System.Diagnostics.Debug.WriteLine("ParsePeople()");
            string _path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            _path = Path.GetDirectoryName(_path);

            # region set images
            Rectangle rect = new Rectangle(0, 0, 32, 32);
            Image[] boyImg = SpriteLoader.loadspritesI(ImageLoad.get(_path + "\\Resources\\TileSets\\OverworldItems.png"), rect, 32, 4);

            rect.X = 23 * 32;
            Image[] girlImg = SpriteLoader.loadspritesI(ImageLoad.get(_path + "\\Resources\\TileSets\\OverworldItems.png"), rect, 32, 4);
            # endregion

            PeopleCoords.Clear();

            List<SpriteBase> positions = getPositions();
            People = People.OrderBy(o => o.Worked).ToList();

            # region
            foreach (Person p in People)
            {
                //if (p.spriteSheet == "")
                //{
                //    p.spriteSheet = "";
                //}
                # region set position and set visible to true if assigned a position
                if (positions.Count > 0)
                {
                    SpriteBase sb = positions[0];
                    p.xPos = sb.xPos;
                    p.yPos = sb.yPos;

                    _backMap.midList.Add(p);

                    p.DirLookNum = r.Next(0, 4);
                    p.Worked = 0;   //days since last worked
                    p.visible = true;
                    sb.visible = false;

                    PeopleCoords.Add(p.xPos + "," + p.yPos, p);

                    positions.RemoveAt(0);  //remove so that it doesn't get double assigned
                }
                else
                {
                    p.visible = false;
                    p.Worked++;     //days since last worked
                }
                # endregion

                p.spriteSheet = _path + "\\Resources\\TileSets\\OverworldItems.png";

                if (p.Gender)
                {
                    p._images = boyImg;
                }
                else
                {
                    p._images = girlImg;
                }
            }
            # endregion

        }

        private List<SpriteBase> getPositions()
        {
            List<SpriteBase> result = new List<SpriteBase>();

            foreach (SpriteBase sb in _backMap.midList)
            {
                if (sb.Name == "SpritePos")
                {
                    result.Add(sb);
                }
            }

            return result;
        }

        public void LoadBackground()
        {
            System.Diagnostics.Debug.WriteLine("LoadBackground()");
            if (File.Exists(BackgroundLoc))
            {
                BackImg = Image.FromFile(BackgroundLoc);
            }
        }
    }

    public class Person : SpritePerson
    {
        # region attributes
        public string ForeName = "Name";
        public string Surname = "";
        new public string Name
        {
            get { return ForeName + " " + Surname; }
            set { }
        }

        public string House = "";

        public int Worked = 0;  //Time since last shown in viewer.
        //public bool Visible = false;

        public string Morning = "Surgery";
        public string Evening = "Home";
        public string Location = "Downtown";

        //public string spriteSheet = "";
        //public int xStart = 0;
        //public int yStart = 0;

        [XmlIgnore]
        public Image[] _images = new Image[4];
        private Image _img;

        public Direction DirLook = Direction.Down;
        public int DirLookNum
        {
            get
            {
                switch (DirLook)
                {
                    case Direction.Up:
                        return 0;

                    case Direction.Right:
                        return 1;

                    case Direction.Down:
                        return 2;

                    case Direction.Left:
                        return 3;

                    default:
                        return 4;
                }
            }
            set
            {
                switch (value)
                {
                    case 0:
                        DirLook = Direction.Up;
                        break;

                    case 1:
                        DirLook = Direction.Right;
                        break;

                    case 2:
                        DirLook = Direction.Down;
                        break;

                    case 3:
                        DirLook = Direction.Left;
                        break;

                    default:
                        DirLook = Direction.None;
                        break;
                }
            }
        }

        public bool Gender = false; //Male?
        # endregion

        public bool Loaded = false;

        private double _opinion = 0;
        public double Opinion
        {
            get { return _opinion; }
            set
            {
                _opinion = value;

                if (_opinion < -100) { _opinion = -100; }
                if (_opinion > 100) { _opinion = 100; }
            }
        }

        private double _disposition = 0;
        public double Disposition
        {
            get { return _disposition; }
            set
            {
                _disposition = value;

                if (_disposition < -100) { _disposition = -100; }
                if (_disposition > 100) { _disposition = 100; }
            }
        }

        public Person() { }

        public override string ToString()
        {
            return Surname + ", " + ForeName;
        }

        public void LoadImages()
        {
            System.Diagnostics.Debug.WriteLine("LoadImages()");
            Rectangle rect = new Rectangle(xStart, yStart, 32, 32);
            _images = SpriteLoader.loadspritesI(ImageLoad.get(spriteSheet), rect, 32, 4);
        }

        public override Image toImage()
        {
            if (DirLookNum < 4)
            {
                _img = _images[DirLookNum];
            }

            if (_img == null)
            {
                return Properties.Resources.Square;
            }
            else
            {
                return _img;
            }

            //return Properties.Resources.Square;
        }
    }

    public class Options
    {
        public enum TimeOfDay
        {
            Morning,
            Evening,
            Night,
        }

        public static Random r = new Random();
        public static int Ticks = 32;

        # region Location
        public static List<String> Location = new List<string>
        {
            "Downtown",
            "Outskirts",
            "Suburbs",
        };

        public static string GetLocation()
        {
            int t = r.Next(0, Location.Count);
            return Location[t];
        }
        # endregion

        # region Place
        public static List<string> Place = new List<string>
        {
            "Home",
            "Hospital",
            "Office",
            "School",
            "Shop",
        };

        public static string GetPlace()
        {
            int t = r.Next(0, Place.Count);
            return Place[t];
        }
        # endregion

        # region Boy Forename
        public static List<string> BoyForename = new List<string>
        {
            "JAMES","JOHN","ROBERT","MICHAEL","WILLIAM","DAVID","RICHARD",
            "CHARLES","JOSEPH","THOMAS","CHRISTOPHER","DANIEL","PAUL","MARK",
            "DONALD","GEORGE","KENNETH","STEVEN","EDWARD","BRIAN","RONALD",
            "ANTHONY","KEVIN","JASON","MATTHEW","GARY","TIMOTHY","JOSE",
            "LARRY","JEFFREY","FRANK","SCOTT","ERIC","STEPHEN","ANDREW",
            "RAYMOND","GREGORY","JOSHUA","JERRY","DENNIS","WALTER","PATRICK",
            "PETER","HAROLD","DOUGLAS","HENRY","CARL","ARTHUR","RYAN","ROGER",
            "JOE","JUAN","JACK","ALBERT","JONATHAN","JUSTIN","TERRY","GERALD",
            "KEITH","SAMUEL","WILLIE","RALPH","LAWRENCE","NICHOLAS","ROY",
            "BENJAMIN","BRUCE","BRANDON","HARRY","FRED","WAYNE","BILLY",
            "STEVE","LOUIS","JEREMY","AARON","RANDY","HOWARD","EUGENE",
            "CARLOS","RUSSELL","BOBBY","VICTOR","MARTIN","ERNEST","PHILLIP",
            "TODD","JESSE","CRAIG","ALAN","SHAWN","CLARENCE","SEAN","PHILIP",
            "CHRIS","JOHNNY","EARL","JIMMY","ANTONIO","DANNY","BRYAN","TONY",
            "LUIS","MIKE","STANLEY","LEONARD","NATHAN","DALE","MANUEL",
            "RODNEY","CURTIS","NORMAN","ALLEN","MARVIN","VINCENT","GLENN",
            "JEFFERY","TRAVIS","JEFF","CHAD","JACOB","LEE","MELVIN","ALFRED",
            "KYLE","FRANCIS","BRADLEY", "ODRIC"
        };

        public static string GetBoyName()
        {
            int i = BoyForename.Count - 1;
            int t = r.Next(0, i);

            return BoyForename[t];
        }
        # endregion

        # region Girl Forename
        public static List<string> GirlForename = new List<string>
        {
            "MARY","PATRICIA","LINDA","BARBARA","ELIZABETH","JENNIFER","MARIA",
            "SUSAN","MARGARET","DOROTHY","LISA","NANCY","KAREN","BETTY","HELEN",
            "SANDRA","DONNA","CAROL","RUTH","SHARON","MICHELLE","LAURA","SARAH",
            "KIMBERLY","DEBORAH","JESSICA","SHIRLEY","CYNTHIA","ANGELA","MELISSA",
            "BRENDA","AMY","ANNA","REBECCA","VIRGINIA","KATHLEEN","PAMELA","MARTHA",
            "DEBRA","AMANDA","STEPHANIE","CAROLYN","CHRISTINE","MARIE","JANET",
            "CATHERINE","FRANCES","ANN","JOYCE","DIANE","ALICE","JULIE","HEATHER",
            "TERESA","DORIS","GLORIA","EVELYN","JEAN","CHERYL","MILDRED","KATHERINE",
            "JOAN","ASHLEY","JUDITH","ROSE","JANICE","KELLY","NICOLE","JUDY","CHRISTINA",
            "KATHY","THERESA","BEVERLY","DENISE","TAMMY","IRENE","JANE","LORI","RACHEL",
            "MARILYN","ANDREA","KATHRYN","LOUISE","SARA","ANNE","JACQUELINE","WANDA",
            "BONNIE","JULIA","RUBY","LOIS","TINA","PHYLLIS","NORMA","PAULA","DIANA",
            "ANNIE","LILLIAN","EMILY",
        };

        public static string GetGirlName()
        {
            int i = GirlForename.Count - 1;
            int t = r.Next(0, i);

            return GirlForename[t];
        }
        # endregion

        # region Surname
        public static List<string> Surname = new List<string>
        {
            "SMITH","JOHNSON","WILLIAMS","BROWN","JONES",
            "MILLER","DAVIS","GARCIA","RODRIGUEZ","WILSON",
            "MARTINEZ","ANDERSON","TAYLOR","THOMAS","HERNANDEZ",
            "MOORE","MARTIN","JACKSON","THOMPSON","WHITE",
            "LOPEZ","LEE","GONZALEZ","HARRIS","CLARK","LEWIS",
            "ROBINSON","WALKER","PEREZ","HALL","YOUNG","ALLEN",
            "SANCHEZ","WRIGHT","KING","SCOTT","GREEN","BAKER",
            "ADAMS","NELSON","HILL","RAMIREZ","CAMPBELL","MITCHELL",
            "ROBERTS","CARTER","PHILLIPS","EVANS","TURNER","TORRES",
            "PARKER","COLLINS","EDWARDS","STEWART","FLORES","MORRIS",
            "NGUYEN","MURPHY","RIVERA","COOK","ROGERS","MORGAN",
            "PETERSON","COOPER","REED","BAILEY","BELL","GOMEZ",
            "KELLY","HOWARD","WARD","COX","DIAZ","RICHARDSON","WOOD",
            "WATSON","BROOKS","BENNETT","GRAY","JAMES","REYES","CRUZ",
            "HUGHES","PRICE","MYERS","LONG","FOSTER","SANDERS","ROSS",
            "MORALES","POWELL","SULLIVAN","RUSSELL","ORTIZ","JENKINS",
            "GUTIERREZ","PERRY","BUTLER","BARNES","FISHER",
        };

        public static string GetSurname()
        {
            int i = Surname.Count - 1;
            int t = r.Next(0, i);

            return Surname[t];
        }
        # endregion

        public static int NormaliseInt(int x)
        {
            int result = x;

            if (result < 0)
            {
                result *= -1;
            }

            return result;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Right,
        Left,
        None
    }

    public delegate void Talk(string s);
}
