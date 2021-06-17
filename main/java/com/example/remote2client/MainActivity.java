package com.example.remote2client;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

public class MainActivity extends AppCompatActivity {

    Button NC;
    Button FC;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        NC = (Button) findViewById(R.id.nc);
        FC = (Button) findViewById(R.id.fc);

        NC.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent MA = new Intent(getApplicationContext(), NewConnect.class);
                startActivity(MA);
            }
        });
    }
}