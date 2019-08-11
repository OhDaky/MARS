package com.example.mars;

import android.content.Intent;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;

public class Home5 extends AppCompatActivity {

    ImageView click;
    ImageView click2;
    private BottomNavigationView bottomNavigationView; //바텀 네비게이션 뷰

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home5);

        bottomNavigationView = findViewById(R.id.bottomNavi);
        bottomNavigationView.setOnNavigationItemSelectedListener(new BottomNavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
                switch (menuItem.getItemId()) {
                    case R.id.home:
                        Intent intent = new Intent(Home5.this, Home5.class);
                        startActivity(intent);
                        break;
                }
                return true;
            }
        });

        click = (ImageView) findViewById(R.id.theme);
        click.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(Home5.this, RouteViewPalace.class);
                startActivity(intent);
            }
        });

        click2 = (ImageView) findViewById(R.id.spot);
        click2.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(Home5.this, Spot5.class);
                startActivity(intent);
            }
        });
    }
}
