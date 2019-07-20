package com.example.mars;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

public class RouteViewPalace extends AppCompatActivity {

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

        wtn1.setText("경복궁");
        wts1.setText("경복궁입니다");
        wtn2.setText("창덕궁");
        wts2.setText("창덕궁입니다");
        wtn3.setText("북촌");
        wts3.setText("북촌입니다");
        wtn4.setText("경복궁");
        wts4.setText("경복궁입니다");
        wtn5.setText("경복궁");
        wts5.setText("경복궁입니다");
    }
}
