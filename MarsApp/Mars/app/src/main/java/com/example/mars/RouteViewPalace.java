package com.example.mars;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

public class RouteViewPalace extends AppCompatActivity {

    ImageView deoksu;
    ImageView cheong;
    ImageView gyeong;
    ImageView bukchon;
    ImageView chang;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_route_view_palace);

        View textView = (View)findViewById(R.id.palace_route_view);

        TextView wtn1 = (TextView)textView.findViewById(R.id.wtn1);
        TextView wts1 = (TextView)textView.findViewById(R.id.wts1);
        TextView wtn2 = (TextView)textView.findViewById(R.id.wtn2);
        TextView wts2 = (TextView)textView.findViewById(R.id.wts2);
        TextView wtn3 = (TextView)textView.findViewById(R.id.wtn3);
        TextView wts3 = (TextView)textView.findViewById(R.id.wts3);
        TextView wtn4 = (TextView)textView.findViewById(R.id.wtn4);
        TextView wts4 = (TextView)textView.findViewById(R.id.wts4);
        TextView wtn5 = (TextView)textView.findViewById(R.id.wtn5);
        TextView wts5 = (TextView)textView.findViewById(R.id.wts5);

        wtn1.setText("Deoksugung 덕수궁");
        wts1.setText("서울특별시 중구 정동 세종대로 99");
        wtn2.setText("Cheonggyecheon 청계천");
        wts2.setText("서울 종로구 무교로 37 (서린동)");
        wtn3.setText("Gyeongbokgung 경복궁");
        wts3.setText("서울특별시 종로구 세종로 사직로 161");
        wtn4.setText("Bukchon Hanok 북촌");
        wts4.setText("서울특별시 종로구 가회동 계동길 37");
        wtn5.setText("Changdeokgung 창덕궁");
        wts5.setText("서울특별시 종로구 와룡동 율곡로 99");

        deoksu = (ImageView)findViewById(R.id.deoksu);
        deoksu.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(RouteViewPalace.this, AboutDeoksu.class);
                startActivity (intent);
            }
        });

        cheong = (ImageView)findViewById(R.id.cheong);
        cheong.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(RouteViewPalace.this, AboutCheonggye.class);
                startActivity (intent);
            }
        });

        gyeong = (ImageView)findViewById(R.id.gyeongbok);
        gyeong.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(RouteViewPalace.this, AboutGyeongbok.class);
                startActivity (intent);
            }
        });

        bukchon = (ImageView)findViewById(R.id.bukchon);
        bukchon.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(RouteViewPalace.this, AboutBukchon.class);
                startActivity (intent);
            }
        });

        chang = (ImageView)findViewById(R.id.chandeok);
        chang.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(RouteViewPalace.this, AboutChangdeok.class);
                startActivity (intent);
            }
        });


    }
}
