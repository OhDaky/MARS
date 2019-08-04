package com.example.mars;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;

public class Home_Empty extends AppCompatActivity {

    RelativeLayout touch;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home__empty);

        touch = (RelativeLayout)findViewById(R.id.click);
        touch.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(Home_Empty.this, HomeEmpty1.class);
                startActivity (intent);
            }
        });
    }
}
