package com.example.remote2client;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

public class NewConnect extends AppCompatActivity {

    Button Con;
    EditText IP;
    EditText Port;

    public static Connection con;

    boolean pass;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_new_connect);

        Con = (Button) findViewById(R.id.butCon);
        IP = (EditText) findViewById(R.id.editIP);
        Port = (EditText) findViewById(R.id.editPort);

        Con.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                String ip = IP.getText().toString();
                int port = Integer.parseInt(Port.getText().toString());

                if (ip.length() < 8)
                {
                    Toast succon = Toast.makeText(getApplicationContext(),
                            "IP-адрес введен неверно!", Toast.LENGTH_SHORT);
                    succon.show();
                } else if (port < 1000)
                {
                    Toast succon = Toast.makeText(getApplicationContext(),
                            "Port введен неверно!", Toast.LENGTH_SHORT);
                    succon.show();
                } else
                {
                    con = new Connection(ip, port);

                    Thread conthread = new Thread(new Runnable() {
                        @Override
                        public void run() {
                            try
                            {
                                con.openConnection();
                                pass = true;

                            } catch (Exception e) {
                                e.printStackTrace();
                                pass = false;
                            }
                        }
                    });
                    conthread.start();
                    try {
                        Thread.sleep(1000);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }

                    if (pass == true)
                    {
                        Toast succon = Toast.makeText(getApplicationContext(),
                                "Подключение успешно!", Toast.LENGTH_SHORT);
                        succon.show();

                        Intent MA = new Intent(getApplicationContext(), Menu.class);
                        //Intent MA = new Intent(getApplicationContext(), Send.class);
                        startActivity(MA);
                    }
                    else
                    {
                        Toast failcon = Toast.makeText(getApplicationContext(),
                                "Подключение не удалось!", Toast.LENGTH_SHORT);
                        failcon.show();
                    }
                }


            }
        });
    }
}