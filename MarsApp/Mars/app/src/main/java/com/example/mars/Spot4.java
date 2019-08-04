package com.example.mars;

import android.support.design.widget.TabLayout;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class Spot4 extends AppCompatActivity {

    private TabLayout tabLayout;
    private ViewPager viewPager;
    private ViewPagerAdapter adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_spot4);

        tabLayout = (TabLayout) findViewById(R.id.tablayout_id);
        viewPager = (ViewPager) findViewById(R.id.viewpager_id);
        adapter = new ViewPagerAdapter(getSupportFragmentManager());

        adapter.AddFragment(new FragmentCafe4(), "");
        adapter.AddFragment(new FragmentPhoto4(), "");
        adapter.AddFragment(new FragmentFood4(), "");
        adapter.AddFragment(new FragmentActivity4(), "");
        adapter.AddFragment(new FragmentShop4(), "");

        viewPager.setAdapter(adapter);
        tabLayout.setupWithViewPager(viewPager);

        tabLayout.getTabAt(0).setIcon(R.drawable.icon_coffee);
        tabLayout.getTabAt(1).setIcon(R.drawable.icon_camera);
        tabLayout.getTabAt(2).setIcon(R.drawable.icon_spoon);
        tabLayout.getTabAt(3).setIcon(R.drawable.icon_activity);
        tabLayout.getTabAt(4).setIcon(R.drawable.icon_shopping);


    }
}
