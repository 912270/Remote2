package com.example.remote2client;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;

public class Menu extends AppCompatActivity {

    Button km;
    Button m;
    Button ss;
    Button sl;
    Button cl;
    Button sd;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_menu);

        km = (Button) findViewById(R.id.KeyTouch);
        m = (Button) findViewById(R.id.FileManager);
        ss = (Button) findViewById(R.id.Screenshot);
        sl = (Button) findViewById(R.id.button);
        cl = (Button) findViewById(R.id.button4);
        sd = (Button) findViewById(R.id.button3);

        km.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                Intent MA = new Intent(getApplicationContext(), touch.class);
                startActivity(MA);

            }
        });

        m.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                //Intent MA = new Intent(getApplicationContext(), touch.class);
                //startActivity(MA);

            }
        });

        ss.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                Intent MA = new Intent(getApplicationContext(), screenshot.class);
                startActivity(MA);

            }
        });

        sl.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                String sf = "SLEEP" + "^";
                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        try {
                            Connection.sendData(sf.getBytes());
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                }).start();

            }
        });

        cl.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                Connection.closeConnection();
                Intent MA = new Intent(getApplicationContext(), NewConnect.class);
                startActivity(MA);

            }
        });

        sd.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                String sf = "SHUTDOWN" + "^";
                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        try {
                            Connection.sendData(sf.getBytes());
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                }).start();
                Intent MA = new Intent(getApplicationContext(), NewConnect.class);
                startActivity(MA);

            }
        });
    }
}