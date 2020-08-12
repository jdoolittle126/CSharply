
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using System.Transactions;

namespace CSharply
{

    /*
     * TODO
     * Menu Items
     */

    class Configuration
    {
        public String UniqueID { get; set; } = "";
        public ConsoleColor TextBackgroundColor { get; set; } = ConsoleColor.Black;
    }

    class PageConfiguration
    {
        //Color, Size, Scrollable, Border
    }

    class ContainerConfiguration
    {

    }

    class SectionConfiguration : Configuration{
        public enum TextAlign
        {
            Left,
            Center,
            Right
        }

        public ConsoleColor TextForegroundColor { get; set; } = ConsoleColor.White;
        
        public ConsoleColor BorderForegroundColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor BorderBackgroundColor { get; set; } = ConsoleColor.White;

        public Char BorderCharHorizontal { get; set; } = '-';
        public Char BorderCharVertical { get; set; } = '|';
        public Char BorderCharIntersect { get; set; } = '+';
        public int BorderWidth { get; set; } = 0;
        public int DrawPriority { get; set; } = 0;
        public bool TextWrap { get; set; } = false;
        public TextAlign TextAlignment { get; set; } = TextAlign.Left;

    }


    abstract class ConsoleElement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private Configuration Config = new Configuration();

        public ConsoleElement(int x, int y, int w, int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }

        public void SetConfig(Configuration config)
        {
            this.Config = config;
            if (config.UniqueID == "") config.UniqueID = this.GetType().ToString() + "-" + this.GetHashCode().ToString();
        }

    }

    class ConsoleSection : ConsoleElement, IComparable<ConsoleSection>
    {
        readonly Char[,] content;
        int cursor_pos_x, cursor_pos_y;

        public SectionConfiguration Config { get; set; } = new SectionConfiguration();

        public ConsoleSection(int x, int y, int w, int h) : base(x, y, w, h)
        {
            cursor_pos_x = 0;
            cursor_pos_y = 0;
            content = new Char[w,h];
            Clear();
        }

        public void SetConfig(SectionConfiguration config)
        {
            this.Config = config;
            if (config.UniqueID == "") config.UniqueID = this.GetType().ToString() + "-" + this.GetHashCode().ToString();
        }

        public void Draw(int offset_x = 0, int offset_y = 0, int container_max_x = -1, int container_max_y = -1)
        {
            if(container_max_x == -1) container_max_x = Console.BufferWidth;
            if (container_max_y == -1) container_max_y = Console.BufferHeight;

            container_max_x = Math.Clamp(container_max_x, 0, Console.BufferWidth);
            container_max_y = Math.Clamp(container_max_y, 0, Console.BufferHeight);

            Console.BackgroundColor = Config.TextBackgroundColor;
            Console.ForegroundColor = Config.TextForegroundColor;

            for (int _x = 0; _x < Width; _x++)
            {
                for(int _y = 0; _y < Height; _y++)
                {
                    if(_x + X + offset_x < container_max_x && _y + Y + offset_y < container_max_y)
                    {
                        Console.SetCursorPosition(_x + X + offset_x, _y + Y + offset_y);
                        Console.Write(content[_x, _y]);
                    }

                }
            }
            
        }

        public bool NewLine()
        {
            if(cursor_pos_y != (Height-1))
            {
                cursor_pos_y++;
                cursor_pos_x = 0;
                return true;
            } else
            {
                return false;
            }
        }

        public void SetPos(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Transform(int x, int y)
        {
            this.X += x;
            this.Y += y;
        }

        public void WriteText(string val)
        {

        }

        public void AppendText(string val)
        {

            //TODO Text Alignment
            int slots = (Config.TextWrap) ? Width - cursor_pos_x + (((Height-1) - cursor_pos_y) * Width) : Width - cursor_pos_x;
            if (val.Length > slots) val = val.Substring(0, slots);


            foreach(Char c in val)
            {
                content[cursor_pos_x, cursor_pos_y] = c;
                if (cursor_pos_x < Width && !Config.TextWrap || cursor_pos_x < Width-1) cursor_pos_x++;
                else if(Config.TextWrap)
                {
                    cursor_pos_x = 0;
                    cursor_pos_y++;
                }
            }

        }

        public void Clear()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    content[i, j] = ' ';
                }
            }
        }

        public int CompareTo([AllowNull] ConsoleSection other)
        {
            if (other == null) return 1;
            return this.Config.DrawPriority - other.Config.DrawPriority;

        }
    }

    class ConsoleContainer
    {
        List<ConsoleSection> sections;

        public ConsoleContainer()
        {
            sections = new List<ConsoleSection>();

        }

        public void AddComponent(ConsoleSection section)
        {
            sections.Add(section);
        }

        public bool RemoveComponent(ConsoleSection section)
        {
            return sections.Remove(section);
        }

        public bool RemoveComponent(string Id)
        {
            int size_old = sections.Count;
            foreach (ConsoleSection s in sections)
            {
                if (s.Config.UniqueID == Id) sections.Remove(s);
            }
            return (sections.Count != size_old);

        }

        public void Draw()
        {
            sections.Sort();
            foreach (ConsoleSection s in sections) s.Draw();
        }
    }


    class ConsolePage
    {
        public ConsoleContainer root = new ConsoleContainer();
        public ConsolePage()
        {
            
        }

        public void Update()
        {

        }




    }


}


