package com.microsoft.projectromedemo.viewModels

import android.content.Context
import android.content.Intent
import android.databinding.ObservableArrayList
import android.util.Log
import android.view.View
import android.widget.AdapterView
import com.microsoft.projectromedemo.ControllerActivity
import com.microsoft.projectromedemo.models.DeviceRepository

class DeviceListViewModel(private val context: Context, val deviceList: ObservableArrayList<Device>) {
    fun deviceSelected(parent: AdapterView<*>, view: View, position: Int, id: Long) {
        val intent = Intent(context, ControllerActivity::class.java).let {
            it.putExtra("id", DeviceRepository.devices[position].id)
        }
        context.startActivity(intent);
    }
}
