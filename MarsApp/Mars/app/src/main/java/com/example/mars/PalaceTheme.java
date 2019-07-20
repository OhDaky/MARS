package com.example.mars;

import android.content.Intent;
import android.media.Image;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

public class PalaceTheme extends AppCompatActivity {

    ImageView palace_btn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_palace_theme);

        View textView = (View)findViewById(R.id.palace_overview);

        TextView n1 = (TextView)textView.findViewById(R.id.overview_name_01);
        TextView s1 = (TextView)textView.findViewById(R.id.overview_sub_01);
        TextView n2 = (TextView)textView.findViewById(R.id.overview_name_02);
        TextView s2 = (TextView)textView.findViewById(R.id.overview_sub_02);
        TextView n3 = (TextView)textView.findViewById(R.id.overview_name_03);
        TextView s3 = (TextView)textView.findViewById(R.id.overview_sub_03);
        TextView n4 = (TextView)textView.findViewById(R.id.overview_name_04);
        TextView s4 = (TextView)textView.findViewById(R.id.overview_sub_04);
        TextView n5 = (TextView)textView.findViewById(R.id.overview_name_05);
        TextView s5 = (TextView)textView.findViewById(R.id.overview_sub_05);

        n1.setText("경복궁");
        s1.setText("경복궁입니다");
        n2.setText("창덕궁");
        s2.setText("창덕궁입니다");
        n3.setText("북촌");
        s3.setText("북촌입니다");
        n4.setText("Gyeongbokgung");
        s4.setText("경복궁입니다");
        n5.setText("Gyeongbokgung");
        s5.setText("경복궁입니다");

        palace_btn = (ImageView) findViewById(R.id.btnOrder);
        palace_btn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(PalaceTheme.this, PalaceStartPos.class);
                startActivity (intent);
            }
        });
    }
}
