package com.example.mars;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;

public class NightTheme extends AppCompatActivity {

    ImageView night_btn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_night_theme);

        night_btn = (ImageView) findViewById(R.id.btnOrder);
        night_btn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(NightTheme.this, NightStartPos.class);
                startActivity (intent);
            }
        });
    }
}
