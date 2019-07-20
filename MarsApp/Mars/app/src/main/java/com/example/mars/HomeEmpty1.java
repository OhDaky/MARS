package com.example.mars;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.RelativeLayout;

public class HomeEmpty1 extends AppCompatActivity {

    ImageView touch;
    RelativeLayout touch2;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_empty1);

        touch = (ImageView)findViewById(R.id.background);
        touch.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(HomeEmpty1.this, HomeEmpty2.class);
                startActivity (intent);
            }
        });

        touch2 = (RelativeLayout)findViewById(R.id.choose);
        touch2.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(HomeEmpty1.this, HomeEmpty2.class);
                startActivity (intent);
            }
        });
    }
}
