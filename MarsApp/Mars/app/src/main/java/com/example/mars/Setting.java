package com.example.mars;

import android.content.Intent;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.MenuItem;

public class Setting extends AppCompatActivity {

    private BottomNavigationView bottomNavigationView; //바텀 네비게이션 뷰

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_setting);

        bottomNavigationView = findViewById(R.id.bottomNavi);
        bottomNavigationView.setOnNavigationItemSelectedListener(new BottomNavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
                switch(menuItem.getItemId()){
                    case R.id.map:
                        Intent intent = new Intent(Setting.this, Map.class);
                        startActivity (intent);
                        break;
                    case R.id.home:
                        Intent intent2 = new Intent(Setting.this, Home.class);
                        startActivity (intent2);
                        break;
                    case R.id.next:
                        Intent intent3 = new Intent(Setting.this, Setting.class);
                        startActivity (intent3);
                        break;
                }
                return true;
            }
        });
    }
}
