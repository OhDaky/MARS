package com.example.mars;

import android.content.Intent;
import android.media.Image;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

public class PalaceTheme extends AppCompatActivity {

    ImageView palace_btn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_palace_theme);

        palace_btn = (ImageView) findViewById(R.id.btnOrder);
        palace_btn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(PalaceTheme.this, PalaceStartPos.class);
                startActivity (intent);
            }
        });
    }
}
