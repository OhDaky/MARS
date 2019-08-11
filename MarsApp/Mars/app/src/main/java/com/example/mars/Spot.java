package com.example.mars;

import android.content.Intent;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.design.widget.TabLayout;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.MenuItem;

public class Spot extends AppCompatActivity {

    private TabLayout tabLayout;
    private ViewPager viewPager;
    private ViewPagerAdapter adapter;
    private BottomNavigationView bottomNavigationView; //바텀 네비게이션 뷰



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_spot);

        bottomNavigationView = findViewById(R.id.bottomNavi);
        bottomNavigationView.setOnNavigationItemSelectedListener(new BottomNavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
                switch(menuItem.getItemId()){
                    case R.id.map:
                        Intent intent = new Intent(Spot.this, Map.class);
                        startActivity (intent);
                        break;
                    case R.id.home:
                        Intent intent2 = new Intent(Spot.this, Home.class);
                        startActivity (intent2);
                        break;
                    case R.id.next:
                        Intent intent3 = new Intent(Spot.this, Home2.class);
                        startActivity (intent3);
                        break;
                }
                return true;
            }
        });

        tabLayout = (TabLayout) findViewById(R.id.tablayout_id);
        viewPager = (ViewPager) findViewById(R.id.viewpager_id);
        adapter = new ViewPagerAdapter(getSupportFragmentManager());

        adapter.AddFragment(new FragmentCall(), "");
        adapter.AddFragment(new FragmentContact(), "");
        adapter.AddFragment(new FragmentFav(), "");
        adapter.AddFragment(new FragmentActivity(), "");
        adapter.AddFragment(new FragmentShop(), "");

        viewPager.setAdapter(adapter);
        tabLayout.setupWithViewPager(viewPager);

        tabLayout.getTabAt(0).setIcon(R.drawable.icon_coffee);
        tabLayout.getTabAt(1).setIcon(R.drawable.icon_camera);
        tabLayout.getTabAt(2).setIcon(R.drawable.icon_spoon);
        tabLayout.getTabAt(3).setIcon(R.drawable.icon_activity);
        tabLayout.getTabAt(4).setIcon(R.drawable.icon_shopping);


    }
}
