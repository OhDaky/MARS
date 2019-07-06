package com.example.mars;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;

public class NightTheme1 extends AppCompatActivity {

    ImageView night1_btn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_night_theme1);

        night1_btn = (ImageView) findViewById(R.id.btnOrder);
        night1_btn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(NightTheme1.this, Night1StartPos.class);
                startActivity (intent);
            }
        });
    }
}
