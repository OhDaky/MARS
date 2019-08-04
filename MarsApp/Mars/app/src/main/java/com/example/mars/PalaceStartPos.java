package com.example.mars;

import android.animation.ArgbEvaluator;
import android.content.Intent;
import android.media.Image;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

public class PalaceStartPos extends AppCompatActivity {

    ImageView n_btn;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_palace_start_pos);

/*        View textView = (View)findViewById(R.id.g_start_item);

        TextView tv1 = (TextView)textView.findViewById(R.id.title);
        TextView tv2 = (TextView)textView.findViewById(R.id.desc);

        View imageView = (View)findViewById(R.id.g_start_item);

        ImageView iv = (ImageView)imageView.findViewById(R.id.image);

        iv.setImageResource(R.drawable.gyeongbokgung);

        tv1.setText("Deoksugung");
        tv2.setText("서울특별시 중구 정동 세종대로 99\n" + "Subway\n" +
                "- line 1,2, Cityhall st. gate 1, 5m\n" +
                "- line 5, Gwanghwamun st. gate 6, 12m\n" +
                "Hours\n" + "- 9:00 AM - 9:00 PM");*/


        n_btn = (ImageView) findViewById(R.id.btnOrder);
        n_btn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(PalaceStartPos.this, GBackground.class);
                startActivity (intent);
            }
        });

    }


}
