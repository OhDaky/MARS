package com.example.mars;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

public class NightTheme extends AppCompatActivity {

    ImageView night_btn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_night_theme);

        View textView = (View)findViewById(R.id.night_overview);

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

        n1.setText("Night1");
        s1.setText("Night1입니다");
        n2.setText("Night2");
        s2.setText("Night2입니다");
        n3.setText("Night3");
        s3.setText("Night3입니다");
        n4.setText("Night4");
        s4.setText("Night4입니다");
        n5.setText("Night5");
        s5.setText("Night5입니다");

        night_btn = (ImageView) findViewById(R.id.btnOrder);
        night_btn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(NightTheme.this, NightStartPos.class);
                startActivity (intent);
            }
        });
    }
}
