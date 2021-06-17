package com.example.remote2client;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.os.Bundle;
import android.os.SystemClock;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.Chronometer;
import android.widget.TextView;

public class touch extends AppCompatActivity {

    TextView touch;
    Button dop;
    Button kb;
    Button pkm;
    Button ud;
    Button langb;

    char del = '^';

    int x;
    int y;

    int x0;
    int y0;

    int xdif;
    int ydif;

    boolean clic = false;

    String sDown;
    String sMove;
    String sUp;

    TextView key;
    TextView langt;

    String lang;
    boolean lc = false;
    boolean lcc = false;

    boolean open = false;
    boolean langbool = false;

    Boolean change = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_touch);

        touch = (TextView) findViewById(R.id.textView);
        langt = (TextView) findViewById(R.id.textView3);
        kb = (Button) findViewById(R.id.button2);
        pkm = (Button) findViewById(R.id.button4);
        ud = (Button) findViewById(R.id.button3);
        dop = (Button) findViewById(R.id.button);
        langb = (Button) findViewById(R.id.button5);

        kb.setVisibility(View.GONE);
        pkm.setVisibility(View.GONE);
        ud.setVisibility(View.GONE);
        langb.setVisibility(View.GONE);

        kb.setClickable(false);
        pkm.setClickable(false);
        ud.setClickable(false);
        langb.setClickable(false);

        langt.setText("ENG");

        dop.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                if (open == false)
                {
                    kb.setVisibility(View.VISIBLE);
                    pkm.setVisibility(View.VISIBLE);
                    ud.setVisibility(View.VISIBLE);
                    langb.setVisibility(View.VISIBLE);

                    kb.setClickable(true);
                    pkm.setClickable(true);
                    ud.setClickable(true);
                    langb.setClickable(true);

                    open = true;
                }else
                {
                    kb.setVisibility(View.GONE);
                    pkm.setVisibility(View.GONE);
                    ud.setVisibility(View.GONE);
                    langb.setVisibility(View.GONE);

                    kb.setClickable(false);
                    pkm.setClickable(false);
                    ud.setClickable(false);
                    langb.setClickable(false);

                    open = false;
                }

            }
        });

        kb.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                InputMethodManager imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
                imm.toggleSoftInput(InputMethodManager.SHOW_FORCED, 0);

            }
        });

        pkm.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                send("PKM" + del);
            }
        });

        ud.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                if (change == false)
                {
                    send("UD ON" + del);
                    change = true;
                }
                else if (change == true)
                {
                    send("UD OFF" + del);
                    change = false;
                }
            }
        });

        langb.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                send("LANGCHANGE");
                if (langbool == false)
                {
                    langt.setText("RUS");
                    langbool = true;
                }else if (langbool == true)
                {
                    langt.setText("ENG");
                    langbool = false;
                }

            }
        });

        touch.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {

                x = (int)event.getX();
                y = (int)event.getY();
                String s = "";

                switch (event.getAction()) {
                    case MotionEvent.ACTION_DOWN: // нажатие
                        sDown = "Down: " + x + "," + y;
                        sMove = ""; sUp = "";

                        x0 = x;
                        y0 = y;

                        s = "TouchStart" + del;
                        //s = "TouchStop" + ".";
                        send(s);

                        break;
                    case MotionEvent.ACTION_MOVE: // движение
                        sMove = "Move: " + x + "," + y;

                        xdif = x - x0;
                        ydif = y - y0;

                        /*if (xdif < 1 && ydif < 1)
                        {
                            clic = true;
                        }*/

                        s = "Touch" + " " + xdif + " " + ydif + del;
                        send(s);

                        break;
                    case MotionEvent.ACTION_UP: // отпускание
                    case MotionEvent.ACTION_CANCEL:
                        sMove = "";
                        sUp = "Up: " + x + "," + y;


                        if (x - x0 < 1 && y - y0 < 1)
                        {
                            s = "CLICK" + del;
                            send(s);
                        }
                        /*if (clic == true)
                        {
                            s = "TouchClick" + ".";
                            send(s);
                        }
                        clic = false;*/

                        break;
                }
                touch.setText(sDown + "\n" + sMove + "\n" + sUp);

                return true;
            }
        });
    }

    void rus(String f)
    {
        if (f.equals("й"))
        {
            f = "q";
            lc = true;
        }else if (f.equals("ц"))
        {
            f = "w";
            lc = true;
        }else if (f.equals("у"))
        {
            f = "e";
            lc = true;
        }else if (f.equals("к"))
        {
            f = "r";
            lc = true;
        }else if (f.equals("е"))
        {
            f = "t";
            lc = true;
        }else if (f.equals("н"))
        {
            f = "y";
            lc = true;
        }else if (f.equals("г"))
        {
            f = "u";
            lc = true;
        }else if (f.equals("ш"))
        {
            f = "i";
            lc = true;
        }else if (f.equals("щ"))
        {
            f = "o";
            lc = true;
        }else if (f.equals("з"))
        {
            f = "p";
            lc = true;
        }else if (f.equals("х"))
        {
            f = "[";
            lc = true;
        }else if (f.equals("ъ"))
        {
            f = "]";
            lc = true;
        }else if (f.equals("ф"))
        {
            f = "a";
            lc = true;
        }else if (f.equals("ы"))
        {
            f = "s";
            lc = true;
        }else if (f.equals("в"))
        {
            f = "d";
            lc = true;
        }else if (f.equals("а"))
        {
            f = "f";
            lc = true;
        }else if (f.equals("п"))
        {
            f = "g";
            lc = true;
        }else if (f.equals("р"))
        {
            f = "h";
            lc = true;
        }else if (f.equals("о"))
        {
            f = "j";
            lc = true;
        }else if (f.equals("л"))
        {
            f = "k";
            lc = true;
        }else if (f.equals("д"))
        {
            f = "l";
            lc = true;
        }else if (f.equals("ж"))
        {
            f = ";";
            lc = true;
        }else if (f.equals("э"))
        {
            f = "\'";
            lc = true;
        }else if (f.equals("я"))
        {
            f = "z";
            lc = true;
        }else if (f.equals("ч"))
        {
            f = "x";
            lc = true;
        }else if (f.equals("с"))
        {
            f = "c";
            lc = true;
        }else if (f.equals("м"))
        {
            f = "v";
            lc = true;
        }else if (f.equals("и"))
        {
            f = "b";
            lc = true;
        }else if (f.equals("т"))
        {
            f = "n";
            lc = true;
        }else if (f.equals("ь"))
        {
            f = "m";
            lc = true;
        }else if (f.equals("б"))
        {
            f = ",";
            lc = true;
        }else if (f.equals("ю"))
        {
            f = ".";
            lc = true;
        }else
        {
            lc = false;
            lcc = false;
        }

        lang = f;
    }

    @Override
    public boolean dispatchKeyEvent(KeyEvent event) {

        key = (TextView) findViewById(R.id.textView2);

        String ks = event.getCharacters();
        if (ks != null)
        {
            rus(ks);
        }
        int kc = event.getKeyCode();

        key.setText("gg" + kc);

        if (kc == 67)
        {
            String ff = "Back" + del;
            send(ff);
        }
        else if (kc == 62)
        {
            String ff = "space" + del;
            send(ff);
        }
        else if (kc == 66)
        {
            String ff = "enter" + del;
            send(ff);
        }
        else
        {
            int keyaction = event.getAction();

            if (ks != null)
            {
                String language = "eng";

                /*if (lcc == false && lc == true)
                {
                    send("LangChange" + del);
                    lcc = true;
                }*/

                String ff = "Key" + " " + lang + del;
                send(ff);
                key.setText("abc=" + ks);
            }
            else if (kc != 0)
            {
                if(keyaction == KeyEvent.ACTION_DOWN)
                {
                    int keycode = event.getKeyCode();
                    String kf = event.getCharacters();
                    int keyunicode = event.getUnicodeChar(event.getMetaState() );
                    char ch = (char) keyunicode;

                    String ff = "Key" + " " + ch + " " + del;
                    send(ff);


                    //key.setText("num=" + ch);

                    //key.setText("DEBUG MESSAGE KEY=" + character + " KEYCODE=" +  keycode);

                    System.out.println("DEBUG MESSAGE KEY=" + ch + " KEYCODE=" +  keycode);
                }
            }
        }

        return super.dispatchKeyEvent(event);
    }

    void send(String s)
    {
        new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    Connection.sendData(s.getBytes());
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }).start();
    }
}