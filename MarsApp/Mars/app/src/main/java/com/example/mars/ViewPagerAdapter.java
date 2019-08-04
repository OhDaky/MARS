package com.example.mars;


import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import java.util.ArrayList;
import java.util.List;

public class ViewPagerAdapter extends FragmentPagerAdapter {

    private final List<Fragment> lsFragment = new ArrayList<>();
    private final List<String> lsTitles = new ArrayList<>();

    public ViewPagerAdapter(FragmentManager fm) {
        super(fm);
    }

    @Override
    public Fragment getItem(int i) {
        return lsFragment.get(i);
    }

    @Override
    public int getCount() {
        return lsTitles.size();
    }

    @Nullable
    @Override
    public CharSequence getPageTitle(int position) {
        return lsTitles.get(position);
    }

    public void AddFragment(Fragment fragment, String title) {

        lsFragment.add(fragment);
        lsTitles.add(title);
    }

}
