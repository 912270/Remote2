package com.example.remote2client;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;

import java.io.InputStream;
import java.net.Socket;

public class screenshot extends AppCompatActivity {

    Button pic;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_screenshot);

        pic = (Button)findViewById(R.id.button2);

        pic.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                String ppc = "SCREEN" + "^";

                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        try {
                            Connection.sendData(ppc.getBytes());
                            Connection.getPic();
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                }).start();

            }
        });

    }
}