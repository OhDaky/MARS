package com.example.mars;

import android.content.Intent;
import android.media.Image;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

public class GBackground extends AppCompatActivity {

    ImageView btn_start;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_gbackground);


        btn_start = (ImageView) findViewById(R.id.btnOrder);
        btn_start.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(GBackground.this, Home.class);
                startActivity (intent);
            }
        });
    }
}
