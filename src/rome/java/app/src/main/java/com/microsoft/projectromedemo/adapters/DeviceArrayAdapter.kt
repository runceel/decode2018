package com.microsoft.projectromedemo.adapters

import android.content.Context
import android.databinding.DataBindingUtil
import android.databinding.ObservableArrayList
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import com.microsoft.projectromedemo.R
import com.microsoft.projectromedemo.databinding.DeviceListviewItemBinding
import com.microsoft.projectromedemo.viewModels.Device

class DeviceArrayAdapter(context: Context, devices: ObservableArrayList<Device>): ArrayAdapter<Device>(context, 0, devices) {
    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
        lateinit var binding: DeviceListviewItemBinding;
        if (convertView == null) {
            val inflater = LayoutInflater.from(context)
            binding = DataBindingUtil.inflate(inflater, R.layout.device_listview_item, parent, false)
            binding.root.tag = binding
        } else {
            binding = convertView.tag as DeviceListviewItemBinding
        }

        binding.viewModel = getItem(position)
        return binding.root
    }
}