package com.example.mars;

import android.content.Intent;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.design.widget.TabLayout;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.MenuItem;

public class Spot5 extends AppCompatActivity {

    private TabLayout tabLayout;
    private ViewPager viewPager;
    private ViewPagerAdapter adapter;
    private BottomNavigationView bottomNavigationView; //바텀 네비게이션 뷰

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_spot5);

        bottomNavigationView = findViewById(R.id.bottomNavi);
        bottomNavigationView.setOnNavigationItemSelectedListener(new BottomNavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
                switch(menuItem.getItemId()){
                    case R.id.home:
                        Intent intent = new Intent(Spot5.this, Home5.class);
                        startActivity (intent);
                        break;
                }
                return true;
            }
        });

        tabLayout = (TabLayout) findViewById(R.id.tablayout_id);
        viewPager = (ViewPager) findViewById(R.id.viewpager_id);
        adapter = new ViewPagerAdapter(getSupportFragmentManager());

        adapter.AddFragment(new FragmentCafe5(), "");
        adapter.AddFragment(new FragmentPhoto5(), "");
        adapter.AddFragment(new FragmentFood5(), "");
        adapter.AddFragment(new FragmentActivity5(), "");
        adapter.AddFragment(new FragmentShop5(), "");

        viewPager.setAdapter(adapter);
        tabLayout.setupWithViewPager(viewPager);

        tabLayout.getTabAt(0).setIcon(R.drawable.icon_coffee);
        tabLayout.getTabAt(1).setIcon(R.drawable.icon_camera);
        tabLayout.getTabAt(2).setIcon(R.drawable.icon_spoon);
        tabLayout.getTabAt(3).setIcon(R.drawable.icon_activity);
        tabLayout.getTabAt(4).setIcon(R.drawable.icon_shopping);


    }
}
