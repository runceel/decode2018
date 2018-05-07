package com.microsoft.projectromedemo.databindings

import android.databinding.BindingAdapter
import android.databinding.ObservableArrayList
import android.widget.ListView
import com.microsoft.projectromedemo.adapters.DeviceArrayAdapter
import com.microsoft.projectromedemo.viewModels.Device

class DataBindingAdapters {
    companion object {
        @JvmStatic
        @BindingAdapter("list")
        fun setList(listView: ListView, list: ObservableArrayList<Device>) {
            listView.adapter = DeviceArrayAdapter(listView.context, list)
        }
    }
}