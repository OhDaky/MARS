package com.example.mars;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

public class NightTheme1 extends AppCompatActivity {

    ImageView night1_btn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_night_theme1);

        View textView = (View)findViewById(R.id.park_overview);

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

        n1.setText("Park1");
        s1.setText("Park1입니다");
        n2.setText("Park2");
        s2.setText("Park2입니다");
        n3.setText("Park2");
        s3.setText("Park2입니다");
        n4.setText("Park2");
        s4.setText("Park2입니다");
        n5.setText("Park2");
        s5.setText("Park2입니다");

        night1_btn = (ImageView) findViewById(R.id.btnOrder);
        night1_btn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(NightTheme1.this, Night1StartPos.class);
                startActivity (intent);
            }
        });
    }
}
