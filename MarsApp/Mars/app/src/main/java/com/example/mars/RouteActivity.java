package com.example.mars;

import android.animation.ArgbEvaluator;
import android.content.Intent;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

public class RouteActivity extends AppCompatActivity {

    ViewPager viewPager;
    Adapter adapter;
    List<Model> models;

    ImageView imgClick;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //액티비티랑 연결
        setContentView(R.layout.activity_route);

        models = new ArrayList<>();
        models.add(new Model(R.drawable.seoul_3599501_1920, "seoul_3599501_1920", ""));
        models.add(new Model(R.drawable.myeongdong_4119806_1920, "myeongdong_4119806_1920", ""));
        models.add(new Model(R.drawable.sunset_4165356_1920, "sunset_4165356_1920", ""));


        adapter = new Adapter(models, this);


        viewPager = findViewById(R.id.viewPager);
        viewPager.setAdapter(adapter);
        viewPager.setPadding(180, 0, 180, 0);
    }
}
