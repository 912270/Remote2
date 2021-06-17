using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Remote2_Server_
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int LoadKeyboardLayout(string pwszKLID, uint Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("User32.dll")]
        static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);

        [Flags]
        enum MouseFlags
        {
            Move = 0x0001, LeftDown = 0x0002, LeftUp = 0x0004, RightDown = 0x0008,
            RightUp = 0x0010, Absolute = 0x8000
        };

        public static TcpClient client;
        private static TcpListener listener;
        private static IPEndPoint ep;
        private static string ipString;
        private static char del = '^';

        static NetworkStream stream;

        static void Main(string[] args)
        {
            int ret = LoadKeyboardLayout("00000409", 1);
            PostMessage(GetForegroundWindow(), 0x50, 1, ret);
            //InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US"));
            pr1();
            //pr2();

        }

        static void pr2()
        {
            IPAddress[] localIp = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIp)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipString = address.ToString();
                }
            }

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipString), 1234);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine(@"  
                ===================================================  
                Started listening requests at: {0}:{1}  
                ===================================================",
            ep.Address, ep.Port);
            client = listener.AcceptTcpClient();
            Console.WriteLine("Connected to client! " + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) + " \n");
           
            while (client.Connected)
            {
                try
                {
                    //const int bytesize = 1024 * 1024;
                    const int bytesize = 1024 * 1024;
                    byte[] buffer = new byte[bytesize];
                    //string x = client.GetStream().Read(buffer, 0, bytesize).ToString();
                    var xi = client.GetStream().Read(buffer, 0, bytesize);
                    var data = ASCIIEncoding.ASCII.GetString(buffer);
                    string ss = data;

                    if (xi == 0)
                    {
                        Console.WriteLine($"Client disconnected!");
                        Console.ReadLine();
                        break;
                    }

                    //Console.WriteLine($"{ss}");

                    if (data.ToUpper().Contains("SLP2"))
                    {
                        Console.WriteLine("Pc is going to Sleep Mode!" + " \n");
                        //Sleep();
                    }
                }
                catch (Exception exc)
                {
                    client.Dispose();
                    client.Close();
                }
            }
        }

        static void pr1()
        {

            IPAddress[] localIp = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIp)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipString = address.ToString();
                }
            }

            ep = new IPEndPoint(IPAddress.Parse(ipString), 1234);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine(@"  
                ===================================================  
                Started listening requests at: {0}:{1}  
                ===================================================",
            ep.Address, ep.Port);
            Start();
            //client = listener.AcceptTcpClient();

            /*while (client.Connected)
            {
                try
                {
                    const int bytesize = 1024 * 1024;
                    byte[] buffer = new byte[bytesize];
                    var data = ASCIIEncoding.ASCII.GetString(buffer);

                    Console.WriteLine($"{data.ToUpper()}");
                }
                catch 
                {
                    client.Dispose();
                    client.Close();
                }
            }*/
        }

        static void Start()
        {
            int x = 0;
            int y = 0;

            client = listener.AcceptTcpClient();

            Console.WriteLine($"Connected to client!" + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()));


            while (client.Connected)
            {
                try
                {
                    const int bytesize = 1024 * 1024;
                    byte[] buffer = new byte[bytesize];
                    var xi = client.GetStream().Read(buffer, 0, bytesize);
                    var data = ASCIIEncoding.ASCII.GetString(buffer);

                    bool hasLetters = data.Any(char.IsLetter);

                    if (hasLetters == true)
                    {
                        string[] ss = data.Split(del);
                        Console.WriteLine(ss[0]);
                        //Console.WriteLine("321");
                    }

                    if (data.ToUpper().Contains("SLEEP"))
                    {

                        Console.WriteLine("Pc is going to Sleep Mode!");
                        Application.SetSuspendState(PowerState.Suspend, true, true);
                    }
                    else if (data.ToUpper().Contains("SHUTDOWN"))
                    {

                        Console.WriteLine("Pc is going to Shutdown!");
                        System.Diagnostics.Process.Start("Shutdown", "-s -t 10");
                    }
                    else if (data.ToUpper().Contains("TOUCHSTART"))
                    {
                        x = Cursor.Position.X;
                        y = Cursor.Position.Y;
                        Console.WriteLine($"X = {x}, Y = {y}");
                    }
                    else if (data.ToUpper().Contains("CLICK"))
                    {
                        //int xn = Cursor.Position.X;
                        //int yn = Cursor.Position.Y;

                        mouse_event(MouseFlags.LeftDown, 608, 431, 0, UIntPtr.Zero);
                        mouse_event(MouseFlags.LeftUp, 608, 431, 0, UIntPtr.Zero);

                        //mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, xn, yn, 0, UIntPtr.Zero);
                        //KeyClick.KeyDown(Keys.RButton);
                        //KeyClick.SendKey(VKeys.VK_RBUTTON);
                        Console.WriteLine("CLICK");
                    }
                    else if (data.ToUpper().Contains("KEY"))
                    {
                        string[] po = data.Split(' ', del);

                        char ff = Convert.ToChar(po[1]);

                        Key(ff);

                        Console.WriteLine("KEYPASSED");
                    }
                    else if (data.ToUpper().Contains("BACK"))
                    {
                        KeyClick.KeyDown(Keys.Back);
                        Console.WriteLine("BACKSPACE");
                    }
                    else if (data.ToUpper().Contains("POINT"))
                    {
                        KeyClick.KeyDown(Keys.OemPeriod);
                        Console.WriteLine("Ю");
                    }
                    else if (data.ToUpper().Contains("PKM"))
                    {
                        mouse_event(MouseFlags.RightDown, 608, 431, 0, UIntPtr.Zero);
                        mouse_event(MouseFlags.RightUp, 608, 431, 0, UIntPtr.Zero);
                        Console.WriteLine("pkm");
                    }
                    else if (data.ToUpper().Contains("UD"))
                    {
                        string[] po = data.Split(' ', del);

                        string ff = Convert.ToString(po[1]);

                        if (ff == "ON")
                        {
                            mouse_event(MouseFlags.LeftDown, 608, 431, 0, UIntPtr.Zero);
                        }
                        else if (ff == "OFF")
                        {
                            mouse_event(MouseFlags.LeftUp, 608, 431, 0, UIntPtr.Zero);
                        }

                        Console.WriteLine("pkm");
                    }
                    else if (data.ToUpper().Contains("LANGCHANGE"))
                    {
                        KeyClick.PressKeyCombination((byte)VKeys.VK_LEFTALT, (byte)VKeys.VK_SHIFT);
                        Console.WriteLine("Lang");
                    }
                    else if (data.ToUpper().Contains("SPACE"))
                    {
                        KeyClick.KeyDown(Keys.Space);
                        Console.WriteLine("Space");
                    }
                    else if (data.ToUpper().Contains("ENTER"))
                    {
                        KeyClick.KeyDown(Keys.Enter);
                        Console.WriteLine("Enter");
                    }
                    else if (data.ToUpper().Contains("TOUCH"))
                    {
                        string[] po = data.Split(' ', del);

                        int xdif = Convert.ToInt32(po[1]);
                        int ydif = Convert.ToInt32(po[2]);

                        Cursor.Position = new Point(x + xdif, y + ydif);
                        Console.WriteLine($"X = {x + xdif}, Y = {y + ydif}");
                    }

                    if (xi == 0)
                    {
                        Console.WriteLine($"Client disconnected!" + " \n");
                        break;
                    }
                }
                catch
                {
                    client.Dispose();
                    client.Close();
                }
            }
            Stop();
            Start();
        }

        static void Key(char ff)
        {
            if (ff == 'q')
            {
                KeyClick.KeyDown(Keys.Q);
            }else if (ff == 'w')
            {
                KeyClick.KeyDown(Keys.W);
            }
            else if (ff == 'e')
            {
                KeyClick.KeyDown(Keys.E);
            }
            else if (ff == 'r')
            {
                KeyClick.KeyDown(Keys.R);
            }
            else if (ff == 't')
            {
                KeyClick.KeyDown(Keys.T);
            }
            else if (ff == 'y')
            {
                KeyClick.KeyDown(Keys.Y);
            }
            else if (ff == 'u')
            {
                KeyClick.KeyDown(Keys.U);
            }
            else if (ff == 'i')
            {
                KeyClick.KeyDown(Keys.I);
            }
            else if (ff == 'o')
            {
                KeyClick.KeyDown(Keys.O);
            }
            else if (ff == 'p')
            {
                KeyClick.KeyDown(Keys.P);
            }
            else if (ff == '[')
            {
                KeyClick.KeyDown(Keys.OemOpenBrackets);
            }
            else if (ff == ']')
            {
                KeyClick.KeyDown(Keys.OemCloseBrackets);
            }
            else if (ff == 'a')
            {
                KeyClick.KeyDown(Keys.A);
            }
            else if (ff == 's')
            {
                KeyClick.KeyDown(Keys.S);
            }
            else if (ff == 'd')
            {
                KeyClick.KeyDown(Keys.D);
            }
            else if (ff == 'f')
            {
                KeyClick.KeyDown(Keys.F);
            }
            else if (ff == 'g')
            {
                KeyClick.KeyDown(Keys.G);
            }
            else if (ff == 'h')
            {
                KeyClick.KeyDown(Keys.H);
            }
            else if (ff == 'j')
            {
                KeyClick.KeyDown(Keys.J);
            }
            else if (ff == 'k')
            {
                KeyClick.KeyDown(Keys.K);
            }
            else if (ff == 'l')
            {
                KeyClick.KeyDown(Keys.L);
            }
            else if (ff == ';')
            {
                KeyClick.KeyDown(Keys.OemSemicolon);
            }
            else if (ff == '\'')
            {
                KeyClick.KeyDown(Keys.OemQuotes);
            }
            else if (ff == '\\')
            {
                KeyClick.KeyDown(Keys.OemBackslash);
            }
            else if (ff == 'z')
            {
                KeyClick.KeyDown(Keys.Z);
            }
            else if (ff == 'x')
            {
                KeyClick.KeyDown(Keys.X);
            }
            else if (ff == 'c')
            {
                KeyClick.KeyDown(Keys.C);
            }
            else if (ff == 'v')
            {
                KeyClick.KeyDown(Keys.V);
            }
            else if (ff == 'b')
            {
                KeyClick.KeyDown(Keys.B);
            }
            else if (ff == 'n')
            {
                KeyClick.KeyDown(Keys.N);
            }
            else if (ff == 'm')
            {
                KeyClick.KeyDown(Keys.M);
            }
            else if (ff == ',')
            {
                KeyClick.KeyDown(Keys.Oemcomma);
            }
            else if (ff == '.')
            {
                KeyClick.KeyDown(Keys.OemPeriod);
            }
            else if (ff == '/')
            {
                KeyClick.KeyDown(Keys.OemQuestion);
            }
            else if (ff == 'Q')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_Q);
            }
            else if (ff == 'W')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_W);
            }
            else if (ff == 'E')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_E);
            }
            else if (ff == 'R')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_R);
            }
            else if (ff == 'T')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_T);
            }
            else if (ff == 'Y')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_Y);
            }
            else if (ff == 'U')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_U);
            }
            else if (ff == 'I')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_I);
            }
            else if (ff == 'O')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_O);
            }
            else if (ff == 'P')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_P);
            }
            else if (ff == 'A')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_A);
            }
            else if (ff == 'S')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_S);
            }
            else if (ff == 'D')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_D);
            }
            else if (ff == 'F')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_F);
            }
            else if (ff == 'G')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_G);
            }
            else if (ff == 'H')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_H);
            }
            else if (ff == 'J')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_J);
            }
            else if (ff == 'K')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_K);
            }
            else if (ff == 'L')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_L);
            }
            else if (ff == 'Z')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_Z);
            }
            else if (ff == 'X')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_X);
            }
            else if (ff == 'C')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_C);
            }
            else if (ff == 'V')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_V);
            }
            else if (ff == 'B')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_B);
            }
            else if (ff == 'N')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_N);
            }
            else if (ff == 'M')
            {
                KeyClick.PressKeyCombination((byte)VKeys.VK_SHIFT, (byte)VKeys.VK_M);
            }
            else if (ff == '1')
            {
                KeyClick.KeyDown(Keys.D1);
            }
            else if (ff == '2')
            {
                KeyClick.KeyDown(Keys.D2);
            }
            else if (ff == '3')
            {
                KeyClick.KeyDown(Keys.D3);
            }
            else if (ff == '4')
            {
                KeyClick.KeyDown(Keys.D4);
            }
            else if (ff == '5')
            {
                KeyClick.KeyDown(Keys.D5);
            }
            else if (ff == '6')
            {
                KeyClick.KeyDown(Keys.D6);
            }
            else if (ff == '7')
            {
                KeyClick.KeyDown(Keys.D7);
            }
            else if (ff == '8')
            {
                KeyClick.KeyDown(Keys.D8);
            }
            else if (ff == '9')
            {
                KeyClick.KeyDown(Keys.D9);
            }
            else if (ff == '0')
            {
                KeyClick.KeyDown(Keys.D0);
            }
        }

        static void Stop()
        {
            client.Dispose();
            client.Close();
        }

        static class KeyClick
        {
            [DllImport("user32.dll")]
            private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
            private const int KEYEVENNTF_EXTENDEDKEY = 1;
            private const int KEYEVENNTF_KEYUP = 2;
            public static void KeyDown(Keys vKey)
            {
                keybd_event((byte)vKey, 0, KEYEVENNTF_EXTENDEDKEY, 0);
            }
            public static void KeyUp(Keys vKey)
            {
                keybd_event((byte)vKey, 0, KEYEVENNTF_EXTENDEDKEY | KEYEVENNTF_KEYUP, 0);
            }

            public static void PressKeyCombination(byte firstKeyCode, byte secondKeyCode)
            {
                keybd_event(firstKeyCode, 0, 0, 0); // Ctrl Press
                keybd_event(secondKeyCode, 0, 0, 0); // C Press
                keybd_event(secondKeyCode, 0, KEYEVENNTF_KEYUP, 0); // C Release
                keybd_event(firstKeyCode, 0, KEYEVENNTF_KEYUP, 0);
            }

            public static int SendKey(VKeys key)
            {
                keybd_event((byte)(key), 0x45, KEYEVENNTF_EXTENDEDKEY, 0);
                keybd_event((byte)(key), 0x45, KEYEVENNTF_EXTENDEDKEY | KEYEVENNTF_KEYUP, 0);

                return 0;
            }
        }
    }

    public enum VKeys
    {
        VK_LBUTTON = 1,
        VK_RBUTTON = 2,
        VK_CANCEL = 3,
        VK_MBUTTON = 4,
        VK_BACK = 8,
        VK_TAB = 9,
        VK_CLEAR = 12,
        VK_RETURN = 13,
        VK_SHIFT = 16,
        VK_CONTROL = 17,
        VK_LEFTALT = 18,
        VK_PAUSE = 19,
        VK_CAPITAL = 20,
        VK_ESCAPE = 27,
        VK_SPACE = 32,
        VK_PRIOR = 33,
        VK_NEXT = 34,
        VK_END = 35,
        VK_HOME = 36,
        VK_LEFT = 37,
        VK_UP = 38,
        VK_RIGHT = 39,
        VK_DOWN = 40,
        VK_SELECT = 41,
        VK_PRINT = 42,
        VK_EXECUTE = 43,
        VK_SNAPSHOT = 44,
        VK_INSERT = 45,
        VK_DELETE = 46,
        VK_HELP = 47,
        VK_0 = 48,
        VK_1 = 49,
        VK_2 = 50,
        VK_3 = 51,
        VK_4 = 52,
        VK_5 = 53,
        VK_6 = 54,
        VK_7 = 55,
        VK_8 = 56,
        VK_9 = 57,
        VK_A = 65,
        VK_B = 66,
        VK_C = 67,
        VK_D = 68,
        VK_E = 69,
        VK_F = 70,
        VK_G = 71,
        VK_H = 72,
        VK_I = 73,
        VK_J = 74,
        VK_K = 75,
        VK_L = 76,
        VK_M = 77,
        VK_N = 78,
        VK_O = 79,
        VK_P = 80,
        VK_Q = 81,
        VK_R = 82,
        VK_S = 83,
        VK_T = 84,
        VK_U = 85,
        VK_V = 86,
        VK_W = 87,
        VK_X = 88,
        VK_Y = 89,
        VK_Z = 90,
        VK_NUMPAD0 = 96,
        VK_NUMPAD1 = 97,
        VK_NUMPAD2 = 98,
        VK_NUMPAD3 = 99,
        VK_NUMPAD4 = 100,
        VK_NUMPAD5 = 101,
        VK_NUMPAD6 = 102,
        VK_NUMPAD7 = 103,
        VK_NUMPAD8 = 104,
        VK_NUMPAD9 = 105,
        VK_SEPARATOR = 108,
        VK_SUBTRACT = 109,
        VK_DECIMAL = 110,
        VK_DIVIDE = 111,
        VK_F1 = 112,
        VK_F2 = 113,
        VK_F3 = 114,
        VK_F4 = 115,
        VK_F5 = 116,
        VK_F6 = 117,
        VK_F7 = 118,
        VK_F8 = 119,
        VK_F9 = 120,
        VK_F10 = 121,
        VK_F11 = 122,
        VK_F12 = 123,
        VK_SCROLL = 145,
        VK_LSHIFT = 160,
        VK_RSHIFT = 161,
        VK_LCONTROL = 162,
        VK_RCONTROL = 163,
        VK_LMENU = 164,
        VK_RMENU = 165,
        VK_PLAY = 250,
        VK_ZOOM = 251,
        VK_LWinKey = 91,
        VK_RWinKey = 92,
        VK_OEM_MINUS = 0xBD,
        VK_OEM_PLUS = 187,
        VK_OEM_1 = 0xba,
        VK_OEM_COMMA = 0xbc,
        VK_OEM_PERIOD = 0xbe,
        VK_OEM_2 = 0xbf,
        VK_OEM3 = 0xc0,
        VK_OEM_4 = 0xdb,
        VK_OEM_5 = 0xdc,
        VK_OEM_6 = 0xdd,
        VK_OEM_7 = 0xde,
        VK_OEM_8 = 0xdf,
        VK_NONE = 255
    };
}
