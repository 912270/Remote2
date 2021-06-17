package com.example.remote2client;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

public class Send extends AppCompatActivity {

    Button send;
    Button close;
    EditText message;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_send);

        send = (Button) findViewById(R.id.send);
        close = (Button) findViewById(R.id.close);
        message = (EditText) findViewById(R.id.message);

        send.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String s = message.getText().toString() + ".";
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
        });

        close.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Connection.closeConnection();
            }
        });
    }
}