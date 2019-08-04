package com.example.mars;

import android.animation.ArgbEvaluator;
import android.content.Intent;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import org.w3c.dom.Text;

import java.util.ArrayList;
import java.util.List;

public class RouteActivity extends AppCompatActivity {

    ViewPager viewPager;
    Adapter adapter;
    List<Model> models;

    ImageView imgClick;

    Integer[] colors = null;
    ArgbEvaluator argbEvaluator = new ArgbEvaluator();
    Class[] theme = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //액티비티랑 연결
        setContentView(R.layout.activity_route);

        models = new ArrayList<>();
        models.add(new Model(R.drawable.choose_palace, "choose_palace", ""));
        models.add(new Model(R.drawable.choose_night, "choose_night", ""));
        models.add(new Model(R.drawable.choose_museum, "choose_museum", ""));


        adapter = new Adapter(models, this);


        viewPager = findViewById(R.id.viewPager);
        viewPager.setAdapter(adapter);
        viewPager.setPadding(150, 0, 150, 0);

        Integer[] colors_temp = {
                getResources().getColor(R.color.color1),
                getResources().getColor(R.color.color2),
                getResources().getColor(R.color.color3)
        };

        Class[] theme_temp = {
                PalaceTheme.class,
                NightTheme.class,
                NightTheme1.class
        };


        colors = colors_temp;
        theme = theme_temp;

        viewPager.setOnPageChangeListener(new ViewPager.OnPageChangeListener() {
            @Override
            public void onPageScrolled(final int position, float positionOffset, int positionOffsetPixels) {

                if (position < (adapter.getCount() - 1) && position < (colors.length - 1) && position < (theme.length - 1)) {
                    viewPager.setBackgroundColor((Integer) argbEvaluator.evaluate(positionOffset, colors[position], colors[position + 1]));
                    imgClick = (ImageView) findViewById(R.id.btnOrder);
                    imgClick.setOnClickListener(new View.OnClickListener() {

                        @Override
                        public void onClick(View v) {
                            Intent intent = new Intent(RouteActivity.this, theme[position]);
                            startActivity(intent);
                        }
                    });

                }else {
                    viewPager.setBackgroundColor(colors[colors.length - 1]);
                    imgClick = (ImageView) findViewById(R.id.btnOrder);
                    imgClick.setOnClickListener(new View.OnClickListener() {

                        @Override
                        public void onClick(View v) {
                            Intent intent = new Intent(RouteActivity.this, theme[theme.length - 1]);
                            startActivity(intent);
                        }
                    });
                }
            }

            @Override
            public void onPageSelected(int i) {

            }

            @Override
            public void onPageScrollStateChanged(int i) {

            }
        });




    }
}
