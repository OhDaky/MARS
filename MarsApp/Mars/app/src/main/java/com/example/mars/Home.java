package com.example.mars;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.RelativeLayout;
import android.widget.TextView;

public class Home extends AppCompatActivity {

    RelativeLayout click;
    RelativeLayout click2;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home);

        click = (RelativeLayout) findViewById(R.id.clickTheme);
        click.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(Home.this, RouteViewPalace.class);
                startActivity (intent);
            }
        });

        click2 = (RelativeLayout) findViewById(R.id.clickSpot);
        click2.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(Home.this, Spot4.class);
                startActivity (intent);
            }
        });
    }
}
